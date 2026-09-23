using GHM.HR.API.Domain.ModelMetas;
using GHM.HR.API.Domain.ViewModels;
using GHM.Infrastructure.Models;

namespace GHM.HR.API.Domain.IServices
{
    public interface IDepartmentService
    {
        Task<ActionResultResponse<string>> InsertAsync(string creatorId, string creatorFullName, string creatorAvatar, DepartmentMeta departmentMeta);
        Task<ActionResultResponse<string>> UpdateAsync(string lastUpdateUserId, string lastUpdateFullName, string lastUpdateAvatar, int id, DepartmentMeta departmentMeta);
        Task<ActionResultResponse> DeleteAsync(string deleteUserId, string deleteFullName, string deleteAvatar, int id);
        Task<ActionResultResponse<DepartmentDetailViewModel>> GetDetailAsync(int id);
    }
}
