using GHM.HR.API.Domain.ModelMetas;
using GHM.Infrastructure.Models;

namespace GHM.HR.API.Domain.IServices
{
    public interface IWorkScheduleService
    {
        Task<ActionResultResponse<string>> InsertAsync(string tenantId, string companyId, string createId, string createFullName, WorkScheduleMeta workScheduleMeta, string userId);

        Task<ActionResultResponse> DeleteAsync(string tenantId, string companyId, string id);
        Task<List<WorkScheduleViewModel>> GetAllScheduleInMonthOfUser(string tenantId, string companyId, string userId, int year, int month);
        

        
    }
}
