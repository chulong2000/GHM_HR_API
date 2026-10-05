using GHM.HR.API.Domain.ViewModels;
using GHM.HR.Domain.ViewModels;

namespace GHM.HR.API.Domain.IRepository
{
    public interface IWorkShiftUsersRepository
    {

        Task<List<ShiftOfUser>> CheckShiftOfUserAsync(string tenantId, string companyId, string userId, DateTime startDate, DateTime endDate);
        Task<WorkShiftRangeResponse> GetWorkShiftDetails(
          string tenantId, string companyId, string userId, DateTime date, TimeSpan startTime, TimeSpan endTime);
    }
}
