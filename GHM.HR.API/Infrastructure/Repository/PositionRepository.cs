using Dapper;
using GHM.HR.Domain.IRepository;
using GHM.HR.Domain.Models;
using GHM.HR.Domain.ViewModels;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GHM.HR.Infrastructure.Repository
{
    public class PositionRepository : IPositionRepository
    {
        private readonly string _connectionString;
        private readonly ILogger<PositionRepository> _logger;

        public PositionRepository(string connectionString, ILogger<PositionRepository> logger)
        {
            _connectionString = connectionString;
            _logger = logger;
        }

        public async Task<List<PositionSearchViewModel>> SelectAllAsync(string tenantId, string companyId)
        {
            try
            {
                using SqlConnection con = new(_connectionString);
                if (con.State == ConnectionState.Closed)
                    await con.OpenAsync();

                DynamicParameters param = new();
                param.Add("@TenantId", tenantId);
                param.Add("@CompanyId", companyId);
                var results = await con.QueryAsync<PositionSearchViewModel>("[dbo].[spPosition_SelectAll]", param, commandType: CommandType.StoredProcedure);
                return results.ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[dbo].[spPosition_SelectAll] SelectAllAsync PositionRepository Error.");
                return new List<PositionSearchViewModel>();
            }
        }

        public async Task<List<PositionSearchViewModel>> SelectAllPositionActiveAsync(string tenantId, string companyId)
        {
            try
            {
                using SqlConnection con = new(_connectionString);
                if (con.State == ConnectionState.Closed)
                    await con.OpenAsync();

                DynamicParameters param = new();
                param.Add("@TenantId", tenantId);
                param.Add("@CompanyId", companyId);
                var results = await con.QueryAsync<PositionSearchViewModel>("[dbo].[spPosition_SelectAll_Active]", param, commandType: CommandType.StoredProcedure);
                return results.ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[dbo].[spPosition_SelectAll_Active] SelectAllActiveAsync PositionRepository Error.");
                return new List<PositionSearchViewModel>();
            }
        }

        public async Task<int> InsertAsync(Position position)
        {
            try
            {
                int rowAffected = 0;
                using (SqlConnection con = new(_connectionString))
                {
                    if (con.State == ConnectionState.Closed)
                        await con.OpenAsync();

                    DynamicParameters param = new();
                    param.Add("@Id", position.Id);
                    param.Add("@TenantId", position.TenantId);
                    param.Add("@CompanyId", position.CompanyId);
                    param.Add("@Code", position.Code);
                    param.Add("@Name", position.Name);
                    param.Add("@Description", position.Description);
                    param.Add("@IsMultiple", position.IsMultiple);
                    param.Add("@IsActive", position.IsActive);
                    param.Add("@IsDelete", position.IsDelete);
                    param.Add("@ConcurrencyStamp", position.ConcurrencyStamp);
                    param.Add("@CreateTime", position.CreateTime);
                    param.Add("@CreatorId", position.CreatorId);
                    param.Add("@CreatorFullName", position.CreatorFullName);
                    if (position.LastUpdate != null && position.LastUpdate != DateTime.MinValue)
                    {
                        param.Add("@LastUpdate", position.LastUpdate);
                    }
                    param.Add("@LastUpdatedUserId", position.LastUpdatedUserId);
                    param.Add("@LastUpdateFullName", position.LastUpdateFullName);
                    if (position.DeleteTime != null && position.DeleteTime != DateTime.MinValue)
                    {
                        param.Add("@DeleteTime", position.DeleteTime);
                    }
                    param.Add("@DeleteUserId", position.DeleteUserId);
                    param.Add("@DeleteFullName", position.DeleteFullName);
                    rowAffected = await con.ExecuteAsync("[dbo].[spPosition_Insert]", param, commandType: CommandType.StoredProcedure);
                }
                return rowAffected;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[dbo].[spPosition_Insert] InsertAsync PositionRepository Error.");
                return -1;
            }
        }

        public async Task<int> UpdateAsync(Position position)
        {
            try
            {
                int rowAffected = 0;
                using (SqlConnection con = new(_connectionString))
                {
                    if (con.State == ConnectionState.Closed)
                        await con.OpenAsync();

                    DynamicParameters param = new();
                    param.Add("@Id", position.Id);
                    param.Add("@TenantId", position.TenantId);
                    param.Add("@CompanyId", position.CompanyId);
                    param.Add("@Code", position.Code);
                    param.Add("@Name", position.Name);
                    param.Add("@Description", position.Description);
                    param.Add("@IsMultiple", position.IsMultiple);
                    param.Add("@IsActive", position.IsActive);
                    param.Add("@IsDelete", position.IsDelete);
                    param.Add("@ConcurrencyStamp", position.ConcurrencyStamp);
                    if (position.CreateTime != null && position.CreateTime != DateTime.MinValue)
                    {
                        param.Add("@CreateTime", position.CreateTime);
                    }
                    param.Add("@CreatorId", position.CreatorId);
                    param.Add("@CreatorFullName", position.CreatorFullName);
                    if (position.LastUpdate != null && position.LastUpdate != DateTime.MinValue)
                    {
                        param.Add("@LastUpdate", position.LastUpdate);
                    }
                    param.Add("@LastUpdatedUserId", position.LastUpdatedUserId);
                    param.Add("@LastUpdateFullName", position.LastUpdateFullName);
                    if (position.DeleteTime != null && position.DeleteTime != DateTime.MinValue)
                    {
                        param.Add("@DeleteTime", position.DeleteTime);
                    }
                    param.Add("@DeleteUserId", position.DeleteUserId);
                    param.Add("@DeleteFullName", position.DeleteFullName);
                    rowAffected = await con.ExecuteAsync("[dbo].[spPosition_Update]", param, commandType: CommandType.StoredProcedure);
                }
                return rowAffected;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[dbo].[spPosition_Update] UpdateAsync PositionRepository Error.");
                return -1;
            }
        }

        public async Task<Position> GetInfoAsync(string id)
        {
            try
            {
                using SqlConnection con = new(_connectionString);
                if (con.State == ConnectionState.Closed)
                    await con.OpenAsync();

                DynamicParameters param = new();
                param.Add("@Id", id);
                return await con.QuerySingleOrDefaultAsync<Position>("[dbo].[spPosition_SelectByID]", param, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[dbo].[spPosition_SelectByID] GetInfoAsync PositionRepository Error.");
                return null;
            }
        }

        public async Task<int> DeleteAsync(Position position)
        {
            try
            {
                int rowAffected = 0;
                using (SqlConnection con = new(_connectionString))
                {
                    if (con.State == ConnectionState.Closed)
                        await con.OpenAsync();

                    DynamicParameters param = new();
                    param.Add("@Id", position.Id);
                    param.Add("@DeleteUserId", position.DeleteUserId);
                    param.Add("@DeleteFullName", position.DeleteFullName);
                    rowAffected = await con.ExecuteAsync("[dbo].[spPosition_DeleteByID]", param, commandType: CommandType.StoredProcedure);
                }
                return rowAffected;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[dbo].[spPosition_DeleteByID] DeleteAsync PositionRepository Error.");
                return -1;
            }
        }

        public async Task<bool> CheckExistCodeAsync(string tenantId, string companyId, string code, string id)
        {
            try
            {
                using SqlConnection con = new(_connectionString);
                if (con.State == ConnectionState.Closed)
                    await con.OpenAsync();

                var sql = @"
					SELECT IIF (EXISTS (SELECT 1 FROM Positions WHERE TenantId = @TenantId AND Code = @Code AND IsDelete = 0 AND Id != @Id AND (IsMultiple = 1 OR CompanyId = @CompanyId)), 1, 0)";

                var result = await con.ExecuteScalarAsync<bool>(sql, new { TenantId = tenantId, Code = code, Id = id,CompanyId = companyId });
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "CheckExistsCodeAsync PositionRepository Error.");
                return false;
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
                    rowAffected = await con.ExecuteAsync("[dbo].[spPosition_ForceDeleteByID]", param, commandType: CommandType.StoredProcedure);
                }
                return rowAffected;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[dbo].[spPosition_ForceDeleteByID] ForceDeleteAsync PositionRepository Error.");
                return -1;
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
					SELECT IIF (EXISTS (SELECT 1 FROM Positions WHERE TenantId = @TenantId AND Name = @Name AND IsDelete = 0 AND (IsMultiple = 1 OR CompanyId = @CompanyId)), 1, 0)";

                var result = await con.ExecuteScalarAsync<bool>(sql, new { TenantId = tenantId, Name = name, CompanyId = companyId });
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "CheckExistsNameAsync PositionRepository Error.");
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
					SELECT IIF (EXISTS (SELECT 1 FROM Positions WHERE TenantId = @TenantId AND Name = @Name AND IsDelete = 0 AND Id != @Id AND (IsMultiple = 1 OR CompanyId = @CompanyId)), 1, 0)";

                var result = await con.ExecuteScalarAsync<bool>(sql, new { TenantId = tenantId, Name = name, CompanyId = companyId, Id = id });
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "CheckExistByNameAsync PositionRepository Error.");
                return false;
            }
        }

        public async Task<int> UpdateByPositionNameAsync(string tenantId,string companyId,string positionId, string positionName)
        {
            try
            {
                int rowAffected = 0;
                using (SqlConnection con = new(_connectionString))
                {
                    if (con.State == ConnectionState.Closed)
                        await con.OpenAsync();

                    DynamicParameters param = new();
                    param.Add("@TenantId",tenantId);
                    param.Add("@CompanyId", companyId);
                    param.Add("@PositionId", positionId);
                    param.Add("@PositionName", positionName);

                    rowAffected = await con.ExecuteAsync("[dbo].[Update_By_PositionName]", param, commandType: CommandType.StoredProcedure);
                }
                return rowAffected;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[dbo].[Update_By_PositionName] UpdateByPositionNameAsync PositionRepository Error.");
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
					SELECT IIF (EXISTS (SELECT 1 FROM Positions WHERE Id = @Id AND CompanyId = @CompanyId AND IsDelete = 0), 1, 0)";

                var result = await con.ExecuteScalarAsync<bool>(sql, new { Id = id });
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "CheckExistsAsync PositionRepository Error.");
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
                rowAffected = await con.ExecuteAsync("[dbo].[spPosition_Update_IsActive]", param, commandType: CommandType.StoredProcedure);
            }
            return rowAffected;
        }
    }
}
