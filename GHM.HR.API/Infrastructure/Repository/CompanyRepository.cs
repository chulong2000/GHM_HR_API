// ----------------------------------------------o0o----------------------------------------------
// Copyright (c) Ghmsoft 2025. All rights reserved.
// Licensed under the Ghmsoft.vn License, Version 1.0. See LICENSE in the project root for license information.
// Create by : TruongTV
// Create date : 16/01/2025 15:03:03
// Description :
// Output :
// Modify :
// Project : HR
// ----------------------------------------------o0o----------------------------------------------

using Dapper;
using GHM.HR.Domain.IRepository;
using GHM.HR.Domain.Models;
using GHM.HR.Domain.ViewModels;
using GHM.Infrastructure.ViewModels;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
namespace GHM.HR.Infrastructure.Repository
{
	public class CompanyRepository : ICompanyRepository
	{
		private readonly string _connectionString;
		private readonly ILogger<CompanyRepository> _logger;
        private readonly IMemoryCache _cache;

        public CompanyRepository(string connectionString, ILogger<CompanyRepository> logger, IMemoryCache cache)
		{
			_connectionString = connectionString;
			_logger = logger;
			_cache = cache;
        }

		public async Task<List<CompanysSearchViewModel>> SelectAllAsync(string tenantId)
		{
			try
			{
				using SqlConnection con = new(_connectionString);
				if (con.State == ConnectionState.Closed)
					await con.OpenAsync();

				DynamicParameters param = new();
				param.Add("@TenantId", tenantId);
				var results = await con.QueryAsync<CompanysSearchViewModel>("[dbo].[spCompany_SelectAll]", param, commandType: CommandType.StoredProcedure);
				return results.ToList();
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "[dbo].[spCompany_SelectAll] SelectAllAsync CompanyRepository Error.");
				return [];
			}
		}


		public async Task<int> InsertAsync(Company company)
		{
			try
			{
				int rowAffected = 0;
				using (SqlConnection con = new(_connectionString))
				{
					if (con.State == ConnectionState.Closed)
						await con.OpenAsync();

					DynamicParameters param = new();
					param.Add("@Id", company.Id);
					param.Add("@TenantId", company.TenantId);
					param.Add("@Code", company.Code);
					param.Add("@Name", company.Name);
					param.Add("@PhoneNumber", company.PhoneNumber);
					param.Add("@Address", company.Address);
					param.Add("@Description", company.Description);
					param.Add("@TaxCode", company.TaxCode);
					param.Add("@IsActive", company.IsActive);
					param.Add("@ConcurrencyStamp", company.ConcurrencyStamp);
					param.Add("@CreateTime", company.CreateTime);
					param.Add("@CreatorId", company.CreatorId);
					param.Add("@CreatorFullName", company.CreatorFullName);
					if (company.LastUpdate != null && company.LastUpdate != DateTime.MinValue)
					{
					param.Add("@LastUpdate", company.LastUpdate);
					}
					param.Add("@LastUpdateUserId", company.LastUpdateUserId);
					param.Add("@LastUpdateFullName", company.LastUpdateFullName);
					param.Add("@IsDelete", company.IsDelete);
					if (company.DeleteTime != null && company.DeleteTime != DateTime.MinValue)
					{
					param.Add("@DeleteTime", company.DeleteTime);
					}
					param.Add("@DeleteUserId", company.DeleteUserId);
					param.Add("@DeleteFullName", company.DeleteFullName);
					param.Add("@Logo", company.Logo);
					param.Add("@LogoFooter", company.LogoFooter);
					param.Add("@AppIds", company.AppIds);
					rowAffected = await con.ExecuteAsync("[dbo].[spCompany_Insert]", param, commandType: CommandType.StoredProcedure);
				}
				return rowAffected;
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "[dbo].[spCompany_Insert] InsertAsync CompanyRepository Error.");
				return -1;
			}
		}


		public async Task<int> UpdateAsync(Company company)
		{
			try
			{
				int rowAffected = 0;
				using (SqlConnection con = new(_connectionString))
				{
					if (con.State == ConnectionState.Closed)
						await con.OpenAsync();

					DynamicParameters param = new();
					param.Add("@Id", company.Id);
					param.Add("@TenantId", company.TenantId);
					param.Add("@Code", company.Code);
					param.Add("@Name", company.Name);
					param.Add("@PhoneNumber", company.PhoneNumber);
					param.Add("@Address", company.Address);
					param.Add("@Description", company.Description);
					param.Add("@TaxCode", company.TaxCode);
					param.Add("@IsActive", company.IsActive);
					param.Add("@ConcurrencyStamp", company.ConcurrencyStamp);
					param.Add("@CreateTime", company.CreateTime);
					param.Add("@CreatorId", company.CreatorId);
					param.Add("@CreatorFullName", company.CreatorFullName);
					if (company.LastUpdate != null && company.LastUpdate != DateTime.MinValue)
					{
					param.Add("@LastUpdate", company.LastUpdate);
					}
					param.Add("@LastUpdateUserId", company.LastUpdateUserId);
					param.Add("@LastUpdateFullName", company.LastUpdateFullName);
					param.Add("@IsDelete", company.IsDelete);
					if (company.DeleteTime != null && company.DeleteTime != DateTime.MinValue)
					{
					param.Add("@DeleteTime", company.DeleteTime);
					}
					param.Add("@DeleteUserId", company.DeleteUserId);
					param.Add("@DeleteFullName", company.DeleteFullName);
                    param.Add("@Logo", company.Logo);
                    param.Add("@LogoFooter", company.LogoFooter);
                    param.Add("@AppIds", company.AppIds);
                    rowAffected = await con.ExecuteAsync("[dbo].[spCompany_Update]", param, commandType: CommandType.StoredProcedure);
				}
				return rowAffected;
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "[dbo].[spCompany_Update] UpdateAsync CompanyRepository Error.");
				return -1;
			}
		}


		public async Task<int> DeleteAsync(Company company)
		{
			try
			{
				int rowAffected = 0;
				using (SqlConnection con = new(_connectionString))
				{
					if (con.State == ConnectionState.Closed)
						await con.OpenAsync();

					DynamicParameters param = new();
					param.Add("@Id",company.Id);
					param.Add("@DeleteUserId", company.DeleteUserId);
					param.Add("@DeleteFullName", company.DeleteFullName);
					rowAffected = await con.ExecuteAsync("[dbo].[spCompany_DeleteByID]", param, commandType: CommandType.StoredProcedure);
				}
				return rowAffected;
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "[dbo].[spCompany_DeleteByID] DeleteAsync CompanyRepository Error.");
				return -1;
			}
		}


		public async Task<int> ForceDeleteAsync(string id)
		{
			try
			{
				int rowAffected = 0;
				using (SqlConnection con = new(_connectionString))
				{
					if (con.State == ConnectionState.Closed)
						await con.OpenAsync();

					DynamicParameters param = new();
					param.Add("@Id",id);
					rowAffected = await con.ExecuteAsync("[dbo].[spCompany_ForceDeleteByID]", param, commandType: CommandType.StoredProcedure);
				}
				return rowAffected;
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "[dbo].[spCompany_ForceDeleteByID] ForceDeleteAsync CompanyRepository Error.");
				return -1;
			}
		}


		public async Task<Company> GetInfoAsync(string id)
		{
			try
			{
				using SqlConnection con = new(_connectionString);
				if (con.State == ConnectionState.Closed)
					await con.OpenAsync();

				DynamicParameters param = new();
				param.Add("@Id", id);
				return await con.QuerySingleOrDefaultAsync<Company>("[dbo].[spCompany_SelectByID]", param, commandType: CommandType.StoredProcedure);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "[dbo].[spCompany_SelectByID] GetInfoAsync CompanyRepository Error.");
				return null;
			}
		}


		public async Task<Company> GetInfoAsync(string tenantId, string id)
		{
			try
			{
				using SqlConnection con = new(_connectionString);
				if (con.State == ConnectionState.Closed)
					await con.OpenAsync();

				DynamicParameters param = new();
				param.Add("@Id", id);
				param.Add("@TenantId", tenantId);
				return await con.QuerySingleOrDefaultAsync<Company>("[dbo].[spCompany_SelectByIDTenantId]", param, commandType: CommandType.StoredProcedure);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "[dbo].[spCompany_SelectByIDTenantId] GetInfoAsync CompanyRepository Error.");
				return null;
			}
		}


		public async Task<bool> CheckExistsAsync(string id)
		{
			try
			{
				using SqlConnection con = new(_connectionString);
				if (con.State == ConnectionState.Closed)
					await con.OpenAsync();

				var sql = @"
				SELECT IIF (EXISTS (SELECT 1 FROM Companys WHERE Id = @Id AND IsDelete = 0), 1, 0)";

				var result = await con.ExecuteScalarAsync<bool>(sql, new {Id = id});
				return result;
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "CheckExistsAsync CompanyRepository Error.");
				return false;
			}
		}


		public async Task<bool> CheckExistsByTenantIdAsync(string tenantId,string id)
		{
			try
			{
				using SqlConnection con = new(_connectionString);
				if (con.State == ConnectionState.Closed)
					await con.OpenAsync();

				var sql = @"
				SELECT IIF (EXISTS (SELECT 1 FROM Companys WHERE Id = @Id AND TenantId = @TenantId AND IsDelete = 0), 1, 0)";

				var result = await con.ExecuteScalarAsync<bool>(sql, new {Id = id, TenantId = tenantId});
				return result;
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "CheckExistsByTenantIdAsync CompanyRepository Error.");
				return false;
			}
		}


		public async Task<bool> CheckExistsNameAsync(string tenantId,string id,string name)
		{
			try
			{
				using SqlConnection con = new(_connectionString);
				if (con.State == ConnectionState.Closed)
					await con.OpenAsync();

				var sql = @"
				SELECT IIF (EXISTS (SELECT 1 FROM Companys WHERE Id != @Id AND TenantId = @TenantId AND Name = @Name AND IsDelete = 0), 1, 0)";

				var result = await con.ExecuteScalarAsync<bool>(sql, new {Id = id, TenantId = tenantId, Name = name});
				return result;
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "CheckExistsNameAsync CompanyRepository Error.");
				return false;
			}
		}

        public async Task<bool> CheckExistsCodeAsync(string tenantId, string id, string code)
        {
            try
            {
                using SqlConnection con = new(_connectionString);
                if (con.State == ConnectionState.Closed)
                    await con.OpenAsync();

                var sql = @"
				SELECT IIF (EXISTS (SELECT 1 FROM Companys WHERE Id != @Id AND TenantId = @TenantId AND Code = @Code AND IsDelete = 0), 1, 0)";

                var result = await con.ExecuteScalarAsync<bool>(sql, new { Id = id, TenantId = tenantId, Code = code });
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "CheckExistsCodeAsync CompanyRepository Error.");
                return false;
            }
        }

        public async Task<Company> GetInfoByCodeAsync(string tenantId, string code)
        {
            try
            {
                using SqlConnection con = new(_connectionString);
                if (con.State == ConnectionState.Closed)
                    await con.OpenAsync();

                var sql = @"
				SELECT TOP 1 * FROM Companys WHERE TenantId = @TenantId AND Code = @Code AND IsDelete = 0";

                return await con.QueryFirstOrDefaultAsync<Company>(sql, new { TenantId = tenantId, Code = code });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetInfoByCodeAsync CompanyRepository Error.");
                return null;
            }
        }

        public async Task<bool> CheckExistUserAsync(string tenantId, string companyId)
        {
            try
            {
                using SqlConnection con = new(_connectionString);
                if (con.State == ConnectionState.Closed)
                    await con.OpenAsync();

                DynamicParameters param = new();
                param.Add("@TenantId", tenantId);
                param.Add("@CompanyId", companyId);

                var result = await con.ExecuteScalarAsync<bool>("[dbo].[spCompany_CheckExistUser]", param, commandType: CommandType.StoredProcedure);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[dbo].[spCompany_CheckExistUser] CheckExistUser CompanyRepository Error.");
                return false;
            }
        }

        public async Task<List<CompanyUserViewModel>> SelectAllByUserAsync(string tenantId, string userId)
        {
            try
            {
                using SqlConnection con = new(_connectionString);
                if (con.State == ConnectionState.Closed)
                    await con.OpenAsync();

                DynamicParameters param = new();
                param.Add("@TenantId", tenantId);
                param.Add("@UserId", userId);
                var results = await con.QueryAsync<CompanyUserViewModel>("[dbo].[spCompany_SelectAllByUser]", param, commandType: CommandType.StoredProcedure);
                return results.ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[dbo].[spCompany_SelectAllByUser] SelectAllByUserAsync CompanyRepository Error.");
                return [];
            }
        }

        public async Task<string> GetCodeCompanyAsync(string companyId)
        {
            try
            {
                using SqlConnection con = new(_connectionString);
                if (con.State == ConnectionState.Closed)
                    await con.OpenAsync();

                DynamicParameters param = new();
                param.Add("@CompanyId", companyId);
                return await con.ExecuteScalarAsync<string>("[dbo].[spCompany_Code]", param, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetCodeCompany UserRepository Error.");
                return string.Empty;
            }
        }

        public async Task<CompanyLogoViewModel> GetLogoAsync(string tenantId,string companyId)
        {
            try
            {
                using SqlConnection con = new(_connectionString);
                if (con.State == ConnectionState.Closed)
                    await con.OpenAsync();

                DynamicParameters param = new();
                param.Add("@TenantId", tenantId);
                param.Add("@CompanyId", companyId);
                return await con.QueryFirstOrDefaultAsync<CompanyLogoViewModel>("[dbo].[spCompany_GetLogo]", param, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[dbo].[spCompany_GetLogo] CompanyRepository Error.");
                return null;
            }
        }
        public async Task<string> GetConnectionAsync(string companyId)
        {
            var cacheKey = "ConnectionString:" + companyId;
            try
            {
                if (_cache.TryGetValue(cacheKey, out string connStr))
                    return connStr;

                using SqlConnection con = new(_connectionString); // metadata DB
                await con.OpenAsync();

                var param = new DynamicParameters();
                param.Add("@CompanyId", companyId);

                var result = await con.QuerySingleOrDefaultAsync<string>(
                    "[dbo].[spCompany_GetConnectionString]", param, commandType: CommandType.StoredProcedure);

                _cache.Set(cacheKey, result ?? "",
                    string.IsNullOrWhiteSpace(result) ? TimeSpan.FromSeconds(1) : TimeSpan.FromDays(1));

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[spCompany_GetConnectionString] GetConnectionAsync Error for company: {Company}", companyId);
                return null!;
            }
        }
    }
}
