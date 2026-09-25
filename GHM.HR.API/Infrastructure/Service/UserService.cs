
using GHM.HR.Domain.IRepository;
using GHM.HR.Domain.IServices;
using GHM.HR.Domain.ModelMetas;
using GHM.HR.Domain.Models;
using GHM.HR.Domain.ViewModels;
using GHM.Infrastructure.Constants;
using GHM.Infrastructure.Extensions;
using GHM.Infrastructure.Helpers;
using GHM.Infrastructure.IServices;
using GHM.Infrastructure.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GHM.HR.Domain;
using GHM.HR.API.Domain.IRepository;
using GHM.HR.API.Domain.Resources;
using GHM.HR.API.Infrastructure.Data;
using GHM.HR.Domain.Constants;

namespace GHM.HR.Infrastructure.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IPositionRepository _positionRepository;
        private readonly IResourceService<GhmHRResource> _ghmHRResource;
        private readonly IDbSession _dbSession;

        public UserService(IUserRepository userRepository,
             IDepartmentRepository departmentRepository,
             IPositionRepository positionRepository,
             IConfiguration configuration,
             IResourceService<GhmHRResource> ghmHRResource,
             IDbSession dbSession)
        {
            _userRepository = userRepository;
            _departmentRepository = departmentRepository;
            _positionRepository = positionRepository;
            _ghmHRResource = ghmHRResource;
            _dbSession = dbSession;
        }


        /// <summary>
        /// Resolve phòng ban (kèm ExpireDays), vị trí, chức danh và gán vào <paramref name="target"/>.
        /// Trả về null nếu hợp lệ, ngược lại trả về lỗi.
        /// </summary>
        private async Task<ActionResultResponse<string>> ResolveOrganizationAsync(User target, UserMeta userMeta)
        {
            var department = await _departmentRepository.GetInfoAsync(userMeta.DepartmentId);
      

            if (department == null)
            {
                target.DepartmentId = null;
                target.DepartmentPath = null;
                target.DepartmentName = null;
            }
            else
            {
                target.DepartmentId = department.Id;
                target.DepartmentPath = department.IdPath;
                target.DepartmentName = department.Name;

                // Chỉ set ExpireDays từ department khi user không tự chỉ định
                if (!userMeta.ExpireDays.HasValue)
                    target.ExpireDays = department.ExpireDays;
            }

            var position = await _positionRepository.GetInfoAsync(userMeta.PositionId);
            if (position == null)
                return new ActionResultResponse<string>(-2,
                    _ghmHRResource.GetString(ErrorMessage.NotExists, _ghmHRResource.GetString("Position")));
            target.PositionId = position.Id;
            target.PositionName = position.Name;

            return null;
        }

        public async Task<List<UserBirthdayViewModel>> SearchUserBirthdayAsync(string tenantId, string companyId, int? month)
        {
            return await _userRepository.SearchUserBirthdayAsync(tenantId, companyId, month);
        }

        public async Task<List<UserResignedViewModel>> SearchUserResignedAsync(string tenantId, string companyId, bool isAll, int month)
        {
            return await _userRepository.SearchUserResignedAsync(tenantId, companyId, isAll, month);
        }


        // <summary>
        /// Kiểm tra giới hạn nghỉ phép ứng trước / nghỉ bù. Trả về null nếu hợp lệ.
        /// </summary>
        private ActionResultResponse<string> ValidateLeaveGranted(UserMeta userMeta)
        {
            if (userMeta.AdvanceLeaveGranted > UserLeavePolicy.MaxAdvanceLeaveDays)
                return new ActionResultResponse<string>(-99,
                    _ghmHRResource.GetString("AdvanceLeaveGranted must not exceed 12 days."));

            if (userMeta.CompLeaveGranted > UserLeavePolicy.MaxCompLeaveMinutes)
                return new ActionResultResponse<string>(-99,
                    _ghmHRResource.GetString("CompLeaveGranted must not exceed 480 minutes."));

            return null;
        }

        public async Task<ActionResultResponse<string>> InsertAsync(string tenantId, string creatorId, string creatorFullName, string creatorAvatar, UserMeta userMeta)
        {
            var userId = Guid.NewGuid().ToString();

            var isCodeExit = await _userRepository.CheckExistsByCodeAsync(tenantId, userId, userMeta.Code?.Trim());
            if (isCodeExit)
                return new ActionResultResponse<string>(-99, _ghmHRResource.GetString(ErrorMessage.Exists, _ghmHRResource.GetString("Code")));

            var isUserNameExit = await _userRepository.CheckExistsByUserNameAsync(tenantId, userId, userMeta.UserName?.ToLower().StripVietnameseChars().Trim());
            if (isUserNameExit)
                return new ActionResultResponse<string>(-4, _ghmHRResource.GetString(ErrorMessage.Exists, _ghmHRResource.GetString("UserName")));

            if (!string.IsNullOrEmpty(userMeta.PhoneNumber))
            {
                var isPhoneNumberExit = await _userRepository.CheckExistsByPhoneNumberAsync(tenantId, userId, userMeta.PhoneNumber?.Trim());
                if (isPhoneNumberExit)
                    return new ActionResultResponse<string>(-3, _ghmHRResource.GetString(ErrorMessage.Exists, _ghmHRResource.GetString("PhoneNumber")));
            }

            if (!string.IsNullOrEmpty(userMeta.Email))
            {
                var isEmailExit = await _userRepository.CheckExistsByEmailAsync(tenantId, userId, userMeta.Email?.ToLower().StripVietnameseChars().Trim());
                if (isEmailExit)
                    return new ActionResultResponse<string>(-7, _ghmHRResource.GetString(ErrorMessage.Exists, _ghmHRResource.GetString("Email")));
            }

            var leaveValidation = ValidateLeaveGranted(userMeta);
            if (leaveValidation != null)
                return leaveValidation;

            var userInsert = new User
            {
                Id = userId,
                ConcurrencyStamp = userId,
                CompanyId = userMeta.CompanyId,
                Code = userMeta.Code?.Trim(),
                FullName = userMeta.FullName?.Trim(),
                FirstName = Common.GetFirstName(userMeta.FullName.Trim()),
                MiddleName = Common.GetMiddleName(userMeta.FullName.Trim()),
                LastName = Common.GetLastName(userMeta.FullName.Trim()),
                UserName = userMeta.UserName?.Trim(),
                Birthday = userMeta.Birthday,
                Gender = userMeta.Gender,
                Avatar = userMeta.Avatar?.Trim(),
                JoinedDate = userMeta.JoinedDate,
                WorkingForm = userMeta.WorkingForm,
                InsuranceCode = userMeta.InsuranceCode?.Trim(),
                InsuranceStatus = userMeta.InsuranceStatus,
                InsuranceName = userMeta.InsuranceName?.Trim(),
                IsActive = userMeta.IsActive,
                PhoneNumber = userMeta.PhoneNumber ?? null,
                ContractCode = userMeta.ContractCode,
                Email = userMeta.Email?.Trim() ?? null,
                Note = userMeta.Note,
                IsDelete = false,
                TenantId = tenantId,
                CreateTime = DateTime.Now,
                CreatorId = creatorId,
                CreatorFullName = creatorFullName,
                ManagerUserId = userMeta.ManagerUserId,
                Month = userMeta.Month ?? null,
                OfficalDate = userMeta.Status == Domain.Constants.UserStatus.Official ? userMeta.OfficalDate : null,
                AdvanceLeaveGranted = userMeta.AdvanceLeaveGranted ?? null,
                CompLeaveGranted = userMeta.CompLeaveGranted ?? null,
                ContractExpirationDate = userMeta.Month != Domain.Constants.TypeMonth.IndefiniteTerm ? userMeta.ContractExpirationDate : null,
                PersonnelStatus = userMeta.PersonnelStatus,
                ExpireDays = userMeta.ExpireDays,
            };


            if (userMeta.PersonnelStatus == PersonnelStatus.Resigned)
            {
                userInsert.IsActive = false;
            }
            else
            {
                userInsert.IsActive = true;
            }

            var organization = await ResolveOrganizationAsync(userInsert, userMeta);
            if (organization != null)
                return organization;


            if (!string.IsNullOrEmpty(userMeta.ManagerUserId))
            {
                var managerUser = await _userRepository.GetInfoAsync(userMeta.ManagerUserId);
                if (managerUser == null || managerUser.IsActive == false)
                    return new ActionResultResponse<string>(-2, _ghmHRResource.GetString(ErrorMessage.NotExists, _ghmHRResource.GetString("ManagerUser")));
                userInsert.ManagerUserId = managerUser.Id;
                userInsert.ManagerFullName = managerUser.FullName;
            }

            var result = await _userRepository.InsertAsync(userInsert);

            if (result <= 0)
                return new ActionResultResponse<string>(result, _ghmHRResource.GetString(ErrorMessage.SomethingWentWrong));
            return new ActionResultResponse<string>(result, _ghmHRResource.GetString("UserAddedSuccessfully"));

        }

        public async Task<ActionResultResponse<string>> UpdateAsync(string tenantId, string lastUpdateUserId, string lastUpdateFullName, string lastUpdateAvatar, string id, UserMeta userMeta)
        {
            var info = await _userRepository.GetInfoAsync(id);
            if (info == null)
                return new ActionResultResponse<string>(-2, _ghmHRResource.GetString(ErrorMessage.NotExists, _ghmHRResource.GetString("User")));

            var isUserNameExit = await _userRepository.CheckExistsByUserNameAsync(tenantId, id, userMeta.UserName?.ToLower().StripVietnameseChars().Trim());
            if (isUserNameExit)
                return new ActionResultResponse<string>(-4, _ghmHRResource.GetString(ErrorMessage.Exists, _ghmHRResource.GetString("UserName")));


            if (!string.IsNullOrEmpty(userMeta.PhoneNumber))
            {
                var isPhoneNumberExit = await _userRepository.CheckExistsByPhoneNumberAsync(tenantId, id, userMeta.PhoneNumber?.Trim());
                if (isPhoneNumberExit)
                    return new ActionResultResponse<string>(-3, _ghmHRResource.GetString(ErrorMessage.Exists, _ghmHRResource.GetString("PhoneNumber")));
            }

            if (!string.IsNullOrEmpty(userMeta.Email))
            {
                var isEmailExit = await _userRepository.CheckExistsByEmailAsync(tenantId, info.Id, userMeta.Email?.ToLower().StripVietnameseChars().Trim());
                if (isEmailExit)
                    return new ActionResultResponse<string>(-7, _ghmHRResource.GetString(ErrorMessage.Exists, _ghmHRResource.GetString("Email")));
            }

            var leaveValidation = ValidateLeaveGranted(userMeta);
            if (leaveValidation != null)
                return leaveValidation;

            info.CompanyId = userMeta.CompanyId;
            info.FullName = userMeta.FullName?.Trim();
            info.FirstName = Common.GetFirstName(userMeta.FullName.Trim());
            info.MiddleName = Common.GetMiddleName(userMeta.FullName.Trim());
            info.LastName = Common.GetLastName(userMeta.FullName.Trim());
            info.Birthday = userMeta.Birthday;
            info.Avatar = userMeta.Avatar?.Trim();
            info.Gender = userMeta.Gender;
            info.JoinedDate = userMeta.JoinedDate;
            info.Status = userMeta.Status;
            info.PhoneNumber = userMeta.PhoneNumber?.Trim() ?? null;
            info.Email = userMeta.Email?.ToLower().StripVietnameseChars().Trim() ?? null;
            info.WorkingForm = userMeta.WorkingForm;
            info.InsuranceCode = userMeta.InsuranceCode?.Trim();
            info.InsuranceStatus = userMeta.InsuranceStatus;
            info.InsuranceName = userMeta.InsuranceName?.Trim();
            info.Note = userMeta.Note;
            info.ConcurrencyStamp = Guid.NewGuid().ToString();
            info.LastUpdate = DateTime.Now;
            info.LastUpdateUserId = lastUpdateUserId;
            info.LastUpdateFullName = lastUpdateFullName;
            info.Month = userMeta.Month ?? null;
            info.OfficalDate = userMeta.Status == Domain.Constants.UserStatus.Official ? userMeta.OfficalDate : null;
            info.AdvanceLeaveGranted = userMeta.AdvanceLeaveGranted ?? null;
            info.CompLeaveGranted = userMeta.CompLeaveGranted ?? null;
            info.ContractExpirationDate = userMeta.Month != Domain.Constants.TypeMonth.IndefiniteTerm ? userMeta.ContractExpirationDate : null;
            info.PersonnelStatus = userMeta.PersonnelStatus;
            info.ExpireDays = userMeta.ExpireDays;

            if (userMeta.PersonnelStatus == PersonnelStatus.Resigned)
            {
                info.IsActive = false;
            }
            else
            {
                info.IsActive = true;
            }


            var organization = await ResolveOrganizationAsync(info, userMeta);
            if (organization != null)
                return organization;

            if (!string.IsNullOrEmpty(userMeta.ManagerUserId))
            {
                if (userMeta.ManagerUserId == info.Id)
                    return new ActionResultResponse<string>(-8, _ghmHRResource.GetString(ErrorMessage.Notaccepted, _ghmHRResource.GetString("ManagerUser")));

                var managerUser = await _userRepository.GetInfoAsync(userMeta.ManagerUserId);
                if (managerUser == null || managerUser.IsActive == false)
                    return new ActionResultResponse<string>(-2, _ghmHRResource.GetString(ErrorMessage.NotExists, _ghmHRResource.GetString("ManagerUser")));
                info.ManagerUserId = managerUser.Id;
                info.ManagerFullName = managerUser.FullName;

            }
            else
            {
                info.ManagerUserId = null;
                info.ManagerFullName = null;
            }

            var result = await _userRepository.UpdateAsync(info);

            if (result <= 0)
                return new ActionResultResponse<string>(result, _ghmHRResource.GetString(ErrorMessage.SomethingWentWrong));
            return new ActionResultResponse<string>(result, _ghmHRResource.GetString("Successful"));
        }

        public async Task<ActionResultResponse> DeleteAsync(string tenantId, string deleteUserId, string deleteFullName, string deleteAvatar, string id)
        {

            var info = await _userRepository.GetInfoAsync(id);
            if (info == null)
                return new ActionResultResponse(-2, _ghmHRResource.GetString(ErrorMessage.NotExists, _ghmHRResource.GetString("User")));

            if (info.TenantId != tenantId || info.IsDelete)
                return new ActionResultResponse(-3, _ghmHRResource.GetString(ErrorMessage.NotHavePermission));

            info.DeleteUserId = deleteUserId;
            info.DeleteFullName = deleteFullName;

            var result = await _userRepository.DeleteAsync(info);

            if (result <= 0)
                return new ActionResultResponse(result, _ghmHRResource.GetString(ErrorMessage.SomethingWentWrong));

            await _userRepository.UpdateManagerUserAysnc(id);
            await _userRepository.DayOffs_DeleteByUserAsync(tenantId, info.CompanyId, id);
  
            return new ActionResultResponse(result, _ghmHRResource.GetString(SuccessMessage.DeleteSuccessful, _ghmHRResource.GetString("User")));
        }


        public async Task<ActionResultResponse<UserDetailViewModel>> GetDetailAsync(string tenantId, string id)
        {
            var info = await _userRepository.GetInfoAsync(id);
            if (info == null)
                return new ActionResultResponse<UserDetailViewModel>(-2, _ghmHRResource.GetString(ErrorMessage.NotExists, _ghmHRResource.GetString("User")));

            if (info.TenantId != tenantId)
                return new ActionResultResponse<UserDetailViewModel>(-3, _ghmHRResource.GetString(ErrorMessage.NotHavePermission));

            var userDetail = new UserDetailViewModel
            {
                Id = info.Id,
                CompanyId = info.CompanyId,
                Code = info.Code,
                FullName = info.FullName,
                Birthday = info.Birthday,
                Avatar = info.Avatar,
                Gender = info.Gender,
                CountryId = info.CountryId,
                CountryName = info.CountryName,
                ProvinceId = info.ProvinceId,
                ProvinceName = info.ProvinceName,
                DistrictId = info.DistrictId,
                DistrictName = info.DistrictName,
                NationId = info.NationId,
                NationName = info.NationName,
                ReligionId = info.ReligionId,
                ReligionName = info.ReligionName,
                Status = info.Status,
                Month = info.Month,
                OfficalDate = info.OfficalDate,
                JoinedDate = info.JoinedDate,
                OutDate = info.OutDate,
                DepartmentId = info.DepartmentId ?? null,
                DepartmentName = info.DepartmentName ?? null,
                DepartmentPath = info.DepartmentPath,
                TitleId = info.TitleId,
                TitleName = info.TitleName,
                PositionId = info.PositionId,
                PositionName = info.PositionName,
                UserName = info.UserName,
                ManagerUserId = info.ManagerUserId,
                ManagerFullName = info.ManagerFullName,
                EnrollNumberTT = info.EnrollNumberTT,
                EnrollNumberNeo = info.EnrollNumberNeo,
                ContractCode = info.ContractCode,
                WorkingForm = info.WorkingForm,
                InsuranceCode = info.InsuranceCode,
                InsuranceStatus = info.InsuranceStatus,
                InsuranceName = info.InsuranceName,
                SuccessorUserId = info.SuccessorUserId,
                SuccessorFullName = info.SuccessorFullName,
                Email = info.Email,
                PhoneNumber = info.PhoneNumber,
                IsActive = info.IsActive,
                ConcurrencyStamp = info.ConcurrencyStamp,
                Note = info.Note,
                AdvanceLeaveGranted = info.AdvanceLeaveGranted,
                CompLeaveGranted = info.CompLeaveGranted,
                ContractExpirationDate = info.ContractExpirationDate,
                PersonnelStatus = info.PersonnelStatus,
                ExpireDays = info.ExpireDays
            };

            return new ActionResultResponse<UserDetailViewModel>
            {
                Code = 1,
                Data = userDetail
            };
        }

        public async Task<UserProfileViewModel> GetProfileAsync(string tenantId, string id)
        {
            var info = await _userRepository.GetInfoAsync(id);
            if (info == null)
                return null;

            if (info.TenantId != tenantId)
                return null;

            var user = new UserProfileViewModel
            {
                Id = info.Id,
                CompanyId = info.CompanyId,
                Code = info.Code,
                FullName = info.FullName,
                Birthday = info.Birthday,
                Avatar = info.Avatar,
                Gender = info.Gender,
                DepartmentName = info.DepartmentName,
                TitleName = info.TitleName,
                PositionName = info.PositionName,
                UserName = info.UserName,
                ManagerFullName = info.ManagerFullName,
                Email = info.Email,
                PhoneNumber = info.PhoneNumber,
                Address = info.Address,
                WorkingForm = info.WorkingForm,
                Status = info.Status,
                Month = info.Month,
                OfficalDate = info.OfficalDate
            };
            return user;

        }

        public async Task<ActionResultResponse<string>> GetCodeAsync(string tenantId)
        {
            return new ActionResultResponse<string>
            {
                Code = 1,
                Data = await _userRepository.GetCodeAsync(tenantId)
            };
        }

        public async Task<List<UserSearchViewModel>> SelectAllAsync(string tenantId, string companyId, DateTime? contractExpirationDate)
        {
            return await _userRepository.SelectAllAsync(tenantId, companyId, contractExpirationDate);
        }

        public async Task<ActionResultResponse<string>> GetUserNameAsync(string tenantId, string fullName)
        {
            var userId = Guid.NewGuid().ToString();

            string userok = string.Empty;
            var userName = fullName.ToLower().Trim().StripVietnameseChars().GenerationUserName();
            var userName1 = fullName.ToLower().Trim().StripVietnameseChars().GenerationUserName();

            for (var i = 0; i <= 1000; i++)
            {

                var isUserNameExit = await _userRepository.CheckExistsByUserNameAsync(tenantId, userId, userName?.ToLower().StripVietnameseChars().Trim());
                if (isUserNameExit)
                {
                    userName = userName1 + Convert.ToString(i + 1);
                    continue;
                }
                else
                {
                    userok = userName;
                    break;
                }

            }
            return new ActionResultResponse<string>
            {
                Code = 1,
                Data = userok
            };
        }

        public async Task<List<UserSearchViewModel>> SelectAllUsersByCompanyDepartmentAsync(string tenantId, string companyId, int departmentId)
        {
            return await _userRepository.SelectAllUsersByCompanyDepartmentAsync(tenantId, companyId, departmentId);
        }

        public async Task<List<UserSearchAllViewModel>> SelectAllUsersAsync(string tenantId, string companyId)
        {
            return await _userRepository.SelectAllUsersAsync(tenantId, companyId);
        }

        public async Task<List<UserSearchViewModel>> SelectAllUsersByFineFormulaAsync(string tenantId, string companyId, string fineFormulaId)
        {
            return await _userRepository.SelectAllUsersByFineFormulaAsync(tenantId, companyId, fineFormulaId);
        }

      
        public async Task<ActionResultResponse<UserCountByRelationshipViewModel>> CountByRelationshipAsync(string companyId, BriefUser currentUser, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _userRepository.CountByRelationshipAsync(companyId, currentUser, cancellationToken);
                return new ActionResultResponse<UserCountByRelationshipViewModel>
                {
                    Code = 1,
                    Data = new UserCountByRelationshipViewModel
                    {
                        Official = result.FirstOrDefault(x => x?.Item1 == (int)UserStatus.Official)?.Item2 ?? 0,
                        Probation = result.FirstOrDefault(x => x?.Item1 == (int)UserStatus.Probation)?.Item2 ?? 0,
                        Intern = result.FirstOrDefault(x => x?.Item1 == (int)UserStatus.Intern)?.Item2 ?? 0,
                        Freelancer = result.FirstOrDefault(x => x?.Item1 == (int)UserStatus.Freelancer)?.Item2 ?? 0,
                    }
                };
            }
            catch
            {
                return new ActionResultResponse<UserCountByRelationshipViewModel>(-1, _ghmHRResource.GetString(ErrorMessage.SomethingWentWrong));
            }
        }

    }
}
