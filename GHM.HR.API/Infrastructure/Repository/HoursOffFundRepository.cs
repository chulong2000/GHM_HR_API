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
            DynamicParameters param = new();
            param.Add("@TenantId", tenantId);
            param.Add("@UserId", userId);
            param.Add("@StartDate", startDate);
            return await con.QuerySingleOrDefaultAsync<UserHoursOffViewModel>("[dbo].[spHoursOffFund_GetUnUsed]", param, commandType: CommandType.StoredProcedure);
        }

        public async Task<int> ExtendAsync(string tenantId, string companyId, DataTable table, DateTime newExpiryDate,
            string creatorId, string creatorFullName, string creatorAvatar)
        {
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

        public async Task<List<HoursOffFundDetailItem>> SelectDetailHoursOffAsync(string tenantId, string companyId, string userId, int year, int month, int filterType)
        {
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

        public async Task<List<HoursOffViewModel>> GetAllHoursOffAsync(string tenantId, string companyId, int year, int month, string keyword, string departmentIds)
        {
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

        public async Task<List<HoursOffFundSummaryItem>> GetSummaryAsync(string tenantId, string companyId, int year, int month, string keyword, string departmentIds)
        {
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


        public async Task<List<HoursOffFundExpiringSoon>> GetExpiringSoonAsync(string tenantId, string companyId, string userId, DateTime startDate)
        {
            DynamicParameters param = new();
            param.Add("@TenantId", tenantId);
            param.Add("@CompanyId", companyId);
            param.Add("@UserId", userId);
            param.Add("@StartDate", startDate);

            var results = await con.QueryAsync<HoursOffFundExpiringSoon>("[dbo].[spHoursOffFund_GetExpiringSoon]", param, commandType: CommandType.StoredProcedure);
            return results.ToList();
        }

        public async Task<List<HoursOffFundExpiredDetailItem>> GetExpiredDetailAsync(string tenantId, string companyId, string userId, int year, int month)
        {
            DynamicParameters param = new();
            param.Add("@TenantId", tenantId);
            param.Add("@CompanyId", companyId);
            param.Add("@UserId", userId);
            param.Add("@Year", year);
            param.Add("@Month", month);

            var results = await con.QueryAsync<HoursOffFundExpiredDetailItem>("[dbo].[spHoursOffFund_GetExpiredDetail]", param, commandType: CommandType.StoredProcedure);
            return results.ToList();
        }
    }
}
