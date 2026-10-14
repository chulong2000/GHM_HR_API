using GHM.Infrastructure.Models;

namespace GHM.HR.API.Domain.IServices
{
    public interface IWorkScheduleService
    {
        Task<ActionResultResponse<string>> InsertAsync(string tenantId, string companyId, string createId, string createFullName, ShiftMeta shiftMeta);
    }
}
