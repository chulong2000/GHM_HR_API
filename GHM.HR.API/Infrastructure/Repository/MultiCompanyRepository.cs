using Dapper;
using GHM.HR.API.Domain.IRepository;
using GHM.HR.API.Domain.Models;
using GHM.HR.API.Domain.ViewModels;
using GHM.HR.API.Infrastructure.Data;
using GHM.HR.Domain.Models;
using GHM.HR.Domain.ViewModels;
using Microsoft.AspNetCore.Connections;
using Microsoft.Data.SqlClient;
using System.ComponentModel.Design;
using System.Data;
using System.Data.Common;
using System.Xml.Linq;

namespace GHM.HR.API.Infrastructure.Repository
{
    public class MultiCompanyRepository : IMultiCompanyRepository
    {
        private readonly string _connectionString;
        private readonly ILogger<DepartmentRepository> _logger;
        private readonly IDbSession _dbSession;

        public MultiCompanyRepository(string connectionString, ILogger<DepartmentRepository> logger, IDbSession dbSession)
        {
            _connectionString = connectionString;
            _logger = logger;
            _dbSession = dbSession;
        }
        public async Task<bool> CheckExistDoctorCodeAsync(string tenantId, string companyId, string doctorCode)
        {
            var con = await _dbSession.GetConnectionAsync();
            var sql = @"
					 SELECT IIF (EXISTS (SELECT 1 FROM dbo.MultipleCompanys WHERE TenantId = @TenantId AND DoctorCode = @DoctorCode AND CompanyId = @CompanyId), 1, 0)";

            var result = await con.ExecuteScalarAsync<bool>(sql, new { TenantId = tenantId, CompanyId = companyId, DoctorCode = doctorCode });
            return result;
        }

        public async Task<List<string>> InsertBulkAsync(List<MultiCompany> multiCompanies)
        {
            var con = await _dbSession.GetConnectionAsync();
            const int bulkBatchSize = 1000;
            const int bulkCommandTimeoutSeconds = 300;

            if (multiCompanies == null || multiCompanies.Count == 0)
                return new List<string>();

            var insertedIds = new List<string>();
            
            
                

            for (var startIndex = 0; startIndex < multiCompanies.Count; startIndex += bulkBatchSize)
            {
                var batchCount = Math.Min(bulkBatchSize, multiCompanies.Count - startIndex);
                var table = BuildMultiCompanyBulkTable(multiCompanies, startIndex, batchCount);

                DynamicParameters param = new();
                param.Add("@MultiCompanies", table.AsTableValuedParameter("dbo.MultiCompanyBulkInsertList"));

                var batchIds = await con.QueryAsync<string>(new CommandDefinition(
                    "[dbo].[spMultiCompany_InsertBulk]", param,
                    commandTimeout: bulkCommandTimeoutSeconds,
                    transaction: _dbSession.Transaction, commandType: CommandType.StoredProcedure));

                insertedIds.AddRange(batchIds);
            }

            return insertedIds;
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
            var con = await _dbSession.GetConnectionAsync();
                DynamicParameters param = new();
                param.Add("@TenantId", tenantId);
                param.Add("@UserId", userId);
                param.Add("@CreatorId", creatorId);
                param.Add("@CreatorFullName", creatorFullName);
                param.Add("@List", dataTable.AsTableValuedParameter("[dbo].[MultipleCompanyType]"));
                await con.ExecuteAsync("[dbo].[spMultipleCompanys_InsertByTableType]", param, transaction: _dbSession.Transaction, commandType: CommandType.StoredProcedure);
        }

        public async Task<List<MultipleCompanySearchViewModel>> GetMultiCompaniesAsync(string tenantId, string companyId, string userId)
        {
            var con = await _dbSession.GetConnectionAsync();
            DynamicParameters param = new();
            param.Add("@TenantId", tenantId);
            param.Add("@CompanyId", companyId);
            param.Add("@UserId", userId);
            var results = await con.QueryAsync<MultipleCompanySearchViewModel>("[dbo].[spMultipleCompanys_SearchMultiCompanyForUser]", param, transaction: _dbSession.Transaction, commandType: CommandType.StoredProcedure);
            return results.ToList();
        }

        public async Task<int> ForceDeleteByUserIdAsync(string tenantId, string userId)
        {
            var con = await _dbSession.GetConnectionAsync();
            int rowAffected = 0;
                DynamicParameters param = new();
                param.Add("@UserId", userId);
                param.Add("@TenantId", tenantId);
                rowAffected = await con.ExecuteAsync("[dbo].[spMultiCompany_ForceDeleteByUserId]", param, transaction: _dbSession.Transaction, commandType: CommandType.StoredProcedure);
            return rowAffected;
        }

        public async Task<List<MultiCompany>> GetInfoAsync(string tenantId, string userId)
        {
            var con = await _dbSession.GetConnectionAsync();
            DynamicParameters param = new();
            param.Add("@UserId", userId);
            param.Add("@TenantId", tenantId);
            var results = await con.QueryAsync<MultiCompany>("[dbo].[spMultiCompany_GetAllCompanyOfUser]", param, transaction: _dbSession.Transaction, commandType: CommandType.StoredProcedure);
            return results.ToList();
        }

        public async Task<CheckDoctorCodeResult> CheckDoctorCode(string tenantId, string companyId, DataTable dataTable)
        {
            var con = await _dbSession.GetConnectionAsync();
            DynamicParameters param = new();
            param.Add("@TenantId", tenantId);
            param.Add("@CompanyId", companyId);
            param.Add("@List", dataTable.AsTableValuedParameter("[dbo].[MultipleCompanyType]"));
            param.Add("@DuplicateCount", dbType: DbType.Int32, direction: ParameterDirection.Output);

            var results = await con.QueryAsync<DoctorCodeDuplicateDto>(
                                           "dbo.spMultipleCompanys_CheckDoctorCode", param,
                                            transaction: _dbSession.Transaction, commandType: CommandType.StoredProcedure);
            var count = param.Get<int>("@DuplicateCount");

            return new CheckDoctorCodeResult(count, results.ToList());
        }
    }
}
