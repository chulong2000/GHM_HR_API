using Dapper;
using GHM.HR.API.Domain.IRepository;
using GHM.HR.API.Domain.Models;
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
   
        public HolidayRepository(string connectionString, ILogger<HolidayRepository> logger, IMemoryCache cache)
        {
            _connectionString = connectionString;
            _logger = logger;
        }

        public async Task<bool> checkNameExists(string tenantId, string companyId, string name, string holidayId)
        {
            try
            {
                using SqlConnection con = new(_connectionString);
                if (con.State == ConnectionState.Closed)
                    await con.OpenAsync();

                DynamicParameters param = new();
                param.Add("@TenantId", tenantId);
                param.Add("@CompanyId", companyId);
                param.Add("@HolidayId", holidayId);
                param.Add("@Name", name);

                var result = await con.QueryFirstOrDefaultAsync<bool>("[dbo].[spHoliday_CheckNameExists]", param, commandType: CommandType.StoredProcedure);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[dbo].[spHoliday_CheckNameExists] checkNameExists HolidayRepository Error.");
                return false;
            }
        }

        public async Task<int> DeleteAsync(Holiday holiday)
        {
            try
            {
                int rowAffected = 0;
                using (SqlConnection con = new(_connectionString))
                {
                    if (con.State == ConnectionState.Closed)
                        await con.OpenAsync();

                    DynamicParameters param = new();
                    param.Add("@Id", holiday.Id);
                    rowAffected = await con.ExecuteAsync("[dbo].[spHoliday_DeleteByID]", param, commandType: CommandType.StoredProcedure);
                }
                return rowAffected;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[dbo].[spShift_DeleteByID] DeleteAsync ShiftRepository  Error.");
                return -1;
            }
        }

        public async Task<Holiday> GetInfoAsync(string id)
        {
            try
            {
                using SqlConnection con = new(_connectionString);
                if (con.State == ConnectionState.Closed)
                    await con.OpenAsync();

                DynamicParameters param = new();
                param.Add("@Id", id);
                return await con.QuerySingleOrDefaultAsync<Holiday>("[dbo].[spHoliday_SelectByID]", param, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[dbo].[spHoliday_SelectByID] GetInfoAsyncHolidayRepository Error.");
                return null;
            }
        }

        public async Task<int> InsertAsync(Holiday holiday)
        {
            try
            {
                int rowAffected = 0;
                using (SqlConnection con = new(_connectionString))
                {
                    if (con.State == ConnectionState.Closed)
                        await con.OpenAsync();

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

                    rowAffected = await con.ExecuteAsync("[dbo].[spHoliday_Insert]", param, commandType: CommandType.StoredProcedure);
                }
                return rowAffected;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[dbo].[spHoliday_Insert] InsertAsync HolidayRepository Error.");
                return -1;
            }
        }

        public async Task<List<HolidayViewModel>> SelectAllAsync(string tenantId, string companyId)
        {
            try
            {
                using SqlConnection con = new(_connectionString);
                if (con.State == ConnectionState.Closed)
                    await con.OpenAsync();

                DynamicParameters param = new();
                param.Add("@TenantId", tenantId);
                param.Add("@CompanyId", companyId);         
                var results = await con.QueryAsync<HolidayViewModel>("[dbo].[spHoliday_SelectAll]", param, commandType: CommandType.StoredProcedure);
                return results.ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[dbo].[spHoliday_SelectAll] SelectAllAsync HolidayRepository Error.");
                return [];
            }
        }

        public async Task<int> UpdateAsync(Holiday holiday)
        {
            try
            {
                int rowAffected = 0;
                using (SqlConnection con = new(_connectionString))
                {
                    if (con.State == ConnectionState.Closed)
                        await con.OpenAsync();

                    DynamicParameters param = new();
                    param.Add("@Id", holiday.Id);
                    param.Add("@FromDay", holiday.FromDay);
                    param.Add("@ToDay", holiday.ToDay);
                    param.Add("@Name", holiday.Name);
                    param.Add("@Description", holiday.Description);
                    param.Add("@LastUpdateTime", holiday.LastUpdateTime);
                    param.Add("@LastUpdateId", holiday.LastUpdateId);
                    param.Add("@LastUpdateFullName", holiday.LastUpdateFullName);

                    rowAffected = await con.ExecuteAsync("[dbo].[spHoliday_Update]", param, commandType: CommandType.StoredProcedure);
                }
                return rowAffected;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[dbo].[spHoliday_Update] UpdateAsync HolidayRepository Error.");
                return -1;
            }
        }
    }
}
