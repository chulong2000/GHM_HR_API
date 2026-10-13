using GHM.HR.API.Domain;
using GHM.HR.API.Domain.Models;
using GHM.HR.API.Domain.ViewModels;
using GHM.HR.Domain.Models;

namespace GHM.HR.API.Domain.IRepository
{
    public interface IWorkScheduleRepository
    {
        Task<int> InsertAsync(WorkSchedule workSchedule);
    }
}
