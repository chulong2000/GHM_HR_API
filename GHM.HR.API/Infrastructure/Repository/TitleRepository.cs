using Dapper;
using GHM.HR.API.Domain.IRepository;
using GHM.HR.API.Domain.Models;
using GHM.HR.API.Domain.ViewModels;
using Microsoft.Data.SqlClient;
using System.Data;

namespace GHM.HR.API.Infrastructure.Repository
{
    public class TitleRepository : ITitleRepository
    {
        private readonly string _connectionString;
        private readonly ILogger<TitleRepository> _logger;
        public TitleRepository(string connectionString, ILogger<TitleRepository> logger)
        {
            _connectionString = connectionString;
            _logger = logger;
        }

        public async Task<List<TitleSearchViewModel>> SelectAllAsync(string tenantId, string companyId)
        {
            try
            {
                using SqlConnection con = new(_connectionString);
                if (con.State == ConnectionState.Closed)
                    await con.OpenAsync();

                DynamicParameters param = new();
                param.Add("@TenantId", tenantId);
                param.Add("@CompanyId", companyId);
                var results = await con.QueryAsync<TitleSearchViewModel>("[dbo].[spTitle_SelectAll]", param, commandType: CommandType.StoredProcedure);
                return results.ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[dbo].[spTitle_SelectAll] SelectAllAsync TitleRepository Error.");
                return new List<TitleSearchViewModel>();
            }
        }

        public async Task<List<TitleSearchViewModel>> SelectAllTitleActiveAsync(string tenantId, string companyId)
        {
            try
            {
                using SqlConnection con = new(_connectionString);
                if (con.State == ConnectionState.Closed)
                    await con.OpenAsync();

                DynamicParameters param = new();
                param.Add("@TenantId", tenantId);
                param.Add("@CompanyId", companyId);
                var results = await con.QueryAsync<TitleSearchViewModel>("[dbo].[spTitle_SelectAll_Active]", param, commandType: CommandType.StoredProcedure);
                return results.ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[dbo].[spTitle_SelectAll_Active] SelectAllActiveAsync TitleRepository Error.");
                return new List<TitleSearchViewModel>();
            }
        }

        public async Task<int> InsertAsync(Title title)
        {
            try
            {
                int rowAffected = 0;
                using (SqlConnection con = new(_connectionString))
                {
                    if (con.State == ConnectionState.Closed)
                        await con.OpenAsync();

                    DynamicParameters param = new();
                    param.Add("@Id", title.Id);
                    param.Add("@TenantId", title.TenantId);
                    param.Add("@CompanyId", title.CompanyId);
                    param.Add("@Code", title.Code);
                    param.Add("@Name", title.Name);
                    param.Add("@Description", title.Description);
                    param.Add("@IsActive", title.IsActive);
                    param.Add("@IsDelete", title.IsDelete);
                    param.Add("@ConcurrencyStamp", title.ConcurrencyStamp);
                    param.Add("@CreateTime", title.CreateTime);
                    param.Add("@CreatorId", title.CreatorId);
                    param.Add("@CreatorFullName", title.CreatorFullName);
                    if (title.LastUpdate != null && title.LastUpdate != DateTime.MinValue)
                    {
                        param.Add("@LastUpdate", title.LastUpdate);
                    }
                    param.Add("@LastUpdateUserId", title.LastUpdateUserId);
                    param.Add("@LastUpdateFullName", title.LastUpdateFullName);

                    if (title.DeleteTime != null && title.DeleteTime != DateTime.MinValue)
                    {
                        param.Add("@DeleteTime", title.DeleteTime);
                    }
                    param.Add("@DeleteUserId", title.DeleteUserId);
                    param.Add("@DeleteFullName", title.DeleteFullName);
                    rowAffected = await con.ExecuteAsync("[dbo].[spTitle_Insert]", param, commandType: CommandType.StoredProcedure);
                }
                return rowAffected;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[dbo].[spTitle_Insert] InsertAsync TitleRepository Error.");
                return -1;
            }
        }

        public async Task<int> UpdateAsync(Title title)
        {
            try
            {
                int rowAffected = 0;
                using (SqlConnection con = new(_connectionString))
                {
                    if (con.State == ConnectionState.Closed)
                        await con.OpenAsync();

                    DynamicParameters param = new();
                    param.Add("@Id", title.Id);
                    param.Add("@TenantId", title.TenantId);
                    param.Add("@CompanyId", title.CompanyId);
                    param.Add("@Code", title.Code);
                    param.Add("@Name", title.Name);
                    param.Add("@Description", title.Description);
                    param.Add("@IsActive", title.IsActive);
                    param.Add("@IsDelete", title.IsDelete);
                    param.Add("@ConcurrencyStamp", title.ConcurrencyStamp);
                    param.Add("@CreateTime", title.CreateTime);
                    param.Add("@CreatorId", title.CreatorId);
                    param.Add("@CreatorFullName", title.CreatorFullName);
                    if (title.LastUpdate != null && title.LastUpdate != DateTime.MinValue)
                    {
                        param.Add("@LastUpdate", title.LastUpdate);
                    }
                    param.Add("@LastUpdateUserId", title.LastUpdateUserId);
                    param.Add("@LastUpdateFullName", title.LastUpdateFullName);

                    if (title.DeleteTime != null && title.DeleteTime != DateTime.MinValue)
                    {
                        param.Add("@DeleteTime", title.DeleteTime);
                    }
                    param.Add("@DeleteUserId", title.DeleteUserId);
                    param.Add("@DeleteFullName", title.DeleteFullName);
                    rowAffected = await con.ExecuteAsync("[dbo].[spTitle_Update]", param, commandType: CommandType.StoredProcedure);
                }
                return rowAffected;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[dbo].[spTitle_Update] UpdateAsync TitleRepository Error.");
                return -1;
            }
        }
        public async Task<int> DeleteAsync(Title title)
        {
            try
            {
                int rowAffected = 0;
                using (SqlConnection con = new(_connectionString))
                {
                    if (con.State == ConnectionState.Closed)
                        await con.OpenAsync();

                    DynamicParameters param = new();
                    param.Add("@Id", title.Id);
                    param.Add("@DeleteUserId", title.DeleteUserId);
                    param.Add("@DeleteFullName", title.DeleteFullName);
                    rowAffected = await con.ExecuteAsync("[dbo].[spTitle_DeleteByID]", param, commandType: CommandType.StoredProcedure);
                }
                return rowAffected;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[dbo].[spTitle_DeleteByID] DeleteAsync TitleRepository Error.");
                return -1;
            }
        }

        public async Task<int> ForceDeleteAsync(string companyId, string id)
        {
            try
            {
                int rowAffected = 0;
                using (SqlConnection con = new(_connectionString))
                {
                    if (con.State == ConnectionState.Closed)
                        await con.OpenAsync();

                    DynamicParameters param = new();
                    param.Add("@CompanyId", companyId);
                    param.Add("@Id", id);
                    rowAffected = await con.ExecuteAsync("[dbo].[spTitle_ForceDeleteByID]", param, commandType: CommandType.StoredProcedure);
                }
                return rowAffected;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[dbo].[spTitle_ForceDeleteByID] ForceDeleteAsync TitleRepository Error.");
                return -1;
            }
        }

        public async Task<Title> GetInfoAsync(string id)
        {
            try
            {
                using SqlConnection con = new(_connectionString);
                if (con.State == ConnectionState.Closed)
                    await con.OpenAsync();

                DynamicParameters param = new();
                param.Add("@Id", id);
                return await con.QuerySingleOrDefaultAsync<Title>("[dbo].[spTitle_SelectByID]", param, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[dbo].[spTitle_SelectByID] GetInfoAsync TitleRepository Error.");
                return null;
            }
        }

        public async Task<bool> CheckExistsNameAsync(string tenantId, string companyId, string name)
        {
            try
            {
                using SqlConnection con = new(_connectionString);
                if (con.State == ConnectionState.Closed)
                    await con.OpenAsync();

                var sql = @"
					SELECT IIF (EXISTS (SELECT 1 FROM Titles WHERE CompanyId = @CompanyId AND TenantId = @TenantId AND Name = @Name AND IsDelete = 0), 1, 0)";

                var result = await con.ExecuteScalarAsync<bool>(sql, new { CompanyId = companyId, TenantId = tenantId, Name = name });
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "CheckExistsNameAsync TitleRepository Error.");
                return false;
            }
        }

        public async Task<bool> CheckExistsCodeAsync(string tenantId, string companyId, string code, string id)
        {
            try
            {
                using SqlConnection con = new(_connectionString);
                if (con.State == ConnectionState.Closed)
                    await con.OpenAsync();

                var sql = @"
					SELECT IIF (EXISTS (SELECT 1 FROM Titles WHERE CompanyId = @CompanyId AND TenantId = @TenantId AND Code = @Code AND IsDelete = 0 and Id != @Id), 1, 0)";

                var result = await con.ExecuteScalarAsync<bool>(sql, new { CompanyId = companyId, TenantId = tenantId, Code = code, Id = id });
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "CheckExistsCodeAsync TitleRepository Error.");
                return false;
            }
        }

        public async Task<bool> CheckExistByNameAsync(string tenantId, string companyId, string id, string name)
        {
            try
            {
                using SqlConnection con = new(_connectionString);
                if (con.State == ConnectionState.Closed)
                    await con.OpenAsync();

                var sql = @"
					SELECT IIF (EXISTS (SELECT 1 FROM Titles WHERE Id != @Id AND CompanyId = @CompanyId AND TenantId = @TenantId AND Name = @Name AND IsDelete = 0), 1, 0)";

                var result = await con.ExecuteScalarAsync<bool>(sql, new { Id = id, CompanyId = companyId, TenantId = tenantId, Name = name });
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "CheckExistByNameAsync TitleRepository Error.");
                return false;
            }
        }

        public async Task<int> UpdateByTitleNameAsync(string tenantId, string companyId, string titleId, string titleName)
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
                    param.Add("@TitleId", titleId);
                    param.Add("@TitleName", titleName);

                    rowAffected = await con.ExecuteAsync("[dbo].[Update_By_TitleName]", param, commandType: CommandType.StoredProcedure);
                }
                return rowAffected;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[dbo].[Update_By_TitleName] UpdateByTitleNameAsync  TitleRepository Error.");
                return -1;
            }
        }

        public async Task<bool> CheckExistsAsync(string companyId, string id)
        {
            try
            {
                using SqlConnection con = new(_connectionString);
                if (con.State == ConnectionState.Closed)
                    await con.OpenAsync();

                var sql = @"
					SELECT IIF (EXISTS (SELECT 1 FROM Titles WHERE Id = @Id AND CompanyId = @CompanyId AND IsDelete = 0), 1, 0)";

                var result = await con.ExecuteScalarAsync<bool>(sql, new { Id = id, CompanyId = companyId });
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "CheckExistsAsync TitleRepository Error.");
                return false;
            }
        }

        public async Task<int> UpdateIsActive(string companyId, string id, bool isActive)
        {
            int rowAffected = 0;
            using (SqlConnection con = new(_connectionString))
            {
                if (con.State == ConnectionState.Closed)
                    await con.OpenAsync();
                DynamicParameters param = new();
                param.Add("@CompanyId", companyId);
                param.Add("@Id", id);
                param.Add("@IsActive", isActive);
                rowAffected = await con.ExecuteAsync("[dbo].[spTitle_Update_IsActive]", param, commandType: CommandType.StoredProcedure);
            }
            return rowAffected;
        }

        public async Task<string> GetCode(string id)
        {
            try
            {
                using SqlConnection con = new(_connectionString);
                if (con.State == ConnectionState.Closed)
                    await con.OpenAsync();

                var sql = @"
					SELECT Code FROM Titles WHERE Id=@Id AND IsDelete=0 AND IsActive=1";

                var result = await con.QueryFirstOrDefaultAsync<string>(sql, new { Id = id });
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetCode TitleRepository Error.");
                return null;
            }
        }
    }
}
