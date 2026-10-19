using GHM.HR.API.Domain.IRepository;
using GHM.HR.API.Domain.Models;
using Microsoft.Data.SqlClient;
using System.Data;
using Dapper;
using GHM.HR.API.Domain.ViewModels;
using GHM.HR.API.Infrastructure.Data;
namespace GHM.HR.API.Infrastructure.Repository
{
    public class DepartmentRepository : IDepartmentRepository
    {
        private readonly string _connectionString;
        private readonly ILogger<DepartmentRepository> _logger;
        private readonly IDbSession _dbSession;

        public DepartmentRepository(string connectionString, ILogger<DepartmentRepository> logger,IDbSession dbSession)
        {
            _connectionString = connectionString;
            _logger = logger;
            _dbSession = dbSession;
        }

        public async Task<List<DepartmentSearchViewModel>> SelectAllAsync(string tenantId, string companyId)
        {
            var con = await _dbSession.GetConnectionAsync();
            DynamicParameters param = new();
            param.Add("@TenantId", tenantId);
            param.Add("@CompanyId", companyId);
            var results = await con.QueryAsync<DepartmentSearchViewModel>("[dbo].[spDepartment_SelectAll]", param, transaction: _dbSession.Transaction, commandType: CommandType.StoredProcedure);
            return results.ToList();
        }

        public async Task<List<DepartmentSearchViewModel>> SelectAllDepartmentsActionAsync(string tenantId, string companyId)
        {
            var con = await _dbSession.GetConnectionAsync();
            DynamicParameters param = new();
            param.Add("@TenantId", tenantId);
            param.Add("@CompanyId", companyId);
            var results = await con.QueryAsync<DepartmentSearchViewModel>("[dbo].[spDepartment_SelectAll_Action]", param, transaction: _dbSession.Transaction, commandType: CommandType.StoredProcedure);
            return results.ToList();
        }

        public async Task<int> InsertAsync(Department department)
        {
            var con = await _dbSession.GetConnectionAsync();
            int rowAffected = 0;
            DynamicParameters param = new();
            param.Add("@Id", dbType: DbType.Int32, direction: ParameterDirection.Output);
            param.Add("@TenantId", department.TenantId);
            param.Add("@CompanyId", department.CompanyId);
            param.Add("@ParentId", department.ParentId);
            param.Add("@Name", department.Name);
            param.Add("@Description", department.Description);
            param.Add("@IdPath", department.IdPath);
            param.Add("@NamePath", department.NamePath);
            param.Add("@ChildCount", department.ChildCount);
            param.Add("@ConcurrencyStamp", department.ConcurrencyStamp);
            param.Add("@IsActive", department.IsActive);
            param.Add("@IsDelete", department.IsDelete);
            param.Add("@CreateTime", department.CreateTime);
            param.Add("@CreatorId", department.CreatorId);
            param.Add("@CreatorFullName", department.CreatorFullName);
            if (department.LastUpdate != null && department.LastUpdate != DateTime.MinValue)
            {
                param.Add("@LastUpdate", department.LastUpdate);
            }
            param.Add("@LastUpdateUserId", department.LastUpdateUserId);
            param.Add("@LastUpdateFullName", department.LastUpdateFullName);
            if (department.DeleteTime != null && department.DeleteTime != DateTime.MinValue)
            {
                param.Add("@DeleteTime", department.DeleteTime);
            }
            param.Add("@DeleteUserId", department.DeleteUserId);
            param.Add("@DeleteFullName", department.DeleteFullName);
            param.Add("@AdvanceLeaveGranted", department.AdvanceLeaveGranted);
            param.Add("@CompLeaveGranted", department.CompLeaveGranted);
            param.Add("@ExpireDays", department.ExpireDays);
            rowAffected = await con.ExecuteAsync("[dbo].[spDepartment_Insert]", param, transaction: _dbSession.Transaction, commandType: CommandType.StoredProcedure);
            department.Id = param.Get<int>("@Id");
            return department.Id;
        }

        public async Task<int> UpdateAsync(Department department)
        {
            var con = await _dbSession.GetConnectionAsync();
            int rowAffected = 0;
            DynamicParameters param = new();
            param.Add("@Id", department.Id);
            param.Add("@TenantId", department.TenantId);
            param.Add("@CompanyId", department.CompanyId);
            param.Add("@ParentId", department.ParentId);
            param.Add("@Name", department.Name);
            param.Add("@Description", department.Description);
            param.Add("@IdPath", department.IdPath);
            param.Add("@NamePath", department.NamePath);
            param.Add("@ChildCount", department.ChildCount);
            param.Add("@ConcurrencyStamp", department.ConcurrencyStamp);
            param.Add("@IsActive", department.IsActive);
            param.Add("@IsDelete", department.IsDelete);
            param.Add("@CreateTime", department.CreateTime);
            param.Add("@CreatorId", department.CreatorId);
            param.Add("@CreatorFullName", department.CreatorFullName);
            if (department.LastUpdate != null && department.LastUpdate != DateTime.MinValue)
            {
                param.Add("@LastUpdate", department.LastUpdate);
            }
            param.Add("@LastUpdateUserId", department.LastUpdateUserId);
            param.Add("@LastUpdateFullName", department.LastUpdateFullName);
            if (department.DeleteTime != null && department.DeleteTime != DateTime.MinValue)
            {
                param.Add("@DeleteTime", department.DeleteTime);
            }
            param.Add("@DeleteUserId", department.DeleteUserId);
            param.Add("@DeleteFullName", department.DeleteFullName);
            param.Add("@AdvanceLeaveGranted", department.AdvanceLeaveGranted);
            param.Add("@CompLeaveGranted", department.CompLeaveGranted);
            param.Add("@ExpireDays", department.ExpireDays);
            rowAffected = await con.ExecuteAsync("[dbo].[spDepartment_Update]", param, transaction: _dbSession.Transaction, commandType: CommandType.StoredProcedure);
            return rowAffected;
        }

        public async Task<Department> GetInfoAsync(int? id)
        {
            var con = await _dbSession.GetConnectionAsync();
            DynamicParameters param = new();
            param.Add("@Id", id);

            return await con.QuerySingleOrDefaultAsync<Department>("[dbo].[spDepartment_SelectByID]", param, transaction: _dbSession.Transaction, commandType: CommandType.StoredProcedure);
        }

        public async Task<bool> CheckExistsByNameAsync(string tenantId, string companyId, int id, string name)
        {
            var con = await _dbSession.GetConnectionAsync();
            var sql = @"
					SELECT IIF (EXISTS (SELECT 1 FROM Departments WHERE TenantId = @TenantId AND Name = @Name AND IsDelete = 0 AND IsActive = 1 AND CompanyId = @CompanyId AND Id != @Id), 1, 0)";

            var result = await con.ExecuteScalarAsync<bool>(sql, new { TenantId = tenantId, CompanyId = companyId, Name = name, Id = id });
            return result;
        }



        public async Task<bool> CheckExistsByTenantIdAsync(string tenantId, string companyId, int id)
        {
            var con = await _dbSession.GetConnectionAsync();
            var sql = @"
					SELECT IIF (EXISTS (SELECT 1 FROM Departments WHERE Id = @Id AND IsActive = 1 AND IsDelete = 0 AND CompanyId=@CompanyId), 1, 0)";

            var result = await con.ExecuteScalarAsync<bool>(sql, new { TenantId = tenantId, CompanyId = companyId, Id = id });
            return result;
        }


        public async Task<int> Update_IdPath_NamePath_DepartmentPathAsync(int id)
        {
            var con = await _dbSession.GetConnectionAsync();
            int rowAffected = 0;
            DynamicParameters param = new();
            param.Add("@Id", id);
            rowAffected = await con.ExecuteAsync("[dbo].[spDepartment_Update_IdPath_NamePath_DepartmentPath]", param, transaction: _dbSession.Transaction, commandType: CommandType.StoredProcedure);
            return rowAffected;
        }

        public async Task<int> Update_ChildCountAsync(int id)
        {
            var con = await _dbSession.GetConnectionAsync();
            int rowAffected = 0;
            DynamicParameters param = new();
            param.Add("@Id", id);
            rowAffected = await con.ExecuteAsync("[dbo].[spDepartment_Update_ChildCount]", param, transaction: _dbSession.Transaction, commandType: CommandType.StoredProcedure);
            return rowAffected;
        }

        public async Task<int> Update_IsActiveAsync(int id, bool isActive)
        {
            var con = await _dbSession.GetConnectionAsync();
            int rowAffected = 0;
            DynamicParameters param = new();
            param.Add("@Id", id);
            param.Add("@IsActive", isActive);
            rowAffected = await con.ExecuteAsync("[dbo].[spDepartment_Update_IsActive]", param, transaction: _dbSession.Transaction, commandType: CommandType.StoredProcedure);
            return rowAffected;
        }

        public async Task<List<int>> GetIdChildrenAsync(int id)
        {
            var con = await _dbSession.GetConnectionAsync();
            DynamicParameters param = new();
            param.Add("@Id", id);
            var results = await con.QueryAsync<int>("[dbo].[spDepartment_GetIdChildren]", param, transaction: _dbSession.Transaction, commandType: CommandType.StoredProcedure);
            return results.ToList();
        }

        public async Task<int> UpdateByDepartmentNameAsync(string tenantId, string companyId, int departmentId, string departmentName)
        {
            var con = await _dbSession.GetConnectionAsync();
            int rowAffected = 0;
            DynamicParameters param = new();
            param.Add("@TenantId", tenantId);
            param.Add("@CompanyId", companyId);
            param.Add("@DepartmentId", departmentId);
            param.Add("@DepartmentName", departmentName);

            rowAffected = await con.ExecuteAsync("[dbo].[Update_By_DepartmentName]", param, transaction: _dbSession.Transaction, commandType: CommandType.StoredProcedure);
            return rowAffected;
        }

        public async Task<int> DeleteAsync(Department department)
        {
            var con = await _dbSession.GetConnectionAsync();
            int rowAffected = 0;
            DynamicParameters param = new();
            param.Add("@Id", department.Id);
            param.Add("@DeleteUserId", department.DeleteUserId);
            param.Add("@DeleteFullName", department.DeleteFullName);
            rowAffected = await con.ExecuteAsync("[dbo].[spDepartment_DeleteByID]", param, transaction: _dbSession.Transaction, commandType: CommandType.StoredProcedure);
            return rowAffected;
        }

        public async Task<bool> CheckParentIdAsync(int id, int parentId)
        {
            var con = await _dbSession.GetConnectionAsync();
            DynamicParameters param = new();
            param.Add("@Id", id);
            param.Add("@ParentId", parentId);
            var result = await con.ExecuteScalarAsync<bool>("[dbo].[spDepartment_Check_ParentId]", param, transaction: _dbSession.Transaction, commandType: CommandType.StoredProcedure);
            return result;
        }

        public async Task<bool> CheckExistNameAsync(string tenantId, string companyId, string name)
        {
            var con = await _dbSession.GetConnectionAsync();
            var sql = @"
					SELECT IIF (EXISTS (SELECT 1 FROM Departments WHERE TenantId = @TenantId AND Name = @Name AND IsDelete = 0 AND IsActive = 1 AND CompanyId = @CompanyId), 1, 0)";

            var result = await con.ExecuteScalarAsync<bool>(sql, new { TenantId = tenantId, CompanyId = companyId, Name = name });
            return result;
        }

        public async Task<List<DepartmentSearchViewModel>> SelectDepartmentByTenantIdAsync(string tenantId, string companyId, string userId)
        {
            var con = await _dbSession.GetConnectionAsync();
            DynamicParameters param = new();
            param.Add("@TenantId", tenantId);
            param.Add("@CompanyId", companyId);
            param.Add("@UserId", userId);
            var results = await con.QueryAsync<DepartmentSearchViewModel>("[dbo].[spDepartment_SelectByTenantId]", param, transaction: _dbSession.Transaction, commandType: CommandType.StoredProcedure);
            return results.ToList();
        }






    }
}
