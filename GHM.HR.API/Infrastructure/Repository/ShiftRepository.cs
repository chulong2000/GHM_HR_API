using Dapper;
using GHM.HR.API.Domain.IRepository;
using GHM.HR.API.Domain.Models;
using GHM.HR.API.Infrastructure.Data;
using GHM.HR.Domain.Models;
using GHM.HR.Domain.ViewModels;
using GHM.HR.Infrastructure.Repository;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Caching.Memory;
using System.ComponentModel.Design;
using System.Data;
using System.Xml.Linq;

namespace GHM.HR.API.Infrastructure.Repository
{
    public class ShiftRepository : IShiftRepository
    {
        private readonly string _connectionString;
        private readonly ILogger<ShiftRepository> _logger;
        private readonly IMemoryCache _cache;
        private readonly IDbSession _dbSession;

        public ShiftRepository(string connectionString, ILogger<ShiftRepository> logger, IMemoryCache cache, IDbSession dbSession)
        {
            _connectionString = connectionString;
            _logger = logger;
            _cache = cache;
            _dbSession = dbSession;
        }

        public async Task<bool> checkNameExists(string tenantId, string companyId, string name, string shiftId)
        {
            var con = await _dbSession.GetConnectionAsync();
            var sql = @"
				SELECT IIF (EXISTS (
                            SELECT 1 FROM dbo.Shift WHERE CompanyId = @CompanyId AND TenantId = @TenantId AND Name = @Name AND Id <> @ShiftId), 1, 0)";

            var result = await con.ExecuteScalarAsync<bool>(sql, new { CompanyId = companyId, TenantId = tenantId, Name = name, ShiftId = shiftId });
            return result;
        }

        public async Task<int> DeleteAsync(Shift shift)
        {
            var con = await _dbSession.GetConnectionAsync();
            int rowAffected = 0;
                DynamicParameters param = new();
                param.Add("@Id", shift.Id);
                rowAffected = await con.ExecuteAsync("[dbo].[spShift_DeleteByID]", param, transaction: _dbSession.Transaction, commandType: CommandType.StoredProcedure);
            return rowAffected;
        }

        public async Task<Shift?> GetInfoAsync(string id)
        {
            var con = await _dbSession.GetConnectionAsync();
            DynamicParameters param = new();
            param.Add("@Id", id);
            return await con.QuerySingleOrDefaultAsync<Shift>("[dbo].[spShift_SelectByID]", param, transaction: _dbSession.Transaction, commandType: CommandType.StoredProcedure);
        }

        public async Task<int> InsertAsync(Shift shift)
        {
            var con = await _dbSession.GetConnectionAsync();
            int rowAffected = 0;
                DynamicParameters param = new();
                param.Add("@Id", shift.Id);
                param.Add("@CompanyId", shift.CompanyId);
                param.Add("@TenantId", shift.TenantId);
                param.Add("@Name", shift.Name);
                param.Add("@StartTime", shift.StartTime);
                param.Add("@EndTime", shift.EndTime);
                param.Add("@CreateTime", shift.CreateTime);
                param.Add("@CreateId", shift.CreateId);
                param.Add("@CreateFullName", shift.CreateFullName);
                param.Add("@LastUpdateTime", shift.LastUpdateTime);
                param.Add("@LastUpdatId", shift.LastUpdateId);
                param.Add("@LastUpdateFullName", shift.LastUpdateFullName);

                rowAffected = await con.ExecuteAsync("[dbo].[spShift_Insert]", param, transaction: _dbSession.Transaction, commandType: CommandType.StoredProcedure);
            return rowAffected;
        }

        public async Task<List<ShiftsViewModel>> SelectAllAsync(string tenantId, string companyId)
        {
            var con = await _dbSession.GetConnectionAsync();
            DynamicParameters param = new();
            param.Add("@TenantId", tenantId);
            param.Add("@CompanyId", companyId);
            var results = await con.QueryAsync<ShiftsViewModel>("[dbo].[spShift_SelectAll]", param, transaction: _dbSession.Transaction, commandType: CommandType.StoredProcedure);
            return results.ToList();
        }

        public async Task<int> UpdateAsync(Shift shift)
        {
            var con = await _dbSession.GetConnectionAsync();
            int rowAffected = 0;
                DynamicParameters param = new();
                param.Add("@Id", shift.Id);
                param.Add("@Name", shift.Name);
                param.Add("@StartTime", shift.StartTime);
                param.Add("@EndTime", shift.EndTime);
                param.Add("@LastUpdatId", shift.LastUpdateId);
                param.Add("@LastUpdateFullName", shift.LastUpdateFullName);

                rowAffected = await con.ExecuteAsync("[dbo].[spShift_Update]", param, transaction: _dbSession.Transaction, commandType: CommandType.StoredProcedure);
            return rowAffected;
        }
    }
}
