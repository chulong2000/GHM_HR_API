using GHM.HR.API.Domain.IRepository;
using GHM.HR.API.Domain.Models;
using Microsoft.Data.SqlClient;
using System.Data;
using Dapper;
using GHM.HR.API.Domain.ViewModels;
namespace GHM.HR.API.Infrastructure.Repository
{
    public class DepartmentRepository : IDepartmentRepository
    {
        private readonly string _connectionString;
        private readonly ILogger<DepartmentRepository> _logger;

        public DepartmentRepository(string connectionString, ILogger<DepartmentRepository> logger)
        {
            _connectionString = connectionString;
            _logger = logger;
        }

        public async Task<List<DepartmentSearchViewModel>> SelectAllAsync(string tenantId, string companyId)
        {
            try
            {
                using SqlConnection con = new(_connectionString);
                if (con.State == ConnectionState.Closed)
                    await con.OpenAsync();

                DynamicParameters param = new();
                param.Add("@TenantId", tenantId);
                param.Add("@CompanyId", companyId);
                var results = await con.QueryAsync<DepartmentSearchViewModel>("[dbo].[spDepartment_SelectAll]", param, commandType: CommandType.StoredProcedure);
                return results.ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[dbo].[spDepartment_SelectAll] SelectAllAsync DepartmentRepository Error.");
                return new List<DepartmentSearchViewModel>();
            }
        }

        public async Task<List<DepartmentSearchViewModel>> SelectAllDepartmentsActionAsync(string tenantId, string companyId)
        {
            try
            {
                using SqlConnection con = new(_connectionString);
                if (con.State == ConnectionState.Closed)
                    await con.OpenAsync();

                DynamicParameters param = new();
                param.Add("@TenantId", tenantId);
                param.Add("@CompanyId", companyId);
                var results = await con.QueryAsync<DepartmentSearchViewModel>("[dbo].[spDepartment_SelectAll_Action]", param, commandType: CommandType.StoredProcedure);
                return results.ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[dbo].[spDepartment_SelectAll_Action] SelectAllAsync_Action DepartmentRepository Error.");
                return new List<DepartmentSearchViewModel>();
            }
        }

        public async Task<int> InsertAsync(Department department)
        {
            try
            {
                int rowAffected = 0;
                using (SqlConnection con = new(_connectionString))
                {
                    if (con.State == ConnectionState.Closed)
                        await con.OpenAsync();

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
                    rowAffected = await con.ExecuteAsync("[dbo].[spDepartment_Insert]", param, commandType: CommandType.StoredProcedure);
                    department.Id = param.Get<int>("@Id");
                }
                return department.Id;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[dbo].[spDepartment_Insert] InsertAsync DepartmentRepository Error.");
                return -1;
            }
        }

        public async Task<int> UpdateAsync(Department department)
        {
            try
            {
                int rowAffected = 0;
                using (SqlConnection con = new(_connectionString))
                {
                    if (con.State == ConnectionState.Closed)
                        await con.OpenAsync();

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
                    rowAffected = await con.ExecuteAsync("[dbo].[spDepartment_Update]", param, commandType: CommandType.StoredProcedure);

                }
                return rowAffected;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[dbo].[spDepartment_Update] UpdateAsync DepartmentRepository Error.");
                return -1;
            }
        }

        public async Task<Department> GetInfoAsync(int? id)
        {
            try
            {
                using SqlConnection con = new(_connectionString);
                if (con.State == ConnectionState.Closed)
                    await con.OpenAsync();

                DynamicParameters param = new();
                param.Add("@Id", id);

                return await con.QuerySingleOrDefaultAsync<Department>("[dbo].[spDepartment_SelectByID]", param, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[dbo].[spDepartment_SelectByID] GetInfoAsyncDepartmentRepository Error.");
                return null;
            }
        }

        public async Task<bool> CheckExistsByNameAsync(string tenantId, string companyId, int id, string name)
        {
            try
            {
                using SqlConnection con = new(_connectionString);
                if (con.State == ConnectionState.Closed)
                    await con.OpenAsync();

                var sql = @"
					SELECT IIF (EXISTS (SELECT 1 FROM Departments WHERE TenantId = @TenantId AND Name = @Name AND IsDelete = 0 AND IsActive = 1 AND CompanyId = @CompanyId AND Id != @Id), 1, 0)";

                var result = await con.ExecuteScalarAsync<bool>(sql, new { TenantId = tenantId, CompanyId = companyId, Name = name, Id = id });
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "CheckExistByNameAsync DepartmentRepository Error.");
                return false;
            }
        }



        public async Task<bool> CheckExistsByTenantIdAsync(string tenantId, string companyId, int id)
        {
            try
            {
                using SqlConnection con = new(_connectionString);
                if (con.State == ConnectionState.Closed)
                    await con.OpenAsync();

                var sql = @"
					SELECT IIF (EXISTS (SELECT 1 FROM Departments WHERE Id = @Id AND TenantId = @TenantId AND IsActive = 1 AND IsDelete = 0 AND CompanyId=@CompanyId), 1, 0)";

                var result = await con.ExecuteScalarAsync<bool>(sql, new { TenantId = tenantId, CompanyId = companyId, Id = id });
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "CheckExistsByTenantIdAsync DepartmentRepository Error.");
                return false;
            }
        }


        public async Task<int> Update_IdPath_NamePath_DepartmentPathAsync(int id)
        {
            try
            {
                int rowAffected = 0;
                using (SqlConnection con = new(_connectionString))
                {
                    if (con.State == ConnectionState.Closed)
                        await con.OpenAsync();

                    DynamicParameters param = new();
                    param.Add("@Id", id);
                    rowAffected = await con.ExecuteAsync("[dbo].[spDepartment_Update_IdPath_NamePath_DepartmentPath]", param, commandType: CommandType.StoredProcedure);
                }
                return rowAffected;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[dbo].[spDepartment_Update_IdPath_NamePath_DepartmentPath] Update_IdPath_NamePath_Level_DepartmentPathAsync Error.");
                return -1;
            }
        }

        public async Task<int> Update_ChildCountAsync(int id)
        {
            try
            {
                int rowAffected = 0;
                using (SqlConnection con = new(_connectionString))
                {
                    if (con.State == ConnectionState.Closed)
                        await con.OpenAsync();

                    DynamicParameters param = new();
                    param.Add("@Id", id);
                    rowAffected = await con.ExecuteAsync("[dbo].[spDepartment_Update_ChildCount]", param, commandType: CommandType.StoredProcedure);
                }
                return rowAffected;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[dbo].[spDepartment_Update_ChildCount] Update_ChildCountAsync Error.");
                return -1;
            }
        }

        public async Task<int> Update_IsActiveAsync(int id, bool isActive)
        {
            try
            {
                int rowAffected = 0;
                using (SqlConnection con = new(_connectionString))
                {
                    if (con.State == ConnectionState.Closed)
                        await con.OpenAsync();

                    DynamicParameters param = new();
                    param.Add("@Id", id);
                    param.Add("@IsActive", isActive);
                    rowAffected = await con.ExecuteAsync("[dbo].[spDepartment_Update_IsActive]", param, commandType: CommandType.StoredProcedure);
                }
                return rowAffected;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[dbo].[spDepartment_Update_IsActive] Update_IsActiveAsync Error.");
                return -1;
            }
        }

        public async Task<List<int>> GetIdChildrenAsync(int id)
        {
            try
            {
                using SqlConnection con = new(_connectionString);
                if (con.State == ConnectionState.Closed)
                    await con.OpenAsync();

                DynamicParameters param = new();
                param.Add("@Id", id);
                var results = await con.QueryAsync<int>("[dbo].[spDepartment_GetIdChildren]", param, commandType: CommandType.StoredProcedure);
                return results.ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[dbo].[spDepartment_GetIdChildren] GetIdChildrenAsync Error.");
                return new List<int>();
            }
        }

        public async Task<int> UpdateByDepartmentNameAsync(string tenantId, string companyId, int departmentId, string departmentName)
        {
            try
            {
                int rowAffected = 0;
                using (SqlConnection con = new(_connectionString))
                {
                    if (con.State == ConnectionState.Closed)
                        await con.OpenAsync();

                    DynamicParameters param = new();
                    param.Add("@TenantId", tenantId);
                    param.Add("@CompanyId", companyId);
                    param.Add("@DepartmentId", departmentId);
                    param.Add("@DepartmentName", departmentName);

                    rowAffected = await con.ExecuteAsync("[dbo].[Update_By_DepartmentName]", param, commandType: CommandType.StoredProcedure);
                }
                return rowAffected;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[dbo].[Update_By_DepartmentName] UpdateByDepartmentNameAsync  DepartmentRepository Error.");
                return -1;
            }
        }

        public async Task<int> DeleteAsync(Department department)
        {
            try
            {
                int rowAffected = 0;
                using (SqlConnection con = new(_connectionString))
                {
                    if (con.State == ConnectionState.Closed)
                        await con.OpenAsync();

                    DynamicParameters param = new();
                    param.Add("@Id", department.Id);
                    param.Add("@DeleteUserId", department.DeleteUserId);
                    param.Add("@DeleteFullName", department.DeleteFullName);
                    rowAffected = await con.ExecuteAsync("[dbo].[spDepartment_DeleteByID]", param, commandType: CommandType.StoredProcedure);
                }
                return rowAffected;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[dbo].[spDepartment_DeleteByID] DeleteAsync DepartmentRepository Error.");
                return -1;
            }
        }

        public async Task<bool> CheckParentIdAsync(int id, int parentId)
        {
            try
            {
                using SqlConnection con = new(_connectionString);
                if (con.State == ConnectionState.Closed)
                    await con.OpenAsync();

                DynamicParameters param = new();
                param.Add("@Id", id);
                param.Add("@ParentId", parentId);
                var result = await con.ExecuteScalarAsync<bool>("[dbo].[spDepartment_Check_ParentId]", param, commandType: CommandType.StoredProcedure);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "spDepartment_Check_ParentId CheckParentIdAsync Error.");
                return false;
            }
        }

        public async Task<bool> CheckExistNameAsync(string tenantId, string companyId, string name)
        {
            try
            {
                using SqlConnection con = new(_connectionString);
                if (con.State == ConnectionState.Closed)
                    await con.OpenAsync();

                var sql = @"
					SELECT IIF (EXISTS (SELECT 1 FROM Departments WHERE TenantId = @TenantId AND Name = @Name AND IsDelete = 0 AND IsActive = 1 AND CompanyId = @CompanyId), 1, 0)";

                var result = await con.ExecuteScalarAsync<bool>(sql, new { TenantId = tenantId, CompanyId = companyId, Name = name });
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "CheckExistNameAsync DepartmentRepository Error.");
                return false;
            }
        }

        public async Task<List<DepartmentSearchViewModel>> SelectDepartmentByTenantIdAsync(string tenantId, string companyId, string userId)
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
                var results = await con.QueryAsync<DepartmentSearchViewModel>("[dbo].[spDepartment_SelectByTenantId]", param, commandType: CommandType.StoredProcedure);
                return results.ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[dbo].[spDepartment_SelectByTenantId] SelectByTenantIdAsync DepartmentRepository Error.");
                return new List<DepartmentSearchViewModel>();
            }
        }






    }
}
