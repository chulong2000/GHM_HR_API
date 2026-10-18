using Dapper;
using GHM.HR.API.Domain.IRepository;
using GHM.HR.API.Domain.Models;
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

        public ShiftRepository(string connectionString, ILogger<ShiftRepository> logger, IMemoryCache cache)
        {
            _connectionString = connectionString;
            _logger = logger;
            _cache = cache;
        }

        public async Task<bool> checkNameExists(string tenantId, string companyId, string name, string shiftId)
        {
                using SqlConnection con = new(_connectionString);
                if (con.State == ConnectionState.Closed)
                    await con.OpenAsync();

                var sql = @"
				SELECT IIF (EXISTS (
                            SELECT 1 FROM dbo.Shift WHERE CompanyId = @CompanyId AND TenantId = @TenantId AND Name = @Name AND Id <> @ShiftId), 1, 0)";

                var result = await con.ExecuteScalarAsync<bool>(sql, new { CompanyId = companyId, TenantId = tenantId, Name = name, ShiftId = shiftId });
                return result;
        }

        public async Task<int> DeleteAsync(Shift shift)
        {
                int rowAffected = 0;
                using (SqlConnection con = new(_connectionString))
                {
                    if (con.State == ConnectionState.Closed)
                        await con.OpenAsync();

                    DynamicParameters param = new();
                    param.Add("@Id", shift.Id);
                    rowAffected = await con.ExecuteAsync("[dbo].[spShift_DeleteByID]", param, commandType: CommandType.StoredProcedure);
                }
                return rowAffected;
        }

        public async Task<Shift?> GetInfoAsync(string id)
        {
                using SqlConnection con = new(_connectionString);
                if (con.State == ConnectionState.Closed)
                    await con.OpenAsync();

                DynamicParameters param = new();
                param.Add("@Id", id);
                return await con.QuerySingleOrDefaultAsync<Shift>("[dbo].[spShift_SelectByID]", param, commandType: CommandType.StoredProcedure);
        }

        public async Task<int> InsertAsync(Shift shift)
        {
                int rowAffected = 0;
                using (SqlConnection con = new(_connectionString))
                {
                    if (con.State == ConnectionState.Closed)
                        await con.OpenAsync();

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

                    rowAffected = await con.ExecuteAsync("[dbo].[spShift_Insert]", param, commandType: CommandType.StoredProcedure);
                }
                return rowAffected;
        }

        public async Task<List<ShiftsViewModel>> SelectAllAsync(string tenantId, string companyId)
        {
                using SqlConnection con = new(_connectionString);
                if (con.State == ConnectionState.Closed)
                    await con.OpenAsync();

                DynamicParameters param = new();
                param.Add("@TenantId", tenantId);
                param.Add("@CompanyId", companyId);
                var results = await con.QueryAsync<ShiftsViewModel>("[dbo].[spShift_SelectAll]", param, commandType: CommandType.StoredProcedure);
                return results.ToList();
        }

        public async Task<int> UpdateAsync(Shift shift)
        {
                int rowAffected = 0;
                using (SqlConnection con = new(_connectionString))
                {
                    if (con.State == ConnectionState.Closed)
                        await con.OpenAsync();

                    DynamicParameters param = new();
                    param.Add("@Id", shift.Id);
                    param.Add("@Name", shift.Name);
                    param.Add("@StartTime", shift.StartTime);
                    param.Add("@EndTime", shift.EndTime);
                    param.Add("@LastUpdatId", shift.LastUpdateId);
                    param.Add("@LastUpdateFullName", shift.LastUpdateFullName);

                    rowAffected = await con.ExecuteAsync("[dbo].[spShift_Update]", param, commandType: CommandType.StoredProcedure);
                }
                return rowAffected;
        }
    }
}
