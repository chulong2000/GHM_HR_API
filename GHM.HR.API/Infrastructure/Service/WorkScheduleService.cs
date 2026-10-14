using GHM.HR.API.Domain.IRepository;
using GHM.HR.API.Domain.IServices;
using GHM.HR.API.Domain.ModelMetas;
using GHM.HR.API.Domain.Models;
using GHM.HR.API.Domain.Resources;
using GHM.Infrastructure.Constants;
using GHM.Infrastructure.IServices;
using GHM.Infrastructure.Models;

namespace GHM.HR.API.Infrastructure.Service
{
    public class WorkScheduleService : IWorkScheduleService
    {
        private readonly IWorkScheduleRepository _workScheduleRepository;
        private readonly IResourceService<GhmHRResource> _ghmHRResource;
        public WorkScheduleService(IWorkScheduleRepository workScheduleRepository,
                                   IResourceService<GhmHRResource> ghmHRResource)
        {
            _workScheduleRepository = workScheduleRepository;
            _ghmHRResource = ghmHRResource;
        }

        public async Task<ActionResultResponse> DeleteAsync(string tenantId, string companyId, string id)
        {
            var result = await _workScheduleRepository.DeleteAsync(tenantId, companyId, id);
            if(result <= 0)
                return new ActionResultResponse(result, _ghmHRResource.GetString(ErrorMessage.SomethingWentWrong));

            return new ActionResultResponse<string>(result, _ghmHRResource.GetString(SuccessMessage.DeleteSuccessful, _ghmHRResource.GetString("WorkSchedule")), string.Empty);
        }

        public async Task<List<WorkScheduleViewModel>> GetAllScheduleInMonthOfUser(string tenantId, string companyId, string userId, int year, int month)
        {
            return await _workScheduleRepository.GetAllScheduleInMonthOfUser(tenantId, companyId, userId, year, month);
        }

        public async Task<ActionResultResponse<string>> InsertAsync(string tenantId, string companyId, string createId, string createFullName, WorkScheduleMeta workScheduleMeta, string userId)
        {


               var result = await _workScheduleRepository.InsertAsync(new WorkSchedule
               {
                   Id = Guid.NewGuid(),
                   TenantId = Guid.Parse(tenantId),
                   CompanyId = Guid.Parse(companyId),
                   ShiftId = Guid.Parse(workScheduleMeta.ShiftId),
                   CreateId = Guid.Parse(createId),
                   CreateTime = DateTime.Now,
                   UserId = Guid.Parse(userId),
                   CreateFullName = createFullName,
                   WorkDay = workScheduleMeta.WorkDay,
                   Year = workScheduleMeta.Year
               });

            if (result <= 0)
                return new ActionResultResponse<string>(result, _ghmHRResource.GetString(ErrorMessage.SomethingWentWrong));

            return new ActionResultResponse<string>(result, _ghmHRResource.GetString(SuccessMessage.AddSuccessful, _ghmHRResource.GetString("WorkSchedule")), string.Empty);
        }
    }
}
