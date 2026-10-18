using GHM.Infrastructure.Models;

namespace GHM.HR.API.Domain.IServices
{
    public interface IHolidayService
    {
        Task<List<HolidayViewModel>> SelectAllByCompanyIdAsync(string tenantId, string companyId);
        Task<ActionResultResponse<string>> InsertAsync(string tenantId, string companyId, string createId, string createFullName, HolidayMeta holidayMeta);
        Task<ActionResultResponse<string>> UpdateAsync(string updateId, string updateFullName, HolidayMeta holidayMeta, string holidayId, string companyId, string tenantId);
        Task<ActionResultResponse> DeleteAsync(string Id, string deleteUserId, string deleteFulName);
    }
}
