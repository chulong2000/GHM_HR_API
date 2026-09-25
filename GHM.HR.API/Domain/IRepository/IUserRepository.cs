using GHM.Infrastructure.ViewModels;
using GHM.HR.Domain.Models;
using GHM.HR.Domain.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;
using GHM.HR.Domain.Constants;
using System;
using System.Threading;
using GHM.Infrastructure.Models;

namespace GHM.HR.Domain.IRepository
{
    public interface IUserRepository
    {
        Task<List<UserSearchViewModel>> SelectAllAsync(string tenantId, string companyId, DateTime? contractExpirationDate);
        Task<List<UserSearchAllViewModel>> SelectAllUsersAsync(string tenantId, string companyId);
        Task<List<UserBirthdayViewModel>> SearchUserBirthdayAsync(string tenantId, string companyId,int? month);
        Task<List<UserResignedViewModel>> SearchUserResignedAsync(string tenantId, string companyId, bool isAll, int month);
        Task<List<UserSearchViewModel>> SelectAllUsersByCompanyDepartmentAsync(string tenantId,string companyId, int departmentId);
        Task<List<UserSearchViewModel>> SelectAllUsersByFineFormulaAsync(string tenantId,string companyId, string fineFormulaId);
        Task<List<string>> GetListQLNSAsync(string tenantId, string companyId);
        Task<int> InsertAsync(User user);
        Task<List<string>> InsertBulkAsync(List<User> users);
        Task<int> UpdateAsync(User user);
        Task<int> DeleteAsync(User user);
        Task<int> ForceDeleteAsync(string tenantId, string id);
        Task<User> GetInfoAsync(string id);
        Task<User> GetInfoAsync(string tenantId, string id);
        Task<User> GetInfoMultiAsync(string companyId, string userId);
        Task<User> GetInfoByUserHisAsync(string companyId, string referenceId);
        Task<bool> CheckExistsAsync(string id);
        Task<bool> CheckExistsByTenantIdAsync(string tenantId, string id);
        Task<bool> CheckExistsByTenantIdActiveAsync(string tenantId, string id);
        Task<bool> CheckExistsByCodeAsync(string tenantId, string id, string code);
        Task<bool> CheckExistsByEmailAsync(string tenantId, string id, string email);
        Task<bool> CheckExistsByReferenceIdAsync(string tenantId, string id, string referenceId);
        Task<bool> CheckExistsByPhoneNumberAsync(string tenantId, string id, string phoneNumber);
        Task<bool> CheckExistsByUserNameAsync(string tenantId, string id, string userName);
        Task<bool> CheckExistsByIdCardNumberAsync(string tenantId, string id, string idCardNumber);
        Task<bool> CheckExistsByPassportIdAsync(string tenantId, string id, string passportId);
        Task<int> GetTotalUserByPosition(string tenantId, string positionId);
        Task<bool> CheckExistsByEnrollNumberAsync(string tenantId, string id, int enrollNumber);
        Task<bool> CheckExistsByCardNumberAsync(string tenantId, string id, string cardNumber);
        Task<bool> CheckExistsPositonByUserAsync(string tenantId, string positionId);
        Task<bool> CheckExistsTitleByUserAsync(string tenantId, string titleId);
        Task<bool> CheckExistsDepartmentByUserAsync(string tenantId, int departmentId);
        Task<string> GetCodeAsync(string tenantId);
        Task<SearchResult<UserSearchViewModel>> SearchIsTechAsync(string tenantId, string branchId, string keyword, int? officeId, int page, int pageSize);
        Task<bool> CheckExistsByCountryIdAsync(string countryId);
        Task<bool> CheckExistsByProvinceIdAsync(string provinceId);
        Task<bool> CheckExistsByDistrictIdAsync(string districtId);
        Task<bool> CheckExistsByNationIdAsync(string nationId);
        Task<bool> CheckExistsByReligionIdAsync(string religionId);
        Task<bool> CheckExistsByJobIdAsync(string jobId);
        Task<int> UpdateByUserFullNameAsync(string userId, string userFullName);
        Task<int> UpdateByUserUsedAsync(string userId, string userFullName, string phoneNumber);
        Task<bool> CheckExistsIsLockAsync(string tenantId, string id);
        Task<int> UpdateLockAsync(string id, bool isLock);
        Task<int> UpdatePhoneNumberAsync(string userId, string phoneNumber);
        Task<int> UpdateUserPaperInfoAsync(UserPaperInfo user);
        Task<int> UpdateUserOtherInfoAsync(UserOtherInfo user);
        Task<int> UpdateManagerUserAysnc(string userId);
        Task<List<UserDetailAllViewModel>> GetUsersByCompanyIdAndRoleId(string tenantId, string companyId, string roleId);
        Task<List<UserSearchAllViewModel>> SelectAllCurrentAsync(string tenantId, string companyId);
        Task<List<TempTable>> GetAllUsers(string tenantId, string companyId, string userId, string managerUserId, string managerUserIdOld, string creatorId, string creatorFullName);
        Task<int> UpdateDepartmentIsActiveAsync(string tenantId,string companyId, int departmentId);
        Task<User> GetUserByDoctorCodeAsync(string companyId, string doctorCode);
        #region DayOffs
        Task<int> DayOffsUpdateUsedAsync(string tenantId, string userId, DateTime date, int minutes);
        Task<bool> DayOffsCheckUserExist(string tenantId,string companyId, string userId);
        Task<int> DayOffsUpdateOldExpiryDateAsync(string tenantId, string companyId, string id, DateTime? expiryDate, DateTime? oldExpiryDate, string lastUpdateUserId, string lastUpdateFullName);
        Task<int> DayOffsUpdateRemainingLeaveDaysAsync(string tenantId, string companyId, string userId, int unusedLeave, string lastUpdateUserId, string lastUpdateFullName);
        Task<int> DayOffsUpdateUsedLeaveDaysAsync(string tenantId, string companyId, string userId, decimal totalDate);
        Task<int> DayOffs_DeleteByUserAsync(string tenantId, string companyId, string userId);
        #endregion

        #region DoiCa validation
        Task<bool> CheckSameDepartmentAsync(string tenantId, string userId1, string userId2);
        #endregion

        Task<List<(int, int)?>> CountByRelationshipAsync(string companyId, BriefUser currentUser, CancellationToken cancellationToken);
    }
}