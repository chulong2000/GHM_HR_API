using GHM.HR.API.Domain.Models;
using GHM.HR.API.Domain.ViewModels;
using GHM.Infrastructure.Models;

namespace GHM.HR.API.Domain.IServices
{
    public interface ITitleService
    {
        Task<List<TitleSearchViewModel>> SelectAllAsync(string tenantId, string companyId);
        Task<List<TitleSearchViewModel>> SelectAllTitleActiveAsync(string tenantId, string companyId);
        Task<ActionResultResponse<string>> InsertAsync(string tenantId, string creatorId, string creatorFullName, string creatorAvatar, TitleMeta titleMeta);
        Task<ActionResultResponse<string>> UpdateAsync(string tenantId, string lastUpdateUserId, string lastUpdateFullName, string lastUpdateAvatar, string id, TitleMeta titleMeta);
        Task<ActionResultResponse> DeleteAsync(string tenantId, string deleteUserId, string deleteFullName, string deleteAvatar, string id);
        Task<ActionResultResponse<TitleDetailViewModel>> GetDetailAsync(string tenantId, string id);
        Task<ActionResultResponse<string>> UpdateIsActive(string companyId, string id, bool isActive);
    }
}
