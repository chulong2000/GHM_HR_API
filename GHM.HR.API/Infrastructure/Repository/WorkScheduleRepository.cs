using Dapper;
using GHM.HR.API.Domain.IRepository;
using GHM.HR.API.Domain.Models;
using GHM.HR.API.Domain.ViewModels;
using GHM.HR.API.Infrastructure.Data;
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
        private readonly IDbSession _dbSession;

        public WorkScheduleRepository(string connectionString, ILogger<WorkScheduleRepository> logger, IMemoryCache cache, IDbSession dbSession)
        {
            _connectionString = connectionString;
            _dbSession = dbSession;
            _logger = logger;
            _cache = cache;
        }

        public async Task<int> DeleteAsync(string tenantId, string companyId, string id)
        {
            var con = await _dbSession.GetConnectionAsync();
            int rowAffected = 0;
                DynamicParameters param = new();
                param.Add("@Id", id);
                param.Add("@CompanyId", companyId);
                param.Add("@TenantId", tenantId);
                rowAffected = await con.ExecuteAsync("[dbo].[spWorkSchedule_DeleteByID]", param, transaction: _dbSession.Transaction, commandType: CommandType.StoredProcedure);
            return rowAffected;
        }

        public async Task<List<WorkScheduleViewModel>> GetAllScheduleInMonthOfUser(string tenantId, string companyId, string userId, int year, int month)
        {
            var con = await _dbSession.GetConnectionAsync();
            DynamicParameters param = new();
            param.Add("@TenantId", tenantId);
            param.Add("@CompanyId", companyId);
            param.Add("@UserId", userId);
            param.Add("@Year", year);
            param.Add("@Month", month);
            var results = await con.QueryAsync<WorkScheduleViewModel>("[dbo].[spWorkSchedule_SelectWorkDayByMonth]", param, transaction: _dbSession.Transaction, commandType: CommandType.StoredProcedure);
            return results.ToList();
        }

        public async Task<int> InsertAsync(WorkSchedule workSchedule)
        {
            var con = await _dbSession.GetConnectionAsync();
            int rowAffected = 0;
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

                rowAffected = await con.ExecuteAsync("[dbo].[spWorkSchedule_Insert]", param, transaction: _dbSession.Transaction, commandType: CommandType.StoredProcedure);
            return rowAffected;
        }

        public async Task<int> UpdateWorkSchedulewhenHolidayCreateOrDelete(string tenantId, string companyId, DateTime FromDate, DateTime ToDate, string holidayId)
        {
            var con = await _dbSession.GetConnectionAsync();

            int rowAffected = 0;

            DynamicParameters param = new();
            param.Add("@TenantId", tenantId);
            param.Add("@CompanyId", companyId);
            param.Add("@FromDay", FromDate.Date);
            param.Add("@ToDay", ToDate.Date);
            param.Add("@HolidayId", holidayId);
            rowAffected = await con.ExecuteAsync("[dbo].[spWorkSchedule_UpdateWhenHolidayCreated]", param, transaction: _dbSession.Transaction, commandType: CommandType.StoredProcedure);

            return rowAffected;


        }

        public async Task<int> UpdateWorkSchedulewhenHolidayUpdatedAsync(string tenantId, string companyId, DateTime FromDate, DateTime ToDate, string holidayId)
        {
            var con = await _dbSession.GetConnectionAsync();
            int rowAffected = 0;

            DynamicParameters param = new();
            param.Add("@TenantId", tenantId);
            param.Add("@CompanyId", companyId);
            param.Add("@FromDay", FromDate.Date);
            param.Add("@ToDay", ToDate.Date);
            param.Add("@HolidayId", holidayId);
            rowAffected = await con.ExecuteAsync("[dbo].[spWorkSchedule_UpdatewhenHolidayUpdated]", param,
                transaction: _dbSession.Transaction, commandType: CommandType.StoredProcedure);

            return rowAffected;
        }
    }
}
