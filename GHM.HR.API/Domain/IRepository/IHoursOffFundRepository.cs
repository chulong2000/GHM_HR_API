using GHM.HR.API.Domain.ViewModels;
using System.Data;

namespace GHM.HR.API.Domain.IRepository
{
    public interface IHoursOffFundRepository
    {
        Task<UserHoursOffViewModel> GetHoursOffAsync(string tenantId, string userId, DateTime startDate);
        Task<List<HoursOffViewModel>> GetAllHoursOffAsync(string tenantId, string companyId, int year, int month, string keyword, string departmentIds);
        Task<List<HoursOffFundDetailItem>> SelectDetailHoursOffAsync(string tenantId, string companyId, string userId, int year, int month, int filterType);
        Task<int> ExtendAsync(string tenantId, string companyId, DataTable table, DateTime newExpiryDate,
            string creatorId, string creatorFullName, string creatorAvatar);

        Task<List<HoursOffFundSummaryItem>> GetSummaryAsync(string tenantId, string companyId, int year, int month, string keyword, string departmentIds);
        Task<List<HoursOffFundExpiringSoon>> GetExpiringSoonAsync(string tenantId, string companyId, string userId, DateTime startDate);
        Task<List<HoursOffFundExpiredDetailItem>> GetExpiredDetailAsync(string tenantId, string companyId, string userId, int year, int month);
    }
}
