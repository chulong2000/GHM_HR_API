using Dapper;
using GHM.HR.API.Domain.IRepository;
using GHM.HR.API.Domain.ViewModels;
using Microsoft.Data.SqlClient;
using System.Data;

namespace GHM.HR.API.Infrastructure.Repository
{
    public class HoursOffFundRepository : IHoursOffFundRepository
    {
        private readonly string _connectionString;
        private readonly ILogger<HoursOffFundRepository> _logger;

        public HoursOffFundRepository(string connectionString, ILogger<HoursOffFundRepository> logger)
        {
            _connectionString = connectionString;
            _logger = logger;
        }


        public async Task<UserHoursOffViewModel> GetHoursOffAsync(string tenantId, string userId, DateTime startDate)
        {
            try
            {
                using SqlConnection con = new(_connectionString);
                if (con.State == ConnectionState.Closed)
                    await con.OpenAsync();

                DynamicParameters param = new();
                param.Add("@TenantId", tenantId);
                param.Add("@UserId", userId);
                param.Add("@StartDate", startDate);
                return await con.QuerySingleOrDefaultAsync<UserHoursOffViewModel>("[dbo].[spHoursOffFund_GetUnUsed]", param, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[dbo].[spHoursOffFund_GetUnUsed] GetHoursOffAsync HoursOffFundRepository Error.");
                return new UserHoursOffViewModel();
            }
        }

        public async Task<int> ExtendAsync(string tenantId, string companyId, DataTable table, DateTime newExpiryDate,
            string creatorId, string creatorFullName, string creatorAvatar)
        {
            try
            {
                using SqlConnection con = new(_connectionString);
                await con.OpenAsync();

                var param = new DynamicParameters();
                param.Add("@TenantId", tenantId);
                param.Add("@CompanyId", companyId);
                param.Add("@FundIds", table.AsTableValuedParameter("[dbo].[IdType]"));
                param.Add("@NewExpiryDate", newExpiryDate);
                param.Add("@CreatorId", creatorId);
                param.Add("@CreatorFullName", creatorFullName);
                param.Add("@CreatorAvatar", creatorAvatar);

                return await con.ExecuteScalarAsync<int>(
                    "spHoursOffFund_Extend",
                    param,
                    commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "ExtendAsync HoursOffFundRepository Error. CompanyId: {CompanyId}", companyId);
                return -1;
            }
        }
        #region
        //Chi tiet quan ly nb
        public async Task<List<HoursOffFundDetailItem>> SelectDetailHoursOffAsync(string tenantId, string companyId, string userId, int year, int month, int filterType)
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
                param.Add("@FilterType", filterType);

                var results = await con.QueryAsync<HoursOffFundDetailItem>("[dbo].[spHoursOffFund_GetDetail]", param, commandType: CommandType.StoredProcedure);
                return results.ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[dbo].[spHoursOffFund_GetDetail] SelectDetailHoursOffAsync HoursOffFundRepository Error.");
                return [];
            }
        }

        public async Task<List<HoursOffViewModel>> GetAllHoursOffAsync(string tenantId, string companyId, int year, int month, string keyword, string departmentIds)
        {
            try
            {
                using SqlConnection con = new(_connectionString);
                if (con.State == ConnectionState.Closed)
                    await con.OpenAsync();

                DynamicParameters param = new();
                param.Add("@TenantId", tenantId);
                param.Add("@CompanyId", companyId);
                param.Add("@Year", year);
                param.Add("@Month", month);
                param.Add("@Keyword", keyword);
                param.Add("@DepartmentIds", departmentIds);

                var results = await con.QueryAsync<HoursOffViewModel>("[dbo].[spHoursOffFund_GetList]", param, commandType: CommandType.StoredProcedure);
                return results.ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[dbo].[spHoursOffFund_GetList] GetAllHoursOffAsync HoursOffFundRepository Error.");
                return [];
            }
        }

        public async Task<List<HoursOffFundSummaryItem>> GetSummaryAsync(string tenantId, string companyId, int year, int month, string keyword, string departmentIds)
        {
            try
            {
                using SqlConnection con = new(_connectionString);
                if (con.State == ConnectionState.Closed)
                    await con.OpenAsync();

                DynamicParameters param = new();
                param.Add("@TenantId", tenantId);
                param.Add("@CompanyId", companyId);
                param.Add("@Year", year);
                param.Add("@Month", month);
                param.Add("@Keyword", keyword);
                param.Add("@DepartmentIds", departmentIds);

                var results = await con.QueryAsync<HoursOffFundSummaryItem>("[dbo].[spHoursOffFund_GetSummary]", param, commandType: CommandType.StoredProcedure);
                return results.ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[dbo].[spHoursOffFund_GetSummary] GetSummaryAsync HoursOffFundRepository Error.");
                return [];
            }
        }
        #endregion

        public async Task<List<HoursOffFundExpiringSoon>> GetExpiringSoonAsync(string tenantId, string companyId, string userId, DateTime startDate)
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
                param.Add("@StartDate", startDate);

                var results = await con.QueryAsync<HoursOffFundExpiringSoon>("[dbo].[spHoursOffFund_GetExpiringSoon]", param, commandType: CommandType.StoredProcedure);
                return results.ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[dbo].[spHoursOffFund_GetExpiringSoon] GetExpiringSoonAsync HoursOffFundRepository Error.");
                return [];
            }
        }

        public async Task<List<HoursOffFundExpiredDetailItem>> GetExpiredDetailAsync(string tenantId, string companyId, string userId, int year, int month)
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

                var results = await con.QueryAsync<HoursOffFundExpiredDetailItem>("[dbo].[spHoursOffFund_GetExpiredDetail]", param, commandType: CommandType.StoredProcedure);
                return results.ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[dbo].[spHoursOffFund_GetExpiredDetail] GetExpiredDetailAsync HoursOffFundRepository Error.");
                return [];
            }
        }
    }
}
