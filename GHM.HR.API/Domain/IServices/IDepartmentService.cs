using GHM.HR.API.Domain.ModelMetas;
using GHM.HR.API.Domain.ViewModels;
using GHM.Infrastructure.Models;

namespace GHM.HR.API.Domain.IServices
{
    public interface IDepartmentService
    {
        Task<List<DepartmentInWorkScheduleViewModel>> SelectAllDepartmentsActionAsync(string tenantId, string companyId);
        Task<ActionResultResponse<string>> InsertAsync(string tenantId, string creatorId, string creatorFullName, string creatorAvatar, DepartmentMeta departmentMeta);
        Task<ActionResultResponse<string>> UpdateAsync(string tenantId, string lastUpdateUserId, string lastUpdateFullName, string lastUpdateAvatar, int id, DepartmentMeta departmentMeta);
        Task<ActionResultResponse> DeleteAsync(string tenantId, string deleteUserId, string deleteFullName, string deleteAvatar, int id);
        Task<ActionResultResponse<DepartmentDetailViewModel>> GetDetailAsync(string tenantId, int id);
        Task<List<TreeData>> GetFullTreeAsync(string tenantId, string companyId);
        Task<List<TreeData>> GetFullTreeActiveAsync(string tenantId, string companyId);
        Task<List<TreeData>> GetFullTreeByUserAsync(string tenantId, string companyId, string userId);
    }
}
