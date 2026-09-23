using GHM.HR.API.Domain.IRepository;
using GHM.HR.API.Domain.Models;
using Microsoft.Data.SqlClient;
using System.Data;
using Dapper;
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

        public async Task<bool> CheckExistNameAsync(string companyId, string name)
        {
            try
            {
                using SqlConnection con = new(_connectionString);
                if (con.State == ConnectionState.Closed)
                    await con.OpenAsync();

                var sql = @"
					SELECT IIF (EXISTS (SELECT 1 FROM Departments WHERE Name = @Name AND IsDelete = 0 AND IsActive = 1 AND CompanyId = @CompanyId), 1, 0)";

                var result = await con.ExecuteScalarAsync<bool>(sql, new { CompanyId = companyId, Name = name });
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "CheckExistNameAsync DepartmentRepository Error.");
                return false;
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

        public async Task<Department?> GetInfoAsync(int? id)
        {
            try
            {
                using SqlConnection con = new(_connectionString);
                if (con.State == ConnectionState.Closed)
                    await con.OpenAsync();

                DynamicParameters param = new();
                param.Add("@Id", id);

                return await con.QuerySingleOrDefaultAsync<Department?>("[dbo].[spDepartment_SelectByID]", param, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[dbo].[spDepartment_SelectByID] GetInfoAsyncDepartmentRepository Error.");
                return null;
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
                    param.Add("@CompanyId", department.CompanyId);
                    param.Add("@ParentId", department.ParentId);
                    param.Add("@Name", department.Name);
                    param.Add("@Description", department.Description);
                    param.Add("@ChildCount", department.ChildCount);
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
                    param.Add("@CompanyId", department.CompanyId);
                    param.Add("@ParentId", department.ParentId);
                    param.Add("@Name", department.Name);
                    param.Add("@Description", department.Description);
                    param.Add("@ChildCount", 0);
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
    }
}
