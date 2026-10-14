using GHM.HR.API.Domain.Models;

namespace GHM.HR.API.Domain.IRepository
{
    public interface IHolidayRepository
    {
        Task<List<HolidayViewModel>> SelectAllAsync(string tenantId, string companyId);
        Task<int> InsertAsync(Holiday holiday);
        Task<int> UpdateAsync(Holiday holiday);
        Task<int> DeleteAsync(Holiday holiday);
        Task<Holiday> GetInfoAsync(string id);
        Task<bool> checkNameExists(string tenantId, string companyId, string v);
    }
}
