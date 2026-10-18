using GHM.HR.API.Domain;
using GHM.HR.API.Domain.Models;
using GHM.HR.API.Domain.ViewModels;
using GHM.HR.Domain.Models;

namespace GHM.HR.API.Domain.IRepository
{
    public interface IWorkScheduleRepository
    {
        Task<int> InsertAsync(WorkSchedule workSchedule);

        Task<int> DeleteAsync(string tenantId, string companyId, string id);

        Task<List<WorkScheduleViewModel>> GetAllScheduleInMonthOfUser(string tenantId, string companyId, string userId, int year, int month);

        Task<int> UpdateWorkSchedulewhenHolidayCreateOrDelete(string tenantId, string companyId, DateTime FromDate, DateTime ToDate, string holidayId);
        Task<int> UpdateWorkSchedulewhenHolidayUpdatedAsync(string tenantId, string companyId, DateTime FromDate, DateTime ToDate, string holidayId);
        

    }
}
