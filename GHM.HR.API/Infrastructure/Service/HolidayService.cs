using GHM.HR.API.Domain.IRepository;
using GHM.HR.API.Domain.IServices;
using GHM.HR.API.Domain.Models;
using GHM.HR.API.Domain.Resources;
using GHM.HR.API.Infrastructure.Data;
using GHM.HR.API.Infrastructure.Repository;
using GHM.HR.Domain.IRepository;
using GHM.HR.Infrastructure.Repository;
using GHM.Infrastructure.Constants;
using GHM.Infrastructure.IServices;
using GHM.Infrastructure.Models;
using IdentityModel.Client;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Net.WebSockets;

namespace GHM.HR.API.Infrastructure.Service
{
    public class HolidayService : IHolidayService
    {
        private readonly IHolidayRepository _holidayRepository;
        private readonly IWorkScheduleRepository _workScheduleRepository;

        private readonly IDbSession _dbSession;


        private readonly IResourceService<GhmHRResource> _ghmHRResource;

        public HolidayService(IHolidayRepository holidayRepository,
                              IWorkScheduleRepository workScheduleRepository,
                              IDbSession dbSession,
                              IResourceService<GhmHRResource> ghmHRResource)
        {
            _holidayRepository = holidayRepository;
            _workScheduleRepository = workScheduleRepository;
            _dbSession = dbSession;
            _ghmHRResource = ghmHRResource;
        }

        public async  Task<ActionResultResponse> DeleteAsync(string Id, string deleteUserId, string deleteFullName)
        {
            var info = await _holidayRepository.GetInfoAsync(Id);
            if (info == null)
                return new ActionResultResponse(-2, _ghmHRResource.GetString(ErrorMessage.NotExists, _ghmHRResource.GetString("Holiday")));
           
            info.DeleteUserId = Guid.Parse(deleteUserId);
            info.DeleteFullName = deleteFullName;
            info.DeleteTime = DateTime.Now;

            try
            {
               await _dbSession.BeginTransactionAsync();

                var result = await _holidayRepository.DeleteAsync(info);

                if (result <= 0)
                    return new ActionResultResponse(result, _ghmHRResource.GetString(ErrorMessage.SomethingWentWrong));

                result = await _workScheduleRepository.UpdateWorkSchedulewhenHolidayCreateOrDelete(info.TenantId.ToString(), info.CompanyId.ToString(), info.FromDay, info.ToDay, null);
                if (result < 0)
                    return new ActionResultResponse(result, _ghmHRResource.GetString(ErrorMessage.SomethingWentWrong));

                await _dbSession.CommitAsync();
                return new ActionResultResponse(result, _ghmHRResource.GetString(SuccessMessage.DeleteSuccessful, _ghmHRResource.GetString("Holiday")));
            }
            catch (Exception ex)
            {
                await _dbSession.RollbackAsync();
                return new ActionResultResponse(-1, _ghmHRResource.GetString(ex.Message));
            }
        }

        public async Task<ActionResultResponse<string>> InsertAsync(string tenantId, string companyId, string createId, string createFullName, HolidayMeta holidayMeta)
        {
            var holidayId = Guid.NewGuid().ToString();

            // kiểm tra tên của ca đã tồn tại hay chưa.
            var existingHoliday = await _holidayRepository.checkNameExists(tenantId, companyId, holidayMeta.Name.Trim(), holidayId);
            if (existingHoliday)
                return new ActionResultResponse<string>(-5, _ghmHRResource.GetString(ErrorMessage.AlreadyExists, _ghmHRResource.GetString("Holiday"), holidayMeta.Name));

            var holiday = new Holiday   
            {
                Id = Guid.Parse(holidayId),
                TenantId = Guid.Parse(tenantId),
                CompanyId = Guid.Parse(companyId),
                FromDay = holidayMeta.FromDay,
                ToDay = holidayMeta.ToDay,
                Name = holidayMeta.Name.Trim(),
                Description = holidayMeta.Description,
                CreateTime = DateTime.Now,
                CreateId = Guid.Parse(createId),
                CreateFullName = createFullName,
                IsDelete = false
            };

            try
            {
                await _dbSession.BeginTransactionAsync();

                await _holidayRepository.InsertAsync(holiday);

                await _workScheduleRepository.UpdateWorkSchedulewhenHolidayCreateOrDelete(tenantId, companyId, holiday.FromDay, holiday.ToDay, holidayId);

                await _dbSession.CommitAsync();

                return new ActionResultResponse<string>(1, _ghmHRResource.GetString(SuccessMessage.AddSuccessful, _ghmHRResource.GetString("Holiday")), string.Empty, holidayId);

            } catch(Exception ex)
            {
                await _dbSession.RollbackAsync();
                return new ActionResultResponse<string>(-1, _ghmHRResource.GetString(ex.Message));
            }

            
        }

        public async Task<List<HolidayViewModel>> SelectAllByCompanyIdAsync(string tenantId, string companyId)
        {
            return await _holidayRepository.SelectAllAsync(tenantId, companyId);
        }

        public async Task<ActionResultResponse<string>> UpdateAsync(string updateId, string updateFullName, HolidayMeta holidayMeta, string holidayId, string companyId, string tenantId)
        {
            var existingHolidayInfo = await _holidayRepository.GetInfoAsync(holidayId);
            if (existingHolidayInfo == null)
            {
                return new ActionResultResponse<string>(-5, _ghmHRResource.GetString(ErrorMessage.NotExists, _ghmHRResource.GetString("Holiday"), holidayMeta.Name));
            }

            var existingHolidayName = await _holidayRepository.checkNameExists(tenantId, companyId, holidayMeta.Name.Trim(), holidayId);
            if (existingHolidayName)
                return new ActionResultResponse<string>(-5, _ghmHRResource.GetString(ErrorMessage.AlreadyExists, _ghmHRResource.GetString("Holiday"), holidayMeta.Name));

            var holiday = new Holiday
            {
                Id = Guid.Parse(holidayId),
                TenantId = Guid.Parse(tenantId),
                CompanyId = Guid.Parse(companyId),
                FromDay = holidayMeta.FromDay,
                ToDay = holidayMeta.ToDay,
                Name = holidayMeta.Name.Trim(),
                Description = holidayMeta.Description,
                LastUpdateTime = DateTime.Now,
                LastUpdateId = Guid.Parse(updateId),
                LastUpdateFullName = updateFullName,
                IsDelete = false
            };

            try
            {
               await _dbSession.BeginTransactionAsync();

               await _holidayRepository.UpdateAsync(holiday);
                
               var dateRangeChanged = (existingHolidayInfo.FromDay.Date, existingHolidayInfo.ToDay.Date)
                                       != (holidayMeta.FromDay.Date, holidayMeta.ToDay.Date);

                if (dateRangeChanged)
                {
                    await _workScheduleRepository.UpdateWorkSchedulewhenHolidayUpdatedAsync(tenantId, companyId, holidayMeta.FromDay, holidayMeta.ToDay, holidayId);
                }

                await _dbSession.CommitAsync();
                return new ActionResultResponse<string>(1, _ghmHRResource.GetString(SuccessMessage.UpdateSuccessful, _ghmHRResource.GetString("Holiday")), string.Empty, holidayId);
            } catch(Exception ex)
            {
                await _dbSession.RollbackAsync();
                return new ActionResultResponse<string>(-1, _ghmHRResource.GetString(ex.Message));
            }

           
        }
    }
}
