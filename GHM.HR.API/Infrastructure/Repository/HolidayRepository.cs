using Dapper;
using GHM.HR.API.Domain.IRepository;
using GHM.HR.API.Domain.Models;
using GHM.HR.API.Infrastructure.Data;
using GHM.HR.Infrastructure.Repository;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Caching.Memory;
using System.Data;

namespace GHM.HR.API.Infrastructure.Repository
{
    public class HolidayRepository : IHolidayRepository
    {
        private readonly string _connectionString;
        private readonly ILogger<HolidayRepository> _logger;
        private readonly IDbSession _dbSession;

        public HolidayRepository(string connectionString, IDbSession dbSession, ILogger<HolidayRepository> logger)
        {
            _connectionString = connectionString;
            _dbSession = dbSession;
            _logger = logger;
        }

        public async Task<bool> checkNameExists(string tenantId, string companyId, string name, string holidayId)
        {
            var con = await _dbSession.GetConnectionAsync();
            DynamicParameters param = new();
            param.Add("@TenantId", tenantId);
            param.Add("@CompanyId", companyId);
            param.Add("@HolidayId", holidayId);
            param.Add("@Name", name);

            var result = await con.QueryFirstOrDefaultAsync<bool>("[dbo].[spHoliday_CheckNameExists]", param, transaction: _dbSession.Transaction, commandType: CommandType.StoredProcedure);
            return result;
        }

        public async Task<int> DeleteAsync(Holiday holiday)
        {
            var con = await _dbSession.GetConnectionAsync();
            int rowAffected = 0;

            DynamicParameters param = new();
            param.Add("@Id", holiday.Id);
            param.Add("@DeleteUserId", holiday.DeleteUserId);
            param.Add("@DeleteFullName", holiday.DeleteFullName);
            param.Add("@DeleteTime", holiday.DeleteTime);
            rowAffected = await con.ExecuteAsync("[dbo].[spHoliday_DeleteByID]", param, transaction: _dbSession.Transaction, commandType: CommandType.StoredProcedure);
            return rowAffected;
        }

        public async Task<Holiday> GetInfoAsync(string id)
        {
            var con = await _dbSession.GetConnectionAsync();

            DynamicParameters param = new();
            param.Add("@Id", id);
            return await con.QuerySingleOrDefaultAsync<Holiday>("[dbo].[spHoliday_SelectByID]", param, transaction: _dbSession.Transaction, commandType: CommandType.StoredProcedure);
        }

        public async Task<int> InsertAsync(Holiday holiday)
        {
            var con = await _dbSession.GetConnectionAsync();

            int rowAffected = 0;
            DynamicParameters param = new();
            param.Add("@Id", holiday.Id);
            param.Add("@TenantId", holiday.TenantId);
            param.Add("@CompanyId", holiday.CompanyId);
            param.Add("@FromDay", holiday.FromDay);
            param.Add("@ToDay", holiday.ToDay);
            param.Add("@Name", holiday.Name);
            param.Add("@Description", holiday.Description);
            param.Add("@CreateTime", holiday.CreateTime);
            param.Add("@CreateId", holiday.CreateId);
            param.Add("@CreateFullName", holiday.CreateFullName);
            param.Add("@LastUpdateTime", holiday.LastUpdateTime);
            param.Add("@LastUpdateId", holiday.LastUpdateId);
            param.Add("@LastUpdateFullName", holiday.LastUpdateFullName);
            param.Add("@IsDelete", holiday.IsDelete);

            rowAffected = await con.ExecuteAsync("[dbo].[spHoliday_Insert]", param, transaction: _dbSession.Transaction, commandType: CommandType.StoredProcedure);
            return rowAffected;
        }

        public async Task<List<HolidayViewModel>> SelectAllAsync(string tenantId, string companyId)
        {
            var con = await _dbSession.GetConnectionAsync();
            DynamicParameters param = new();
            param.Add("@TenantId", tenantId);
            param.Add("@CompanyId", companyId);
            var results = await con.QueryAsync<HolidayViewModel>("[dbo].[spHoliday_SelectAll]", param, transaction: _dbSession.Transaction, commandType: CommandType.StoredProcedure);
            return results.ToList();
        }

        public async Task<int> UpdateAsync(Holiday holiday)
        {
            int rowAffected = 0;
            var con = await _dbSession.GetConnectionAsync();

            DynamicParameters param = new();
            param.Add("@Id", holiday.Id);
            param.Add("@TenantId", holiday.TenantId);
            param.Add("@CompanyId", holiday.CompanyId);
            param.Add("@FromDay", holiday.FromDay);
            param.Add("@ToDay", holiday.ToDay);
            param.Add("@Name", holiday.Name);
            param.Add("@Description", holiday.Description);
            param.Add("@LastUpdateTime", holiday.LastUpdateTime);
            param.Add("@LastUpdateId", holiday.LastUpdateId);
            param.Add("@LastUpdateFullName", holiday.LastUpdateFullName);
            param.Add("@IsDelete", false);

            rowAffected = await con.ExecuteAsync("[dbo].[spHoliday_Update]", param,
                transaction: _dbSession.Transaction, commandType: CommandType.StoredProcedure);

            return rowAffected;
        }
    }
}
