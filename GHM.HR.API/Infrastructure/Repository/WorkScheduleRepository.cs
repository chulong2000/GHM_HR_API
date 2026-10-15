using Dapper;
using GHM.HR.API.Domain.IRepository;
using GHM.HR.API.Domain.Models;
using GHM.HR.API.Domain.ViewModels;
using GHM.HR.Domain.Models;
using GHM.HR.Infrastructure.Repository;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Caching.Memory;
using System.Data;

namespace GHM.HR.API.Infrastructure.Repository
{
    public class WorkScheduleRepository : IWorkScheduleRepository
    {
        private readonly string _connectionString;
        private readonly ILogger<WorkScheduleRepository> _logger;
        private readonly IMemoryCache _cache;

        public WorkScheduleRepository(string connectionString, ILogger<WorkScheduleRepository> logger, IMemoryCache cache)
        {
            _connectionString = connectionString;
            _logger = logger;
            _cache = cache;
        }

        public async Task<int> DeleteAsync(string tenantId, string companyId, string id)
        {

            try
            {
                int rowAffected = 0;
                using (SqlConnection con = new(_connectionString))
                {
                    if (con.State == ConnectionState.Closed)
                        await con.OpenAsync();

                    DynamicParameters param = new();
                    param.Add("@Id", id);
                    param.Add("@CompanyId", companyId);
                    param.Add("@TenantId", tenantId);
                    rowAffected = await con.ExecuteAsync("[dbo].[spWorkSchedule_DeleteByID]", param, commandType: CommandType.StoredProcedure);
                }
                return rowAffected;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[dbo].[spWorkSchedule_DeleteByID] DeleteAsync WorkScheduleRepository Error.");
                return -1;
            }
        }

        public async Task<List<WorkScheduleViewModel>> GetAllScheduleInMonthOfUser(string tenantId, string companyId, string userId, int year, int month)
        {
            try
            {
                using SqlConnection con = new(_connectionString);
                if (con.State == ConnectionState.Closed)
                    await con.OpenAsync();

                DynamicParameters param = new();
                param.Add("@TenantId", tenantId);
                param.Add("@CompanyId", companyId);
                param.Add("@UserId", userId);
                param.Add("@Year", year);
                param.Add("@Month", month);
                var results = await con.QueryAsync<WorkScheduleViewModel>("[dbo].[spWorkSchedule_SelectWorkDayByMonth]", param, commandType: CommandType.StoredProcedure);
                return results.ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[dbo].[spWorkSchedule_GetAllInMonthOfUser] GetAllScheduleInMonthOfUser WorkScheduleRepository Error.");
                return new List<WorkScheduleViewModel>();
            }
        }

        public async Task<int> InsertAsync(WorkSchedule workSchedule)
        {
            try
            {
                int rowAffected = 0;
                using (SqlConnection con = new(_connectionString))
                {
                    if (con.State == ConnectionState.Closed)
                        await con.OpenAsync();

                    DynamicParameters param = new();
                    param.Add("@Id", workSchedule.Id);
                    param.Add("@WorkDay", workSchedule.WorkDay);
                    param.Add("@ShiftId", workSchedule.ShiftId);
                    param.Add("@UserId", workSchedule.UserId);
                    param.Add("@CreateTime", workSchedule.CreateTime);
                    param.Add("@CreateId", workSchedule.CreateId);
                    param.Add("@CreateFullName", workSchedule.CreateFullName);
                    param.Add("@CompanyId", workSchedule.CompanyId);
                    param.Add("@UserId", workSchedule.UserId);
                    param.Add("@TenantId", workSchedule.TenantId);
                    param.Add("@Year", workSchedule.Year);
                    
                    rowAffected = await con.ExecuteAsync("[dbo].[spWorkSchedule_Insert]", param, commandType: CommandType.StoredProcedure);
                }
                return rowAffected;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[dbo].[spWorkSchedule_Insert] InsertAsync WorkSchedule Error.");
                return -1;
            }
        }

        public async Task<int> UpdateWorkSchedulewhenHolidayCreated(string tenantId, string companyId, DateTime FromDate, DateTime ToDate, string holidayId)
        {
            try
            {
                int rowAffected = 0;
                using (SqlConnection con = new(_connectionString))
                {
                    if (con.State == ConnectionState.Closed)
                        await con.OpenAsync();

                    DynamicParameters param = new();
                    param.Add("@TenantId", tenantId);
                    param.Add("@CompanyId", companyId);
                    param.Add("@FromDay", FromDate);
                    param.Add("@ToDay", ToDate);
                    param.Add("@HolidayId", holidayId);
                    rowAffected = await con.ExecuteAsync("[dbo].[spWorkSchedule_UpdateWhenHolidayCreated]", param, commandType: CommandType.StoredProcedure);
                }
                return rowAffected;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[dbo].[spWorkSchedule_UpdateWhenHolidayCreated] UpdateWorkSchedulewhenHolidayCreated WorkScheduleRepository Error.");
                return -1;
            }
        }

        public async Task<int> UpdateWorkSchedulewhenHolidayUpdatedAsync(string tenantId, string companyId, DateTime FromDate, DateTime ToDate, string holidayId)
        {
            try
            {
                int rowAffected = 0;
                using (SqlConnection con = new(_connectionString))
                {
                    if (con.State == ConnectionState.Closed)
                        await con.OpenAsync();

                    DynamicParameters param = new();
                    param.Add("@TenantId", tenantId);
                    param.Add("@CompanyId", companyId);
                    param.Add("@FromDay", FromDate);
                    param.Add("@ToDay", ToDate);
                    param.Add("@HolidayId", holidayId);
                    rowAffected = await con.ExecuteAsync("[dbo].[spWorkSchedule_UpdatewhenHolidayUpdated]", param, commandType: CommandType.StoredProcedure);
                }
                return rowAffected;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[dbo].[spWorkSchedule_UpdatewhenHolidayUpdated] UpdateWorkSchedulewhenHolidayUpdatedAsync WorkScheduleRepository Error.");
                return -1;
            }
        }
    }
}
