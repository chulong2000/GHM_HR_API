using GHM.HR.API.Domain.Models;

namespace GHM.HR.API.Domain.IRepository
{
    public interface ITitleRepositorycs
    {
        Task<List<TitleSearchViewModel>> SelectAllAsync(string tenantId, string companyId);
        Task<List<TitleSearchViewModel>> SelectAllTitleActiveAsync(string tenantId, string companyId);
        Task<int> InsertAsync(Title title);
        Task<int> UpdateAsync(Title title);
        Task<int> DeleteAsync(Title title);
        Task<int> ForceDeleteAsync(string companyId, string id);
        Task<Title> GetInfoAsync(string id);
        Task<bool> CheckExistsNameAsync(string tenantId, string companyId, string name);
        Task<bool> CheckExistsCodeAsync(string tenantId, string companyId, string code, string id);
        Task<bool> CheckExistByNameAsync(string tenantId, string companyId, string id, string name);
        Task<int> UpdateByTitleNameAsync(string tenantId, string companyId, string titleId, string titleName);
        Task<bool> CheckExistsAsync(string companyId, string id);
        Task<int> UpdateIsActive(string companyId, string id, bool isActive);
        Task<string> GetCode(string id);
    }
}
