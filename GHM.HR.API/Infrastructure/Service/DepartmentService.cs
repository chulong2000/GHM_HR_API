using GHM.HR.API.Domain.IRepository;
using GHM.HR.API.Domain.IServices;
using GHM.HR.API.Domain.ModelMetas;
using GHM.HR.API.Domain.Models;
using GHM.HR.API.Domain.Resources;
using GHM.HR.API.Domain.ViewModels;
using GHM.Infrastructure.Constants;
using GHM.Infrastructure.IServices;
using GHM.Infrastructure.Models;

namespace GHM.HR.API.Infrastructure.Service
{
    public class DepartmentService : IDepartmentService
    {
        private readonly IDepartmentRepository _departmentRepository;

        private readonly IResourceService<GhmHRResource> _ghmHRResource;

        public DepartmentService(IDepartmentRepository departmentRepository)
        {
            _departmentRepository = departmentRepository;
        }

        public async Task<ActionResultResponse> DeleteAsync(string deleteUserId, string deleteFullName, string deleteAvatar, int id)
        {
            var info = await _departmentRepository.GetInfoAsync(id);
            if (info == null)
                return new ActionResultResponse(-2, _ghmHRResource.GetString(ErrorMessage.NotExists, _ghmHRResource.GetString("Department")));

            var ListUpdate = await _departmentRepository.GetIdChildrenAsync(id);

            info.DeleteUserId = deleteUserId;
            info.DeleteFullName = deleteFullName;
            var result = await _departmentRepository.DeleteAsync(info);

            await _departmentRepository.Update_ChildCountAsync(id);
            if (info.ParentId.HasValue)
            {
                await _departmentRepository.Update_ChildCountAsync(info.ParentId.Value);
            }

            foreach (int ids in ListUpdate)
            {
                var infos = await _departmentRepository.GetInfoAsync(ids);
                if (infos != null)
                {
                    infos.DeleteUserId = deleteUserId;
                    infos.DeleteFullName = deleteFullName;
                    await _departmentRepository.DeleteAsync(infos);
                }
            }

            foreach (int ids in ListUpdate)
            {
                await _departmentRepository.Update_ChildCountAsync(ids);
            }

            if (result <= 0)
                return new ActionResultResponse(result, _ghmHRResource.GetString(ErrorMessage.SomethingWentWrong));

            return new ActionResultResponse(result, _ghmHRResource.GetString(SuccessMessage.DeleteSuccessful, _ghmHRResource.GetString("Department")));
        }

        public async Task<ActionResultResponse<DepartmentDetailViewModel>> GetDetailAsync(int id)
        {
            var info = await _departmentRepository.GetInfoAsync(id);
            if (info == null)
                return new ActionResultResponse<DepartmentDetailViewModel>(-2, _ghmHRResource.GetString(ErrorMessage.NotExists, _ghmHRResource.GetString("Department")));

            var departmentDetail = new DepartmentDetailViewModel
            {
                Id = info.Id,
                CompanyId = info.CompanyId,
                ParentId = info.ParentId,
                Name = info.Name,
                Description = info.Description,
                ChildCount = info.ChildCount,
                IsActive = info.IsActive,
                AdvanceLeaveGranted = info.AdvanceLeaveGranted,
                CompLeaveGranted = info.CompLeaveGranted,
                ExpireDays = info.ExpireDays
            };

            return new ActionResultResponse<DepartmentDetailViewModel>
            {
                Code = 1,
                Data = departmentDetail
            };
        }

        public async Task<ActionResultResponse<string>> InsertAsync(string creatorId, string creatorFullName, string creatorAvatar, DepartmentMeta departmentMeta)
        {
           
            var isNameExit = await _departmentRepository.CheckExistNameAsync(departmentMeta.CompanyId, departmentMeta.Name?.Trim());
            if (isNameExit)
                return new ActionResultResponse<string>(-5, _ghmHRResource.GetString(ErrorMessage.AlreadyExists, _ghmHRResource.GetString("Department-Name"), departmentMeta.Name));

            if (departmentMeta.AdvanceLeaveGranted > 12)
            {
                return new ActionResultResponse<string>(-99, _ghmHRResource.GetString("AdvanceLeaveGranted must not exceed 12 days."));
            }

            if (departmentMeta.CompLeaveGranted > 480)
            {
                return new ActionResultResponse<string>(-99, _ghmHRResource.GetString("CompLeaveGranted must not exceed 12 days."));
            }
            var department = new Department
            {
                CompanyId = departmentMeta.CompanyId?.Trim(),
                ParentId = departmentMeta.ParentId,
                Name = departmentMeta.Name?.Trim(),
                Description = departmentMeta.Description?.Trim(),
                IsActive = departmentMeta.IsActive,
                CreateTime = DateTime.Now,
                CreatorId = creatorId,
                CreatorFullName = creatorFullName,
                AdvanceLeaveGranted = departmentMeta.AdvanceLeaveGranted ?? null,
                CompLeaveGranted = departmentMeta.CompLeaveGranted ?? null,
                ExpireDays = departmentMeta.ExpireDays
            };

            var result = await _departmentRepository.InsertAsync(department);

            if (result <= 0)
                return new ActionResultResponse<string>(result, _ghmHRResource.GetString(ErrorMessage.SomethingWentWrong));

            if (departmentMeta.ParentId.HasValue)
            {
                await _departmentRepository.Update_ChildCountAsync(departmentMeta.ParentId.Value);
            }

            return new ActionResultResponse<string>(result, _ghmHRResource.GetString(SuccessMessage.AddSuccessful, _ghmHRResource.GetString("Department")), string.Empty, result.ToString());
        }

        public async Task<ActionResultResponse<string>> UpdateAsync(string lastUpdateUserId, string lastUpdateFullName, string lastUpdateAvatar, int id, DepartmentMeta departmentMeta)
        {
            
            var info = await _departmentRepository.GetInfoAsync(id);
            if (info == null)
                return new ActionResultResponse<string>(-2, _ghmHRResource.GetString(ErrorMessage.NotExists, _ghmHRResource.GetString("Department")));

            var isNameExit = await _departmentRepository.CheckExistNameAsync(departmentMeta.CompanyId, departmentMeta.Name?.Trim());
            if (isNameExit)
                return new ActionResultResponse<string>(-5, _ghmHRResource.GetString(ErrorMessage.AlreadyExists, _ghmHRResource.GetString("Department-Name"), departmentMeta.Name));

            if (departmentMeta.ParentId.HasValue)
            {
                
                var isParentId = await _departmentRepository.CheckParentIdAsync(id, departmentMeta.ParentId.Value);
                if (isParentId)
                    return new ActionResultResponse<string>(-8, _ghmHRResource.GetString(ErrorMessage.Notaccepted, _ghmHRResource.GetString("ParentId")));
            }

            if (departmentMeta.AdvanceLeaveGranted > 12)
            {
                return new ActionResultResponse<string>(-99, _ghmHRResource.GetString("AdvanceLeaveGranted must not exceed 12 days."));
            }

            if (departmentMeta.CompLeaveGranted > 480)
            {
                return new ActionResultResponse<string>(-99, _ghmHRResource.GetString("CompLeaveGranted must not exceed 12 days."));
            }

            var oldName = info.Name;
            var oldParentId = info.ParentId;

            info.CompanyId = departmentMeta.CompanyId;
            info.ParentId = departmentMeta.ParentId;
            info.Name = departmentMeta.Name;
            info.IsActive = departmentMeta.IsActive;
            info.Description = departmentMeta.Description;
            info.LastUpdate = DateTime.Now;
            info.LastUpdateUserId = lastUpdateUserId;
            info.LastUpdateFullName = lastUpdateFullName;
            info.AdvanceLeaveGranted = departmentMeta.AdvanceLeaveGranted ?? null;
            info.CompLeaveGranted = departmentMeta.CompLeaveGranted ?? null;
            info.ExpireDays = departmentMeta.ExpireDays;

            var result = await _departmentRepository.UpdateAsync(info);
            await _departmentRepository.Update_ChildCountAsync(id);

            if (oldParentId != departmentMeta.ParentId)
            {
                if (departmentMeta.ParentId.HasValue)
                {
                    await _departmentRepository.Update_ChildCountAsync(departmentMeta.ParentId.Value);
                }

                if (oldParentId.HasValue)
                {
                    await _departmentRepository.Update_ChildCountAsync(oldParentId.Value);
                }
            }

            // Update lai cua cac node con
            var ListUpdate = await _departmentRepository.GetIdChildrenAsync(id);
            foreach (int ids in ListUpdate)
            {
                await _departmentRepository.Update_ChildCountAsync(ids);
            }

            if (result <= 0)
                return new ActionResultResponse<string>(result, _ghmHRResource.GetString(ErrorMessage.SomethingWentWrong));

            return new ActionResultResponse<string>(result, _ghmHRResource.GetString(SuccessMessage.UpdateSuccessful, _ghmHRResource.GetString("Department")), string.Empty);
        }
    }
}
