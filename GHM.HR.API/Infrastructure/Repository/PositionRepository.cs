using Dapper;
using GHM.HR.API.Infrastructure.Data;
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
        private readonly IDbSession _dbSession;

        public PositionRepository(string connectionString, ILogger<PositionRepository> logger, IDbSession dbSession)
        {
            _connectionString = connectionString;
            _logger = logger;
            _dbSession = dbSession;
        }

        public async Task<List<PositionSearchViewModel>> SelectAllAsync(string tenantId, string companyId)
        {
            var con = await _dbSession.GetConnectionAsync();
            DynamicParameters param = new();
            param.Add("@TenantId", tenantId);
            param.Add("@CompanyId", companyId);
            var results = await con.QueryAsync<PositionSearchViewModel>("[dbo].[spPosition_SelectAll]", param, transaction: _dbSession.Transaction, commandType: CommandType.StoredProcedure);
            return results.ToList();
        }

        public async Task<List<PositionSearchViewModel>> SelectAllPositionActiveAsync(string tenantId, string companyId)
        {
            var con = await _dbSession.GetConnectionAsync();
            DynamicParameters param = new();
            param.Add("@TenantId", tenantId);
            param.Add("@CompanyId", companyId);
            var results = await con.QueryAsync<PositionSearchViewModel>("[dbo].[spPosition_SelectAll_Active]", param, transaction: _dbSession.Transaction, commandType: CommandType.StoredProcedure);
            return results.ToList();
        }

        public async Task<int> InsertAsync(Position position)
        {
            var con = await _dbSession.GetConnectionAsync();
            int rowAffected = 0;
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
            rowAffected = await con.ExecuteAsync("[dbo].[spPosition_Insert]", param, transaction: _dbSession.Transaction, commandType: CommandType.StoredProcedure);
            return rowAffected;
        }

        public async Task<int> UpdateAsync(Position position)
        {
            var con = await _dbSession.GetConnectionAsync();
            int rowAffected = 0;
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
            rowAffected = await con.ExecuteAsync("[dbo].[spPosition_Update]", param, transaction: _dbSession.Transaction, commandType: CommandType.StoredProcedure);
            return rowAffected;
        }

        public async Task<Position> GetInfoAsync(string id)
        {
            var con = await _dbSession.GetConnectionAsync();
            DynamicParameters param = new();
            param.Add("@Id", id);
            return await con.QuerySingleOrDefaultAsync<Position>("[dbo].[spPosition_SelectByID]", param, transaction: _dbSession.Transaction, commandType: CommandType.StoredProcedure);
        }

        public async Task<int> DeleteAsync(Position position)
        {
            var con = await _dbSession.GetConnectionAsync();
            int rowAffected = 0;
            DynamicParameters param = new();
            param.Add("@Id", position.Id);
            param.Add("@DeleteUserId", position.DeleteUserId);
            param.Add("@DeleteFullName", position.DeleteFullName);
            rowAffected = await con.ExecuteAsync("[dbo].[spPosition_DeleteByID]", param, transaction: _dbSession.Transaction, commandType: CommandType.StoredProcedure);
            return rowAffected;
        }

        public async Task<bool> CheckExistCodeAsync(string tenantId, string companyId, string code, string id)
        {
            var con = await _dbSession.GetConnectionAsync();
            var sql = @"
					SELECT IIF (EXISTS (SELECT 1 FROM Positions WHERE TenantId = @TenantId AND Code = @Code AND IsDelete = 0 AND Id != @Id AND (IsMultiple = 1 OR CompanyId = @CompanyId)), 1, 0)";

            var result = await con.ExecuteScalarAsync<bool>(sql, new { TenantId = tenantId, Code = code, Id = id, CompanyId = companyId });
            return result;
        }

        public async Task<int> ForceDeleteAsync(string companyId, string id)
        {
            var con = await _dbSession.GetConnectionAsync();
            int rowAffected = 0;
            DynamicParameters param = new();
            param.Add("@CompanyId", companyId);
            param.Add("@Id", id);
            rowAffected = await con.ExecuteAsync("[dbo].[spPosition_ForceDeleteByID]", param, transaction: _dbSession.Transaction, commandType: CommandType.StoredProcedure);
            return rowAffected;
        }

        public async Task<bool> CheckExistNameAsync(string tenantId, string companyId, string name)
        {
            var con = await _dbSession.GetConnectionAsync();
            var sql = @"
					SELECT IIF (EXISTS (SELECT 1 FROM Positions WHERE TenantId = @TenantId AND Name = @Name AND IsDelete = 0 AND (IsMultiple = 1 OR CompanyId = @CompanyId)), 1, 0)";

            var result = await con.ExecuteScalarAsync<bool>(sql, new { TenantId = tenantId, Name = name, CompanyId = companyId });
            return result;
        }

        public async Task<bool> CheckExistByNameAsync(string tenantId, string companyId, string id, string name)
        {
            var con = await _dbSession.GetConnectionAsync();
            var sql = @"
					SELECT IIF (EXISTS (SELECT 1 FROM Positions WHERE TenantId = @TenantId AND Name = @Name AND IsDelete = 0 AND Id != @Id AND (IsMultiple = 1 OR CompanyId = @CompanyId)), 1, 0)";

            var result = await con.ExecuteScalarAsync<bool>(sql, new { TenantId = tenantId, Name = name, CompanyId = companyId, Id = id });
            return result;
        }

        public async Task<int> UpdateByPositionNameAsync(string tenantId, string companyId, string positionId, string positionName)
        {
            var con = await _dbSession.GetConnectionAsync();
            int rowAffected = 0;
            DynamicParameters param = new();
            param.Add("@TenantId", tenantId);
            param.Add("@CompanyId", companyId);
            param.Add("@PositionId", positionId);
            param.Add("@PositionName", positionName);

            rowAffected = await con.ExecuteAsync("[dbo].[Update_By_PositionName]", param, transaction: _dbSession.Transaction, commandType: CommandType.StoredProcedure);
            return rowAffected;
        }

        public async Task<bool> CheckExistsAsync(string companyId, string id)
        {
            var con = await _dbSession.GetConnectionAsync();
            var sql = @"
					SELECT IIF (EXISTS (SELECT 1 FROM Positions WHERE Id = @Id AND CompanyId = @CompanyId AND IsDelete = 0), 1, 0)";

            var result = await con.ExecuteScalarAsync<bool>(sql, new { Id = id });
            return result;
        }

        public async Task<int> UpdateIsActive(string companyId, string id, bool isActive)
        {
            var con = await _dbSession.GetConnectionAsync();
            int rowAffected = 0;
            DynamicParameters param = new();
            param.Add("@CompanyId", companyId);
            param.Add("@Id", id);
            param.Add("@IsActive", isActive);
            rowAffected = await con.ExecuteAsync("[dbo].[spPosition_Update_IsActive]", param, transaction: _dbSession.Transaction, commandType: CommandType.StoredProcedure);
            return rowAffected;
        }
    }
}
