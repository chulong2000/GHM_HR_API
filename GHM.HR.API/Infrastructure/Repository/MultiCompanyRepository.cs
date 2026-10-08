using Dapper;
using GHM.HR.API.Domain.IRepository;
using GHM.HR.API.Domain.Models;
using GHM.HR.API.Domain.ViewModels;
using GHM.HR.Domain.Models;
using GHM.HR.Domain.ViewModels;
using Microsoft.AspNetCore.Connections;
using Microsoft.Data.SqlClient;
using System.ComponentModel.Design;
using System.Data;
using System.Xml.Linq;

namespace GHM.HR.API.Infrastructure.Repository
{
    public class MultiCompanyRepository : IMultiCompanyRepository
    {
        private readonly string _connectionString;
        private readonly ILogger<DepartmentRepository> _logger;

        public MultiCompanyRepository(string connectionString, ILogger<DepartmentRepository> logger)
        {
            _connectionString = connectionString;
            _logger = logger;
        }
        public async Task<bool> CheckExistDoctorCodeAsync(string tenantId, string companyId, string doctorCode)
        {
            try
            {
                using SqlConnection con = new(_connectionString);
                if (con.State == ConnectionState.Closed)
                    await con.OpenAsync();

                var sql = @"
					 SELECT IIF (EXISTS (SELECT 1 FROM dbo.MultipleCompanys WHERE TenantId = @TenantId AND DoctorCode = @DoctorCode AND CompanyId = @CompanyId), 1, 0)";

                var result = await con.ExecuteScalarAsync<bool>(sql, new { TenantId = tenantId, CompanyId = companyId, DoctorCode = doctorCode });
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "CheckExistDoctorCodeAsync MultiCompanyRepository Error.");
                return false;
            }
        }

        public async Task<List<string>> InsertBulkAsync(List<MultiCompany> multiCompanies)
        {
            const int bulkBatchSize = 1000;
            const int bulkCommandTimeoutSeconds = 300;

            if (multiCompanies == null || multiCompanies.Count == 0)
                return new List<string>();

            try
            {
                var insertedIds = new List<string>();
                using SqlConnection con = new(_connectionString);
                if (con.State == ConnectionState.Closed)
                    await con.OpenAsync();

                for (var startIndex = 0; startIndex < multiCompanies.Count; startIndex += bulkBatchSize)
                {
                    var batchCount = Math.Min(bulkBatchSize, multiCompanies.Count - startIndex);
                    var table = BuildMultiCompanyBulkTable(multiCompanies, startIndex, batchCount);

                    DynamicParameters param = new();
                    param.Add("@MultiCompanies", table.AsTableValuedParameter("dbo.MultiCompanyBulkInsertList"));

                    var batchIds = await con.QueryAsync<string>(new CommandDefinition(
                        "[dbo].[spMultiCompany_InsertBulk]", param,
                        commandTimeout: bulkCommandTimeoutSeconds,
                        commandType: CommandType.StoredProcedure));

                    insertedIds.AddRange(batchIds);
                }

                return insertedIds;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[dbo].[spMultiCompany_InsertBulk] InsertBulkAsync MultiCompanyRepository Error.");
                return new List<string>();
            }
        }

       
        private static DataTable BuildMultiCompanyBulkTable(IReadOnlyList<MultiCompany> multiCompanies, int startIndex, int count)
        {
            var table = new DataTable();
            table.Columns.Add("Id", typeof(string));
            table.Columns.Add("TenantId", typeof(string));
            table.Columns.Add("UserId", typeof(string));
            table.Columns.Add("CompanyId", typeof(string));
            table.Columns.Add("DepartmentId", typeof(string));
            table.Columns.Add("PositionId", typeof(string));
            table.Columns.Add("PositionName", typeof(string));
            table.Columns.Add("DepartmentName", typeof(string));
            table.Columns.Add("CreateTime", typeof(DateTime));
            table.Columns.Add("CreatorId", typeof(string));
            table.Columns.Add("CreatorFullName", typeof(string));

            static object Date(DateTime? value) =>
                value.HasValue && value.Value != DateTime.MinValue ? value.Value : DBNull.Value;

            for (var i = 0; i < count; i++)
            {
                var u = multiCompanies[startIndex + i];
                table.Rows.Add(
                    startIndex + i + 1,
                    (object)u.Id ?? DBNull.Value,
                    (object)u.TenantId ?? DBNull.Value,
                    (object)u.UserId ?? DBNull.Value,
                    (object)u.CompanyId ?? DBNull.Value,
                    (object)u.DepartmentId ?? DBNull.Value,
                    (object)u.PositionId ?? DBNull.Value,
                    (object)u.PositionName ?? DBNull.Value,
                    (object)u.DepartmentName ?? DBNull.Value,
                    u.CreateTime == DateTime.MinValue ? DateTime.Now : u.CreateTime,
                    (object)u.CreatorId ?? DBNull.Value,
                    (object)u.CreatorFullName ?? DBNull.Value);
             
            }

            return table;
        }

        public async Task InsertMultiCompanyAsync(string tenantId, string userId, string creatorId, string creatorFullName, DataTable dataTable)
        {
            try
            {
                using (SqlConnection con = new(_connectionString))
                {
                    if (con.State == ConnectionState.Closed)
                        await con.OpenAsync();

                    DynamicParameters param = new();
                    param.Add("@TenantId", tenantId);
                    param.Add("@UserId", userId);
                    param.Add("@CreatorId", creatorId);
                    param.Add("@CreatorFullName", creatorFullName);
                    param.Add("@List", dataTable.AsTableValuedParameter("[dbo].[MultipleCompanyType]"));
                    await con.ExecuteAsync("[dbo].[spMultipleCompanys_InsertByTableType]", param, commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[dbo].[spMultipleCompanys_InsertManyData] InsertAsync DepartmentRepository Error.");
            }
        }

        public async Task<List<MultipleCompanySearchViewModel>> GetMultiCompaniesAsync(string tenantId, string companyId, string userId)
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
                var results = await con.QueryAsync<MultipleCompanySearchViewModel>("[dbo].[spMultipleCompanys_SearchMultiCompanyForUser]", param, commandType: CommandType.StoredProcedure);
                return results.ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[dbo].[spMultipleCompanys_SearchMultiCompanyForUser]  spMultipleCompanys_SearchMultiCompanyForUser Error.");
                return new List<MultipleCompanySearchViewModel>();
            }
        }

        public async Task<int> ForceDeleteByUserIdAsync(string tenantId, string userId)
        {
            try
            {
                int rowAffected = 0;
                using (SqlConnection con = new(_connectionString))
                {
                    if (con.State == ConnectionState.Closed)
                        await con.OpenAsync();

                    DynamicParameters param = new();
                    param.Add("@UserId", userId);
                    param.Add("@TenantId", tenantId);
                    rowAffected = await con.ExecuteAsync("[dbo].[spMultiCompany_ForceDeleteByUserId]", param, commandType: CommandType.StoredProcedure);
                }
                return rowAffected;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[dbo].[spMultiCompany_ForceDeleteByUserId] ForceDeleteByUserIdAsync MultiCompanyRepository Error.");
                return -1;
            }
        }

        public async Task<List<MultiCompany>> GetInfoAsync(string tenantId, string userId)
        {
            try
            {
                using SqlConnection con = new(_connectionString);
                if (con.State == ConnectionState.Closed)
                    await con.OpenAsync();

                DynamicParameters param = new();
                param.Add("@UserId", userId);
                param.Add("@TenantId", tenantId);
                var results = await con.QueryAsync<MultiCompany>("[dbo].[spMultiCompany_GetAllCompanyOfUser]", param, commandType: CommandType.StoredProcedure);
                return results.ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[dbo].[spMultiCompany_GetAllCompanyOfUser] GetInfoAsyncMultiCompanyRepository Error.");
                Console.WriteLine(ex.ToString());
                return new List<MultiCompany>();
            }
        }
    }
}
