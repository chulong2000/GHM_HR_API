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
        public DepartmentService(IDepartmentRepository departmentRepository,
                                 IResourceService<GhmHRResource> ghmHRResource)
        {
            _departmentRepository = departmentRepository;
            _ghmHRResource = ghmHRResource;
        }

        public async Task<List<DepartmentInWorkScheduleViewModel>> SelectAllDepartmentsActionAsync(string tenantId, string companyId)
        {
            var result = new List<DepartmentInWorkScheduleViewModel>();
            var list = await _departmentRepository.SelectAllDepartmentsActionAsync(tenantId, companyId);
            foreach (var item in list)
            {
                result.Add(new DepartmentInWorkScheduleViewModel
                {
                    DepartmentId = item.Id,
                    DepartmentName = item.Name
                });
            }
            return result.ToList();
        }

        public async Task<ActionResultResponse<string>> InsertAsync(string tenantId, string creatorId, string creatorFullName, string creatorAvatar, DepartmentMeta departmentMeta)
        {
            if (string.IsNullOrEmpty(tenantId))
                return new ActionResultResponse<string>(-2, _ghmHRResource.GetString(ErrorMessage.NotExists, _ghmHRResource.GetString("Tenant")));

            var isNameExit = await _departmentRepository.CheckExistNameAsync(tenantId, departmentMeta.CompanyId, departmentMeta.Name?.Trim());
            if (isNameExit)
                return new ActionResultResponse<string>(-5, _ghmHRResource.GetString(ErrorMessage.AlreadyExists, _ghmHRResource.GetString("Department-Name"), departmentMeta.Name));

            if (departmentMeta.ParentId.HasValue)
            {
                var isExistParentId = await _departmentRepository.CheckExistsByTenantIdAsync(tenantId, departmentMeta.CompanyId, departmentMeta.ParentId.Value);
                if (!isExistParentId)
                    return new ActionResultResponse<string>(-2, _ghmHRResource.GetString(ErrorMessage.NotExists, _ghmHRResource.GetString("ParentId")));
            }
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
                ConcurrencyStamp = Guid.NewGuid().ToString(),
                CompanyId = departmentMeta.CompanyId?.Trim(),
                ParentId = departmentMeta.ParentId,
                Name = departmentMeta.Name?.Trim(),
                Description = departmentMeta.Description?.Trim(),
                IsActive = departmentMeta.IsActive,
                TenantId = tenantId,
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

            await _departmentRepository.Update_IdPath_NamePath_DepartmentPathAsync(result);
            await _departmentRepository.Update_ChildCountAsync(result);

            if (departmentMeta.ParentId.HasValue)
            {
                await _departmentRepository.Update_ChildCountAsync(departmentMeta.ParentId.Value);
            }

            return new ActionResultResponse<string>(result, _ghmHRResource.GetString(SuccessMessage.AddSuccessful, _ghmHRResource.GetString("Department")), string.Empty, result.ToString());
        }

        public async Task<ActionResultResponse<string>> UpdateAsync(string tenantId, string lastUpdateUserId, string lastUpdateFullName, string lastUpdateAvatar, int id, DepartmentMeta departmentMeta)
        {
            if (string.IsNullOrEmpty(tenantId))
                return new ActionResultResponse<string>(-2, _ghmHRResource.GetString(ErrorMessage.NotExists, _ghmHRResource.GetString("Tenant")));

            var info = await _departmentRepository.GetInfoAsync(id);
            if (info == null)
                return new ActionResultResponse<string>(-2, _ghmHRResource.GetString(ErrorMessage.NotExists, _ghmHRResource.GetString("Department")));

            if (info.TenantId != tenantId)
                return new ActionResultResponse<string>(-3, _ghmHRResource.GetString(ErrorMessage.NotHavePermission));

            if (info.CompanyId != departmentMeta.CompanyId)
                return new ActionResultResponse<string>(-3, _ghmHRResource.GetString(ErrorMessage.NotHavePermission));

            if (info.ConcurrencyStamp != departmentMeta.ConcurrencyStamp)
                return new ActionResultResponse<string>(-4, _ghmHRResource.GetString(ErrorMessage.AlreadyUpdatedByAnother));

            var isNameExit = await _departmentRepository.CheckExistsByNameAsync(tenantId, departmentMeta.CompanyId, id, departmentMeta.Name?.Trim());
            if (isNameExit)
                return new ActionResultResponse<string>(-5, _ghmHRResource.GetString(ErrorMessage.AlreadyExists, _ghmHRResource.GetString("Department-Name"), departmentMeta.Name));

            if (departmentMeta.ParentId.HasValue)
            {
                var isExistParentId = await _departmentRepository.CheckExistsByTenantIdAsync(tenantId, departmentMeta.CompanyId, departmentMeta.ParentId.Value);
                if (!isExistParentId)
                    return new ActionResultResponse<string>(-2, _ghmHRResource.GetString(ErrorMessage.NotExists, _ghmHRResource.GetString("ParentId")));

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
            info.ConcurrencyStamp = Guid.NewGuid().ToString();
            info.LastUpdate = DateTime.Now;
            info.LastUpdateUserId = lastUpdateUserId;
            info.LastUpdateFullName = lastUpdateFullName;
            info.AdvanceLeaveGranted = departmentMeta.AdvanceLeaveGranted ?? null;
            info.CompLeaveGranted = departmentMeta.CompLeaveGranted ?? null;
            info.ExpireDays = departmentMeta.ExpireDays;

            var result = await _departmentRepository.UpdateAsync(info);
            //await _departmentRepository.Update_IsActiveAsync(id, departmentMeta.IsActive);
            await _departmentRepository.Update_IdPath_NamePath_DepartmentPathAsync(id);
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
                await _departmentRepository.Update_IdPath_NamePath_DepartmentPathAsync(ids);
                await _departmentRepository.Update_ChildCountAsync(ids);
            }

            if (result <= 0)
                return new ActionResultResponse<string>(result, _ghmHRResource.GetString(ErrorMessage.SomethingWentWrong));

            if (oldName != info.Name)
            {
                await _departmentRepository.UpdateByDepartmentNameAsync(tenantId, info.CompanyId, info.Id, info.Name);
            }
            return new ActionResultResponse<string>(result, _ghmHRResource.GetString(SuccessMessage.UpdateSuccessful, _ghmHRResource.GetString("Department")), string.Empty, info.ConcurrencyStamp);
        }

        public async Task<ActionResultResponse<DepartmentDetailViewModel>> GetDetailAsync(string tenantId, int id)
        {
            var info = await _departmentRepository.GetInfoAsync(id);
            if (info == null)
                return new ActionResultResponse<DepartmentDetailViewModel>(-2, _ghmHRResource.GetString(ErrorMessage.NotExists, _ghmHRResource.GetString("Department")));

            if (info.TenantId != tenantId)
                return new ActionResultResponse<DepartmentDetailViewModel>(-3, _ghmHRResource.GetString(ErrorMessage.NotHavePermission));

            //if (info.CompanyId != companyId)
            //    return new ActionResultResponse<DepartmentDetailViewModel>(-3, _ghmHRResource.GetString(ErrorMessage.NotHavePermission));

            var departmentDetail = new DepartmentDetailViewModel
            {
                Id = info.Id,
                CompanyId = info.CompanyId,
                ParentId = info.ParentId,
                Name = info.Name,
                Description = info.Description,
                IdPath = info.IdPath,
                NamePath = info.NamePath,
                ChildCount = info.ChildCount,
                ConcurrencyStamp = info.ConcurrencyStamp,
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



        public async Task<ActionResultResponse> DeleteAsync(string tenantId, string deleteUserId, string deleteFullName, string deleteAvatar, int id)
        {
            var info = await _departmentRepository.GetInfoAsync(id);
            if (info == null)
                return new ActionResultResponse(-2, _ghmHRResource.GetString(ErrorMessage.NotExists, _ghmHRResource.GetString("Department")));

            var ListUpdate = await _departmentRepository.GetIdChildrenAsync(id);

            info.DeleteUserId = deleteUserId;
            info.DeleteFullName = deleteFullName;
            var result = await _departmentRepository.DeleteAsync(info);

            await _departmentRepository.Update_IdPath_NamePath_DepartmentPathAsync(id);
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
                await _departmentRepository.Update_IdPath_NamePath_DepartmentPathAsync(ids);
                await _departmentRepository.Update_ChildCountAsync(ids);
            }

            if (result <= 0)
                return new ActionResultResponse(result, _ghmHRResource.GetString(ErrorMessage.SomethingWentWrong));

            return new ActionResultResponse(result, _ghmHRResource.GetString(SuccessMessage.DeleteSuccessful, _ghmHRResource.GetString("Department")));
        }

        public async Task<List<TreeData>> GetFullTreeAsync(string tenantId, string companyId)
        {
            var tree = new List<TreeData>();
            var departments = await _departmentRepository.SelectAllAsync(tenantId, companyId);
            if (departments == null || !departments.Any())
                return tree;

            tree = RenderDepartmentTree(departments, null);
            return tree;
        }

        public async Task<List<TreeData>> GetFullTreeActiveAsync(string tenantId, string companyId)
        {
            var tree = new List<TreeData>();
            var departments = await _departmentRepository.SelectAllDepartmentsActionAsync(tenantId, companyId);
            if (departments == null || !departments.Any())
                return tree;

            tree = RenderDepartmentTree(departments, null);
            return tree;
        }

        public async Task<List<TreeData>> GetFullTreeByUserAsync(string tenantId, string companyId, string userId)
        {
            var tree = new List<TreeData>();
            var departments = await _departmentRepository.SelectDepartmentByTenantIdAsync(tenantId, companyId, userId);
            if (departments == null || !departments.Any())
                return tree;

            tree = RenderDepartmentTree(departments, null);
            return tree;
        }

        private List<TreeData> RenderDepartmentTree(List<DepartmentSearchViewModel> departments, int? parentId)
        {
            var departmentTree = new List<TreeData>();
            var listDepartments = departments.Where(x => x.ParentId == parentId).ToList();
            if (!listDepartments.Any()) return departmentTree;

            departmentTree.AddRange(listDepartments.Select(g => new TreeData
            {
                Id = g.Id,
                Text = g.Name,
                ParentId = parentId,
                IdPath = g.IdPath,
                Data = g,
                Children = RenderDepartmentTree(departments, g.Id),
                State = new State(),
                ChildCount = g.ChildCount
            }));
            return departmentTree;
        }
    }
}
