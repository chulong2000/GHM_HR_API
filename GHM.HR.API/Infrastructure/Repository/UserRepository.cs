using Dapper;
using GHM.HR.Domain.IRepository;
using GHM.HR.Domain.Models;
using GHM.HR.Domain.ViewModels;
using GHM.Infrastructure.ViewModels;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GHM.Infrastructure.Models;

namespace GHM.HR.Infrastructure.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly string _connectionString;
        private readonly ILogger<UserRepository> _logger;
        public UserRepository(string connectionString, ILogger<UserRepository> logger)
        {
            _connectionString = connectionString;
            _logger = logger;
        }

        public async Task<List<UserSearchViewModel>> SelectAllAsync(string tenantId, string companyId, DateTime? contractExpirationDate)
        {
            try
            {
                using SqlConnection con = new(_connectionString);
                if (con.State == ConnectionState.Closed)
                    await con.OpenAsync();

                DynamicParameters param = new();
                param.Add("@TenantId", tenantId);
                param.Add("@CompanyId", companyId);
                param.Add("@ContractExpirationDate", contractExpirationDate);
                var results = await con.QueryAsync<UserSearchViewModel>("[dbo].[spUser_SelectAll]", param, commandType: CommandType.StoredProcedure);
                return results.ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[dbo].[spUser_SelectAll] SelectAllAsync UserRepository Error.");
                return new List<UserSearchViewModel>();
            }
        }

        public async Task<List<UserSearchAllViewModel>> SelectAllUsersAsync(string tenantId, string companyId)
        {
            try
            {
                using SqlConnection con = new(_connectionString);
                if (con.State == ConnectionState.Closed)
                    await con.OpenAsync();

                DynamicParameters param = new();
                param.Add("@TenantId", tenantId);
                param.Add("@CompanyId", companyId);
                var results = await con.QueryAsync<UserSearchAllViewModel>("[dbo].[spUser_SelectAllUsers]", param, commandType: CommandType.StoredProcedure);
                return results.ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[dbo].[spUser_SelectAllUsers] SelectAllUsersAsync UserRepository Error.");
                return new List<UserSearchAllViewModel>();
            }
        }

        public async Task<List<UserBirthdayViewModel>> SearchUserBirthdayAsync(string tenantId, string companyId, int? month)
        {
            try
            {
                using SqlConnection con = new(_connectionString);
                if (con.State == ConnectionState.Closed)
                    await con.OpenAsync();

                DynamicParameters param = new();
                param.Add("@TenantId", tenantId);
                param.Add("@CompanyId", companyId);
                param.Add("@Month", month);

                var results = await con.QueryAsync<UserBirthdayViewModel>("[dbo].[spUser_SearchBirthday]", param, commandType: CommandType.StoredProcedure);
                return results.ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[dbo].[spUser_SearchBirthday] SearchBirthdayAsync UserRepository Error.");
                return new List<UserBirthdayViewModel>();
            }
        }

        public async Task<List<UserResignedViewModel>> SearchUserResignedAsync(string tenantId, string companyId, bool isAll, int month)
        {
            try
            {
                using SqlConnection con = new(_connectionString);
                if (con.State == ConnectionState.Closed)
                    await con.OpenAsync();

                DynamicParameters param = new();
                param.Add("@TenantId", tenantId);
                param.Add("@CompanyId", companyId);
                param.Add("@isAll", isAll);
                param.Add("@Month", month);

                var results = await con.QueryAsync<UserResignedViewModel>("[dbo].[spUser_GetResigned]", param, commandType: CommandType.StoredProcedure);
                return results.ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[dbo].[spUser_GetResigned] SearchUserResignedAsync UserRepository Error.");
                return [];
            }
        }

        public async Task<List<UserSearchViewModel>> SelectAllUsersByCompanyDepartmentAsync(string tenantId, string companyId, int departmentId)
        {
            try
            {
                using SqlConnection con = new(_connectionString);
                if (con.State == ConnectionState.Closed)
                    await con.OpenAsync();

                DynamicParameters param = new();
                param.Add("@TenantId", tenantId);
                param.Add("@CompanyId", companyId);
                param.Add("@DepartmentId", departmentId);
                var results = await con.QueryAsync<UserSearchViewModel>("[dbo].[spUser_SelectAllByCompanyAndDepartmentAsync]", param, commandType: CommandType.StoredProcedure);
                return results.ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[dbo].[spUser_SelectAllByCompanyAndDepartmentAsync] SelectAllAsync UserRepository Error.");
                return new List<UserSearchViewModel>();
            }
        }

        public async Task<List<UserSearchViewModel>> SelectAllUsersByFineFormulaAsync(string tenantId, string companyId, string fineFormulaId)
        {
            try
            {
                using SqlConnection con = new(_connectionString);
                if (con.State == ConnectionState.Closed)
                    await con.OpenAsync();

                DynamicParameters param = new();
                param.Add("@TenantId", tenantId);
                param.Add("@CompanyId", companyId);
                param.Add("@FineFormulaId", fineFormulaId);
                var results = await con.QueryAsync<UserSearchViewModel>("[dbo].[spUser_SelectAllByFineFormulaAsync]", param, commandType: CommandType.StoredProcedure);
                return results.ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[dbo].[spUser_SelectAllByFineFormulaAsync] SelectAllAsync UserRepository Error.");
                return new List<UserSearchViewModel>();
            }
        }

        public async Task<int> InsertAsync(User users)
        {
            try
            {
                int rowAffected = 0;
                using (SqlConnection con = new(_connectionString))
                {
                    if (con.State == ConnectionState.Closed)
                        await con.OpenAsync();

                    DynamicParameters param = new();
                    param.Add("@Id", users.Id);
                    param.Add("@Code", users.Code);
                    param.Add("@TenantId", users.TenantId);
                    param.Add("@CompanyId", users.CompanyId);
                    param.Add("@FullName", users.FullName);
                    param.Add("@FirstName", users.FirstName);
                    param.Add("@MiddleName", users.MiddleName);
                    param.Add("@LastName", users.LastName);
                    param.Add("@Birthday", users.Birthday);
                    param.Add("@Avatar", users.Avatar);
                    param.Add("@Gender", users.Gender);
                    param.Add("@UserName", users.UserName);
                    param.Add("@CountryId", users.CountryId);
                    param.Add("@ProvinceId", users.ProvinceId);
                    param.Add("@DistrictId", users.DistrictId);
                    param.Add("@Address", users.Address);
                    param.Add("@PermanentAddress", users.PermanentAddress);
                    param.Add("@TemporaryAddress", users.TemporaryAddress);
                    param.Add("@NationId", users.NationId);
                    param.Add("@ReligionId", users.ReligionId);
                    param.Add("@MarriedStatus", users.MarriedStatus);
                    param.Add("@Status", users.Status);
                    param.Add("@Month", users.Month);
                    if (users.OfficalDate != null && users.OfficalDate != DateTime.MinValue)
                    {
                        param.Add("@OfficalDate", users.OfficalDate);
                    }
                    param.Add("@JoinedDate", users.JoinedDate);
                    if (users.OutDate != null && users.OutDate != DateTime.MinValue)
                    {
                        param.Add("@OutDate", users.OutDate);
                    }
                    param.Add("@DepartmentId", users.DepartmentId);
                    param.Add("@DepartmentPath", users.DepartmentPath);
                    param.Add("@DepartmentName", users.DepartmentName);
                    param.Add("@TitleId", users.TitleId);
                    param.Add("@TitleName", users.TitleName);
                    param.Add("@PositionId", users.PositionId);
                    param.Add("@PositionName", users.PositionName);
                    param.Add("@PhoneNumber", users.PhoneNumber);
                    param.Add("@Email", users.Email);
                    param.Add("@ManagerUserId", users.ManagerUserId);
                    param.Add("@ManagerFullName", users.ManagerFullName);
                    param.Add("@SuccessorUserId", users.SuccessorUserId);
                    param.Add("@SuccessorFullName", users.SuccessorFullName);
                    param.Add("@WorkingForm", users.WorkingForm);
                    param.Add("@TaxCode", users.TaxCode);
                    param.Add("@EnrollNumberTT", users.EnrollNumberTT);
                    param.Add("@EnrollNumberNeo", users.EnrollNumberNeo);
                    param.Add("@Note", users.Note);
                    param.Add("@ContractCode", users.ContractCode);
                    param.Add("@InsuranceCode", users.InsuranceCode);
                    param.Add("@InsuranceStatus", users.InsuranceStatus);
                    param.Add("@InsuranceName", users.InsuranceName);
                    param.Add("@IdCardNumber", users.IdCardNumber);
                    if (users.IdCardDateOfIssue != null && users.IdCardDateOfIssue != DateTime.MinValue)
                    {
                        param.Add("@IdCardDateOfIssue", users.IdCardDateOfIssue);
                    }
                    param.Add("@IdCardPlaceOfIssue", users.IdCardPlaceOfIssue);
                    param.Add("@IsActive", users.IsActive);
                    param.Add("@ConcurrencyStamp", users.ConcurrencyStamp);
                    param.Add("@CreateTime", users.CreateTime);
                    param.Add("@CreatorId", users.CreatorId);
                    param.Add("@CreatorFullName", users.CreatorFullName);
                    if (users.LastUpdate != null && users.LastUpdate != DateTime.MinValue)
                    {
                        param.Add("@LastUpdate", users.LastUpdate);
                    }
                    param.Add("@LastUpdateUserId", users.LastUpdateUserId);
                    param.Add("@LastUpdateFullName", users.LastUpdateFullName);
                    param.Add("@IsDelete", users.IsDelete);
                    if (users.DeleteTime != null && users.DeleteTime != DateTime.MinValue)
                    {
                        param.Add("@DeleteTime", users.DeleteTime);
                    }
                    param.Add("@DeleteUserId", users.DeleteUserId);
                    param.Add("@DeleteFullName", users.DeleteFullName);
                    param.Add("@AdvanceLeaveGranted", users.AdvanceLeaveGranted);
                    param.Add("@CompLeaveGranted", users.CompLeaveGranted);
                    if (users.ContractExpirationDate != null && users.ContractExpirationDate != DateTime.MinValue)
                    {
                        param.Add("@ContractExpirationDate", users.ContractExpirationDate);
                    }
                    param.Add("@PersonnelStatus", users.PersonnelStatus);
                    param.Add("@ExpireDays", users.ExpireDays);
                    rowAffected = await con.ExecuteAsync("[dbo].[spUser_Insert]", param, commandType: CommandType.StoredProcedure);
                }
                return rowAffected;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[dbo].[spUser_Insert] InsertAsync UserRepository Error.");
                return -1;
            }
        }

        // Bulk insert for the external-app sync. Streams users into the TVP
        // dbo.UserBulkInsertList and calls [dbo].[spUser_InsertBulk], which dedups
        // (within-batch + against existing) and returns the Ids actually inserted.
        // One connection, batched; on error logs and returns an empty list.
        public async Task<List<string>> InsertBulkAsync(List<User> users)
        {
            const int bulkBatchSize = 1000;
            const int bulkCommandTimeoutSeconds = 300;

            if (users == null || users.Count == 0)
                return new List<string>();

            try
            {
                var insertedIds = new List<string>();
                using SqlConnection con = new(_connectionString);
                if (con.State == ConnectionState.Closed)
                    await con.OpenAsync();

                for (var startIndex = 0; startIndex < users.Count; startIndex += bulkBatchSize)
                {
                    var batchCount = Math.Min(bulkBatchSize, users.Count - startIndex);
                    var table = BuildUserBulkTable(users, startIndex, batchCount);

                    DynamicParameters param = new();
                    param.Add("@Users", table.AsTableValuedParameter("dbo.UserBulkInsertList"));

                    var batchIds = await con.QueryAsync<string>(new CommandDefinition(
                        "[dbo].[spUser_InsertBulk]", param,
                        commandTimeout: bulkCommandTimeoutSeconds,
                        commandType: CommandType.StoredProcedure));

                    insertedIds.AddRange(batchIds);
                }

                return insertedIds;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[dbo].[spUser_InsertBulk] InsertBulkAsync UserRepository Error.");
                return new List<string>();
            }
        }

        // Builds a DataTable whose column ORDER matches dbo.UserBulkInsertList exactly
        // (SQL Server maps TVP columns by ordinal position). Conditional date columns
        // are passed as NULL when unset, mirroring [dbo].[spUser_Insert].
        private static DataTable BuildUserBulkTable(IReadOnlyList<User> users, int startIndex, int count)
        {
            var table = new DataTable();
            table.Columns.Add("SourceRowNumber", typeof(int));
            table.Columns.Add("Id", typeof(string));
            table.Columns.Add("Code", typeof(string));
            table.Columns.Add("TenantId", typeof(string));
            table.Columns.Add("CompanyId", typeof(string));
            table.Columns.Add("FullName", typeof(string));
            table.Columns.Add("FirstName", typeof(string));
            table.Columns.Add("MiddleName", typeof(string));
            table.Columns.Add("LastName", typeof(string));
            table.Columns.Add("Birthday", typeof(DateTime));
            table.Columns.Add("Avatar", typeof(string));
            table.Columns.Add("Gender", typeof(int));
            table.Columns.Add("UserName", typeof(string));
            table.Columns.Add("CountryId", typeof(string));
            table.Columns.Add("ProvinceId", typeof(string));
            table.Columns.Add("DistrictId", typeof(string));
            table.Columns.Add("Address", typeof(string));
            table.Columns.Add("PermanentAddress", typeof(string));
            table.Columns.Add("TemporaryAddress", typeof(string));
            table.Columns.Add("NationId", typeof(string));
            table.Columns.Add("ReligionId", typeof(string));
            table.Columns.Add("MarriedStatus", typeof(int));
            table.Columns.Add("Status", typeof(int));
            table.Columns.Add("Month", typeof(int));
            table.Columns.Add("OfficalDate", typeof(DateTime));
            table.Columns.Add("JoinedDate", typeof(DateTime));
            table.Columns.Add("OutDate", typeof(DateTime));
            table.Columns.Add("DepartmentId", typeof(int));
            table.Columns.Add("DepartmentPath", typeof(string));
            table.Columns.Add("DepartmentName", typeof(string));
            table.Columns.Add("TitleId", typeof(string));
            table.Columns.Add("TitleName", typeof(string));
            table.Columns.Add("PositionId", typeof(string));
            table.Columns.Add("PositionName", typeof(string));
            table.Columns.Add("PhoneNumber", typeof(string));
            table.Columns.Add("Email", typeof(string));
            table.Columns.Add("ManagerUserId", typeof(string));
            table.Columns.Add("ManagerFullName", typeof(string));
            table.Columns.Add("SuccessorUserId", typeof(string));
            table.Columns.Add("SuccessorFullName", typeof(string));
            table.Columns.Add("WorkingForm", typeof(int));
            table.Columns.Add("TaxCode", typeof(string));
            table.Columns.Add("EnrollNumberTT", typeof(string));
            table.Columns.Add("EnrollNumberNeo", typeof(string));
            table.Columns.Add("Note", typeof(string));
            table.Columns.Add("ContractCode", typeof(string));
            table.Columns.Add("InsuranceCode", typeof(string));
            table.Columns.Add("InsuranceStatus", typeof(int));
            table.Columns.Add("InsuranceName", typeof(string));
            table.Columns.Add("IdCardNumber", typeof(string));
            table.Columns.Add("IdCardDateOfIssue", typeof(DateTime));
            table.Columns.Add("IdCardPlaceOfIssue", typeof(string));
            table.Columns.Add("IsActive", typeof(bool));
            table.Columns.Add("ConcurrencyStamp", typeof(string));
            table.Columns.Add("CreateTime", typeof(DateTime));
            table.Columns.Add("CreatorId", typeof(string));
            table.Columns.Add("CreatorFullName", typeof(string));
            table.Columns.Add("LastUpdate", typeof(DateTime));
            table.Columns.Add("LastUpdateUserId", typeof(string));
            table.Columns.Add("LastUpdateFullName", typeof(string));
            table.Columns.Add("IsDelete", typeof(bool));
            table.Columns.Add("DeleteTime", typeof(DateTime));
            table.Columns.Add("DeleteUserId", typeof(string));
            table.Columns.Add("DeleteFullName", typeof(string));
            table.Columns.Add("AdvanceLeaveGranted", typeof(int));
            table.Columns.Add("CompLeaveGranted", typeof(int));
            table.Columns.Add("ContractExpirationDate", typeof(DateTime));
            table.Columns.Add("PersonnelStatus", typeof(int));
            table.Columns.Add("ExpireDays", typeof(int));

            static object Date(DateTime? value) =>
                value.HasValue && value.Value != DateTime.MinValue ? value.Value : DBNull.Value;

            for (var i = 0; i < count; i++)
            {
                var u = users[startIndex + i];
                table.Rows.Add(
                    startIndex + i + 1,
                    (object)u.Id ?? DBNull.Value,
                    (object)u.Code ?? DBNull.Value,
                    (object)u.TenantId ?? DBNull.Value,
                    (object)u.CompanyId ?? DBNull.Value,
                    (object)u.FullName ?? DBNull.Value,
                    (object)u.FirstName ?? DBNull.Value,
                    (object)u.MiddleName ?? DBNull.Value,
                    (object)u.LastName ?? DBNull.Value,
                    Date(u.Birthday),
                    (object)u.Avatar ?? DBNull.Value,
                    (int)u.Gender,
                    (object)u.UserName ?? DBNull.Value,
                    (object)u.CountryId ?? DBNull.Value,
                    (object)u.ProvinceId ?? DBNull.Value,
                    (object)u.DistrictId ?? DBNull.Value,
                    (object)u.Address ?? DBNull.Value,
                    (object)u.PermanentAddress ?? DBNull.Value,
                    (object)u.TemporaryAddress ?? DBNull.Value,
                    (object)u.NationId ?? DBNull.Value,
                    (object)u.ReligionId ?? DBNull.Value,
                    (int)u.MarriedStatus,
                    (int)u.Status,
                    u.Month.HasValue ? (int)u.Month.Value : (object)DBNull.Value,
                    Date(u.OfficalDate),
                    u.JoinedDate == DateTime.MinValue ? DateTime.Now : u.JoinedDate,
                    Date(u.OutDate),
                    u.DepartmentId.HasValue ? u.DepartmentId.Value : (object)DBNull.Value,
                    (object)u.DepartmentPath ?? DBNull.Value,
                    (object)u.DepartmentName ?? DBNull.Value,
                    (object)u.TitleId ?? DBNull.Value,
                    (object)u.TitleName ?? DBNull.Value,
                    (object)u.PositionId ?? DBNull.Value,
                    (object)u.PositionName ?? DBNull.Value,
                    (object)u.PhoneNumber ?? DBNull.Value,
                    (object)u.Email ?? DBNull.Value,
                    (object)u.ManagerUserId ?? DBNull.Value,
                    (object)u.ManagerFullName ?? DBNull.Value,
                    (object)u.SuccessorUserId ?? DBNull.Value,
                    (object)u.SuccessorFullName ?? DBNull.Value,
                    (int)u.WorkingForm,
                    (object)u.TaxCode ?? DBNull.Value,
                    (object)u.EnrollNumberTT ?? DBNull.Value,
                    (object)u.EnrollNumberNeo ?? DBNull.Value,
                    (object)u.Note ?? DBNull.Value,
                    (object)u.ContractCode ?? DBNull.Value,
                    (object)u.InsuranceCode ?? DBNull.Value,
                    (int)u.InsuranceStatus,
                    (object)u.InsuranceName ?? DBNull.Value,
                    (object)u.IdCardNumber ?? DBNull.Value,
                    Date(u.IdCardDateOfIssue),
                    (object)u.IdCardPlaceOfIssue ?? DBNull.Value,
                    u.IsActive,
                    (object)u.ConcurrencyStamp ?? DBNull.Value,
                    u.CreateTime == DateTime.MinValue ? DateTime.Now : u.CreateTime,
                    (object)u.CreatorId ?? DBNull.Value,
                    (object)u.CreatorFullName ?? DBNull.Value,
                    Date(u.LastUpdate),
                    (object)u.LastUpdateUserId ?? DBNull.Value,
                    (object)u.LastUpdateFullName ?? DBNull.Value,
                    u.IsDelete,
                    Date(u.DeleteTime),
                    (object)u.DeleteUserId ?? DBNull.Value,
                    (object)u.DeleteFullName ?? DBNull.Value,
                    u.AdvanceLeaveGranted.HasValue ? u.AdvanceLeaveGranted.Value : (object)DBNull.Value,
                    u.CompLeaveGranted.HasValue ? u.CompLeaveGranted.Value : (object)DBNull.Value,
                    Date(u.ContractExpirationDate),
                    (int)u.PersonnelStatus,
                    u.ExpireDays.HasValue ? u.ExpireDays.Value : (object)DBNull.Value);
            }

            return table;
        }


        public async Task<int> UpdateAsync(User users)
        {
            try
            {
                int rowAffected = 0;
                using (SqlConnection con = new(_connectionString))
                {
                    if (con.State == ConnectionState.Closed)
                        await con.OpenAsync();

                    DynamicParameters param = new();
                    param.Add("@Id", users.Id);
                    param.Add("@TenantId", users.TenantId);
                    param.Add("@CompanyId", users.CompanyId);
                    param.Add("@FullName", users.FullName);
                    param.Add("@FirstName", users.FirstName);
                    param.Add("@MiddleName", users.MiddleName);
                    param.Add("@LastName", users.LastName);
                    param.Add("@Birthday", users.Birthday);
                    param.Add("@Avatar", users.Avatar);
                    param.Add("@Gender", users.Gender);
                    param.Add("@CountryId", users.CountryId);
                    param.Add("@ProvinceId", users.ProvinceId);
                    param.Add("@DistrictId", users.DistrictId);
                    param.Add("@Address", users.Address);
                    param.Add("@PermanentAddress", users.PermanentAddress);
                    param.Add("@TemporaryAddress", users.TemporaryAddress);
                    param.Add("@NationId", users.NationId);
                    param.Add("@ReligionId", users.ReligionId);
                    param.Add("@MarriedStatus", users.MarriedStatus);
                    param.Add("@Status", users.Status);
                    param.Add("@Month", users.Month);
                    if (users.OfficalDate != null && users.OfficalDate != DateTime.MinValue)
                    {
                        param.Add("@OfficalDate", users.OfficalDate);
                    }
                    param.Add("@JoinedDate", users.JoinedDate);
                    if (users.OutDate != null && users.OutDate != DateTime.MinValue)
                    {
                        param.Add("@OutDate", users.OutDate);
                    }
                    param.Add("@DepartmentId", users.DepartmentId);
                    param.Add("@DepartmentPath", users.DepartmentPath);
                    param.Add("@DepartmentName", users.DepartmentName);
                    param.Add("@TitleId", users.TitleId);
                    param.Add("@TitleName", users.TitleName);
                    param.Add("@PositionId", users.PositionId);
                    param.Add("@PositionName", users.PositionName);
                    param.Add("@PhoneNumber", users.PhoneNumber);
                    param.Add("@Email", users.Email);
                    param.Add("@ManagerUserId", users.ManagerUserId);
                    param.Add("@ManagerFullName", users.ManagerFullName);
                    param.Add("@SuccessorUserId", users.SuccessorUserId);
                    param.Add("@SuccessorFullName", users.SuccessorFullName);
                    param.Add("@WorkingForm", users.WorkingForm);
                    param.Add("@TaxCode", users.TaxCode);
                    param.Add("@EnrollNumberTT", users.EnrollNumberTT);
                    param.Add("@EnrollNumberNeo", users.EnrollNumberNeo);
                    param.Add("@Note", users.Note);
                    param.Add("@InsuranceCode", users.InsuranceCode);
                    param.Add("@InsuranceStatus", users.InsuranceStatus);
                    param.Add("@InsuranceName", users.InsuranceName);
                    param.Add("@IdCardNumber", users.IdCardNumber);
                    if (users.IdCardDateOfIssue != null && users.IdCardDateOfIssue != DateTime.MinValue)
                    {
                        param.Add("@IdCardDateOfIssue", users.IdCardDateOfIssue);
                    }
                    param.Add("@IdCardPlaceOfIssue", users.IdCardPlaceOfIssue);
                    param.Add("@IsActive", users.IsActive);
                    param.Add("@ConcurrencyStamp", users.ConcurrencyStamp);
                    param.Add("@CreateTime", users.CreateTime);
                    param.Add("@CreatorId", users.CreatorId);
                    param.Add("@CreatorFullName", users.CreatorFullName);
                    if (users.LastUpdate != null && users.LastUpdate != DateTime.MinValue)
                    {
                        param.Add("@LastUpdate", users.LastUpdate);
                    }
                    param.Add("@LastUpdateUserId", users.LastUpdateUserId);
                    param.Add("@LastUpdateFullName", users.LastUpdateFullName);
                    param.Add("@IsDelete", users.IsDelete);
                    if (users.DeleteTime != null && users.DeleteTime != DateTime.MinValue)
                    {
                        param.Add("@DeleteTime", users.DeleteTime);
                    }
                    param.Add("@DeleteUserId", users.DeleteUserId);
                    param.Add("@DeleteFullName", users.DeleteFullName);
                    param.Add("@AdvanceLeaveGranted", users.AdvanceLeaveGranted);
                    param.Add("@CompLeaveGranted", users.CompLeaveGranted);
                    if (users.ContractExpirationDate != null && users.ContractExpirationDate != DateTime.MinValue)
                    {
                        param.Add("@ContractExpirationDate", users.ContractExpirationDate);
                    }
                    param.Add("@PersonnelStatus", users.PersonnelStatus);
                    param.Add("@ExpireDays", users.ExpireDays);
                    rowAffected = await con.ExecuteAsync("[dbo].[spUser_Update]", param, commandType: CommandType.StoredProcedure);
                }
                return rowAffected;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[dbo].[spUser_Update] UpdateAsync UserRepository Error.");
                return -1;
            }
        }

        public async Task<int> DeleteAsync(User user)
        {
            try
            {
                int rowAffected = 0;
                using (SqlConnection con = new(_connectionString))
                {
                    if (con.State == ConnectionState.Closed)
                        await con.OpenAsync();

                    DynamicParameters param = new();
                    param.Add("@Id", user.Id);
                    param.Add("@DeleteUserId", user.DeleteUserId);
                    param.Add("@DeleteFullName", user.DeleteFullName);
                    rowAffected = await con.ExecuteAsync("[dbo].[spUser_DeleteByID]", param, commandType: CommandType.StoredProcedure);
                }
                return rowAffected;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[dbo].[spUser_DeleteByID] DeleteAsync UserRepository Error.");
                return -1;
            }
        }


        public async Task<int> ForceDeleteAsync(string tenantId, string id)
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
                    param.Add("@TenantId", tenantId);
                    rowAffected = await con.ExecuteAsync("[dbo].[spUser_ForceDeleteByID]", param, commandType: CommandType.StoredProcedure);
                }
                return rowAffected;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[dbo].[spUser_ForceDeleteByID] ForceDeleteAsync UserRepository Error.");
                return -1;
            }
        }


        public async Task<User> GetInfoAsync(string id)
        {
            try
            {
                using SqlConnection con = new(_connectionString);
                if (con.State == ConnectionState.Closed)
                    await con.OpenAsync();

                DynamicParameters param = new();
                param.Add("@Id", id);
                return await con.QuerySingleOrDefaultAsync<User>("[dbo].[spUser_SelectByID]", param, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[dbo].[spUser_SelectByID] GetInfoAsync UserRepository Error.");
                return null;
            }
        }


        public async Task<User> GetInfoAsync(string tenantId, string id)
        {
            try
            {
                using SqlConnection con = new(_connectionString);
                if (con.State == ConnectionState.Closed)
                    await con.OpenAsync();

                DynamicParameters param = new();
                param.Add("@Id", id);
                param.Add("@TenantId", tenantId);
                return await con.QuerySingleOrDefaultAsync<User>("[dbo].[spUser_SelectByIDTenantId]", param, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[dbo].[spUser_SelectByIDTenantId] GetInfoAsync UserRepository Error.");
                return null;
            }
        }

        public async Task<User> GetInfoByUserHisAsync(string companyId, string referenceId)
        {
            try
            {
                using SqlConnection con = new(_connectionString);
                if (con.State == ConnectionState.Closed)
                    await con.OpenAsync();

                DynamicParameters param = new();
                param.Add("@CompanyId", companyId);
                param.Add("@ReferenceId", referenceId);
                return await con.QuerySingleOrDefaultAsync<User>("[dbo].[spUser_SelectByUserHis]", param, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[dbo].[spUser_SelectByUserHis] GetInfoAsync UserRepository Error.");
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
					SELECT IIF (EXISTS (SELECT 1 FROM Users WHERE Id = @Id AND IsDelete = 0), 1, 0)";

                var result = await con.ExecuteScalarAsync<bool>(sql, new { Id = id });
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "CheckExistsAsync UserRepository Error.");
                return false;
            }
        }


        public async Task<bool> CheckExistsByTenantIdAsync(string tenantId, string id)
        {
            try
            {
                using SqlConnection con = new(_connectionString);
                if (con.State == ConnectionState.Closed)
                    await con.OpenAsync();

                var sql = @"
					SELECT IIF (EXISTS (SELECT 1 FROM Users WHERE Id = @Id AND TenantId = @TenantId AND IsDelete = 0), 1, 0)";

                var result = await con.ExecuteScalarAsync<bool>(sql, new { Id = id, TenantId = tenantId });
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "CheckExistsByTenantIdAsync UserRepository Error.");
                return false;
            }
        }


        public async Task<bool> CheckExistsByTenantIdActiveAsync(string tenantId, string id)
        {
            try
            {
                using SqlConnection con = new(_connectionString);
                if (con.State == ConnectionState.Closed)
                    await con.OpenAsync();

                var sql = @"
					SELECT IIF (EXISTS (SELECT 1 FROM Users WHERE Id = @Id AND TenantId = @TenantId AND IsActive = 1 AND IsDelete = 0), 1, 0)";

                var result = await con.ExecuteScalarAsync<bool>(sql, new { Id = id, TenantId = tenantId });
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "CheckExistsByTenantIdAsync UserRepository Error.");
                return false;
            }
        }

        public async Task<bool> CheckExistsByCodeAsync(string tenantId, string id, string code)
        {
            try
            {
                using SqlConnection con = new(_connectionString);
                if (con.State == ConnectionState.Closed)
                    await con.OpenAsync();

                var sql = @"
					SELECT IIF (EXISTS (SELECT 1 FROM Users WHERE Id != @Id AND TenantId = @TenantId AND IsDelete = 0 AND Code = @Code), 1, 0)";

                var result = await con.ExecuteScalarAsync<bool>(sql, new { Id = id, TenantId = tenantId, Code = code });
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "CheckExistsByCodeAsync UserRepository Error.");
                return false;
            }
        }

        public async Task<bool> CheckExistsByReferenceIdAsync(string tenantId, string id, string referenceId)
        {
            try
            {
                using SqlConnection con = new(_connectionString);
                if (con.State == ConnectionState.Closed)
                    await con.OpenAsync();

                var sql = @"
					SELECT IIF (EXISTS (SELECT 1 FROM Users WHERE Id != @Id AND TenantId = @TenantId AND IsDelete = 0  AND ReferenceId = @ReferenceId), 1, 0)";

                var result = await con.ExecuteScalarAsync<bool>(sql, new { Id = id, TenantId = tenantId, ReferenceId = referenceId });
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "CheckExistsByEmailAsync UserRepository Error.");
                return false;
            }
        }

        public async Task<bool> CheckExistsByEmailAsync(string tenantId, string id, string email)
        {
            try
            {
                using SqlConnection con = new(_connectionString);
                if (con.State == ConnectionState.Closed)
                    await con.OpenAsync();

                var sql = @"
					SELECT IIF (EXISTS (SELECT 1 FROM Users WHERE Id != @Id AND TenantId = @TenantId AND IsDelete = 0  AND Email = @Email), 1, 0)";

                var result = await con.ExecuteScalarAsync<bool>(sql, new { Id = id, TenantId = tenantId, Email = email });
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "CheckExistsByEmailAsync UserRepository Error.");
                return false;
            }
        }

        public async Task<bool> CheckExistsByPhoneNumberAsync(string tenantId, string id, string phoneNumber)
        {
            try
            {
                using SqlConnection con = new(_connectionString);
                if (con.State == ConnectionState.Closed)
                    await con.OpenAsync();

                var sql = @"
					SELECT IIF (EXISTS (SELECT 1 FROM Users WHERE Id != @Id AND TenantId = @TenantId AND IsDelete = 0  AND PhoneNumber = @PhoneNumber), 1, 0)";

                var result = await con.ExecuteScalarAsync<bool>(sql, new { Id = id, TenantId = tenantId, PhoneNumber = phoneNumber });
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "CheckExistsByPhoneNumberAsync UserRepository Error.");
                return false;
            }
        }


        public async Task<bool> CheckExistsByUserNameAsync(string tenantId, string id, string userName)
        {
            try
            {
                using SqlConnection con = new(_connectionString);
                if (con.State == ConnectionState.Closed)
                    await con.OpenAsync();

                var sql = @"
					SELECT IIF (EXISTS (SELECT 1 FROM Users WHERE Id != @Id AND TenantId = @TenantId AND IsDelete = 0 AND UserName = @UserName), 1, 0)";

                var result = await con.ExecuteScalarAsync<bool>(sql, new { Id = id, TenantId = tenantId, UserName = userName });
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "CheckExistsByUserNameAsync UserRepository Error.");
                return false;
            }
        }

        public async Task<bool> CheckExistsByIdCardNumberAsync(string tenantId, string id, string idCardNumber)
        {
            try
            {
                using SqlConnection con = new(_connectionString);
                if (con.State == ConnectionState.Closed)
                    await con.OpenAsync();

                var sql = @"
					SELECT IIF (EXISTS (SELECT 1 FROM Users WHERE Id != @Id AND TenantId = @TenantId AND IsDelete = 0 AND IdCardNumber = @IdCardNumber), 1, 0)";

                var result = await con.ExecuteScalarAsync<bool>(sql, new { Id = id, TenantId = tenantId, IdCardNumber = idCardNumber });
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "CheckExistsByIdCardNumberAsync UserRepository Error.");
                return false;
            }
        }

        public async Task<bool> CheckExistsByPassportIdAsync(string tenantId, string id, string passportId)
        {
            try
            {
                using SqlConnection con = new(_connectionString);
                if (con.State == ConnectionState.Closed)
                    await con.OpenAsync();

                var sql = @"
					SELECT IIF (EXISTS (SELECT 1 FROM Users WHERE Id != @Id AND TenantId = @TenantId AND IsDelete = 0 AND PassportId = @PassportId), 1, 0)";

                var result = await con.ExecuteScalarAsync<bool>(sql, new { Id = id, TenantId = tenantId, PassportId = passportId });
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "CheckExistsByPassportIdAsync UserRepository Error.");
                return false;
            }
        }

        public async Task<int> GetTotalUserByPosition(string tenantId, string positionId)
        {
            try
            {
                using SqlConnection con = new(_connectionString);
                if (con.State == ConnectionState.Closed)
                    await con.OpenAsync();

                var sql = @"
					SELECT COUNT(*) FROM Users WHERE TenantId = @TenantId AND IsDelete = 0 AND PositionId = @PositionId";

                var result = await con.ExecuteScalarAsync<int>(sql, new { TenantId = tenantId, PositionId = positionId });
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetTotalUserByPosition UserRepository Error.");
                return 0;
            }
        }

        public async Task<bool> CheckExistsByEnrollNumberAsync(string tenantId, string id, int enrollNumber)
        {
            try
            {
                using SqlConnection con = new(_connectionString);
                if (con.State == ConnectionState.Closed)
                    await con.OpenAsync();

                var sql = @"
					SELECT IIF (EXISTS (SELECT 1 FROM Users WHERE Id != @Id AND TenantId = @TenantId AND IsDelete = 0 AND EnrollNumber = @EnrollNumber), 1, 0)";

                var result = await con.ExecuteScalarAsync<bool>(sql, new { Id = id, TenantId = tenantId, EnrollNumber = enrollNumber });
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "CheckExistsByEnrollNumberAsync UserRepository Error.");
                return false;
            }
        }

        public async Task<bool> CheckExistsByCardNumberAsync(string tenantId, string id, string cardNumber)
        {
            try
            {
                using SqlConnection con = new(_connectionString);
                if (con.State == ConnectionState.Closed)
                    await con.OpenAsync();

                var sql = @"
					SELECT IIF (EXISTS (SELECT 1 FROM Users WHERE Id != @Id AND TenantId = @TenantId AND IsDelete = 0 AND CardNumber = @CardNumber), 1, 0)";

                var result = await con.ExecuteScalarAsync<bool>(sql, new { Id = id, TenantId = tenantId, CardNumber = cardNumber });
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "CheckExistsByCardNumberAsync UserRepository Error.");
                return false;
            }
        }

        public async Task<bool> CheckExistsPositonByUserAsync(string tenantId, string positionId)
        {
            try
            {
                using SqlConnection con = new(_connectionString);
                if (con.State == ConnectionState.Closed)
                    await con.OpenAsync();

                var sql = @"
					SELECT IIF (
                        EXISTS (SELECT 1 FROM Users WHERE TenantId = @TenantId AND IsDelete = 0 AND PositionId = @PositionId)
                        OR
                        EXISTS (SELECT 1 FROM MultipleCompanys WHERE TenantId = @TenantId AND PositionId = @PositionId),1,0)";

                var result = await con.ExecuteScalarAsync<bool>(sql, new { TenantId = tenantId, PositionId = positionId });
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "CheckExistsPositonByUserAsync UserRepository Error.");
                return false;
            }
        }

        public async Task<bool> CheckExistsTitleByUserAsync(string tenantId, string titleId)
        {
            try
            {
                using SqlConnection con = new(_connectionString);
                if (con.State == ConnectionState.Closed)
                    await con.OpenAsync();

                var sql = @"
					SELECT IIF (EXISTS (SELECT 1 FROM Users WHERE TenantId = @TenantId AND IsDelete = 0 AND TitleId = @TitleId), 1, 0)";

                var result = await con.ExecuteScalarAsync<bool>(sql, new { TenantId = tenantId, TitleId = titleId });
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "CheckExistsTitleByUserAsync UserRepository Error.");
                return false;
            }
        }

        public async Task<bool> CheckExistsDepartmentByUserAsync(string tenantId, int departmentId)
        {
            try
            {
                using SqlConnection con = new(_connectionString);
                if (con.State == ConnectionState.Closed)
                    await con.OpenAsync();

                var sql = @"
					SELECT IIF (
                        EXISTS (SELECT 1 FROM Users WHERE TenantId = @TenantId AND IsDelete = 0 AND DepartmentId = @DepartmentId)
                        OR
                        EXISTS (SELECT 1 FROM MultipleCompanys WHERE TenantId = @TenantId AND DepartmentId = @DepartmentId),1,0)";

                var result = await con.ExecuteScalarAsync<bool>(sql, new { TenantId = tenantId, DepartmentId = departmentId });
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "CheckExistsDepartmentByUserAsync UserRepository Error.");
                return false;
            }
        }

        public async Task<string> GetCodeAsync(string tenantId)
        {
            try
            {
                using SqlConnection con = new(_connectionString);
                if (con.State == ConnectionState.Closed)
                    await con.OpenAsync();

                DynamicParameters param = new();
                param.Add("@TenantId", tenantId);
                return await con.ExecuteScalarAsync<string>("[dbo].[spUser_Code]", param, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetCode UserRepository Error.");
                return string.Empty;
            }
        }

        public async Task<SearchResult<UserSearchViewModel>> SearchIsTechAsync(string tenantId, string branchId, string keyword, int? officeId, int page, int pageSize)
        {
            try
            {
                using SqlConnection con = new(_connectionString);
                if (con.State == ConnectionState.Closed)
                    await con.OpenAsync();

                DynamicParameters param = new();
                param.Add("@TenantId", tenantId);
                param.Add("@BranchId", branchId);
                param.Add("@Keyword", keyword);
                param.Add("@OfficeId", officeId);
                param.Add("@page", page);
                param.Add("@pageSize", pageSize);

                using var multi = await con.QueryMultipleAsync("[dbo].[spUser_SearchIsTech]", param, commandType: CommandType.StoredProcedure);
                return new SearchResult<UserSearchViewModel>
                {
                    TotalRows = (await multi.ReadAsync<int>()).SingleOrDefault(),
                    Data = (await multi.ReadAsync<UserSearchViewModel>()).ToList()
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[dbo].[spUser_SearchIsTech] SearchIsTechAsync UserRepository Error.");
                return new SearchResult<UserSearchViewModel> { TotalRows = 0, Data = null };
            }
        }

        public async Task<bool> CheckExistsByCountryIdAsync(string countryId)
        {
            try
            {
                using SqlConnection con = new(_connectionString);
                if (con.State == ConnectionState.Closed)
                    await con.OpenAsync();

                var sql = @"
					SELECT IIF (EXISTS (SELECT 1 FROM Users WHERE CountryId = @CountryId AND IsDelete = 0), 1, 0)";

                var result = await con.ExecuteScalarAsync<bool>(sql, new { CountryId = countryId });
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "CheckExistsByCountryIdAsync UserRepository Error.");
                return false;
            }
        }

        public async Task<bool> CheckExistsByProvinceIdAsync(string provinceId)
        {
            try
            {
                using SqlConnection con = new(_connectionString);
                if (con.State == ConnectionState.Closed)
                    await con.OpenAsync();

                var sql = @"
					SELECT IIF (EXISTS (SELECT 1 FROM Users WHERE ProvinceId = @ProvinceId AND IsDelete = 0), 1, 0)";

                var result = await con.ExecuteScalarAsync<bool>(sql, new { ProvinceId = provinceId });
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "CheckExistsByProvinceIdAsync UserRepository Error.");
                return false;
            }
        }

        public async Task<bool> CheckExistsByDistrictIdAsync(string districtId)
        {
            try
            {
                using SqlConnection con = new(_connectionString);
                if (con.State == ConnectionState.Closed)
                    await con.OpenAsync();

                var sql = @"
					SELECT IIF (EXISTS (SELECT 1 FROM Users WHERE DistrictId = @DistrictId AND IsDelete = 0), 1, 0)";

                var result = await con.ExecuteScalarAsync<bool>(sql, new { DistrictId = districtId });
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "CheckExistsByDistrictIdAsync UserRepository Error.");
                return false;
            }
        }

        public async Task<bool> CheckExistsByNationIdAsync(string nationId)
        {
            try
            {
                using SqlConnection con = new(_connectionString);
                if (con.State == ConnectionState.Closed)
                    await con.OpenAsync();

                var sql = @"
					SELECT IIF (EXISTS (SELECT 1 FROM Users WHERE NationId = @NationId AND IsDelete = 0), 1, 0)";

                var result = await con.ExecuteScalarAsync<bool>(sql, new { NationId = nationId });
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "CheckExistsByNationIdAsync UserRepository Error.");
                return false;
            }
        }

        public async Task<bool> CheckExistsByReligionIdAsync(string religionId)
        {
            try
            {
                using SqlConnection con = new(_connectionString);
                if (con.State == ConnectionState.Closed)
                    await con.OpenAsync();

                var sql = @"
					SELECT IIF (EXISTS (SELECT 1 FROM Users WHERE ReligionId = @ReligionId AND IsDelete = 0), 1, 0)";

                var result = await con.ExecuteScalarAsync<bool>(sql, new { ReligionId = religionId });
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "CheckExistsByReligionIdAsync UserRepository Error.");
                return false;
            }
        }

        public async Task<bool> CheckExistsByJobIdAsync(string jobId)
        {
            try
            {
                using SqlConnection con = new(_connectionString);
                if (con.State == ConnectionState.Closed)
                    await con.OpenAsync();

                var sql = @"
					SELECT IIF (EXISTS (SELECT 1 FROM Users WHERE JobId = @JobId AND IsDelete = 0), 1, 0)";

                var result = await con.ExecuteScalarAsync<bool>(sql, new { JobId = jobId });
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "CheckExistsByJobIdAsync UserRepository Error.");
                return false;
            }
        }

        public async Task<int> UpdateByUserFullNameAsync(string userId, string userFullName)
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
                    param.Add("@UserFullName", userFullName);

                    rowAffected = await con.ExecuteAsync("[dbo].[Update_By_UserFullName]", param, commandType: CommandType.StoredProcedure);
                }
                return rowAffected;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[dbo].[Update_By_UserFullName] UpdateByUserFullNameAsync  UserRepository Error.");
                return -1;
            }
        }

        public async Task<int> UpdateByUserUsedAsync(string userId, string userFullName, string phoneNumber)
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
                    param.Add("@UserFullName", userFullName);
                    param.Add("@PhoneNumber", phoneNumber);

                    rowAffected = await con.ExecuteAsync("[dbo].[spUser_UpdateByUserUsed]", param, commandType: CommandType.StoredProcedure);
                }
                return rowAffected;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[dbo].[spUser_UpdateByUserUsed] UpdateByUserUsedAsync  UserRepository Error.");
                return -1;
            }
        }

        public async Task<bool> CheckExistsIsLockAsync(string tenantId, string id)
        {
            try
            {
                using SqlConnection con = new(_connectionString);
                if (con.State == ConnectionState.Closed)
                    await con.OpenAsync();

                var sql = @"
					SELECT IIF (EXISTS (SELECT 1 FROM Users WHERE Id != @Id AND TenantId = @TenantId AND IsLock = 1 AND IsDelete = 0), 1, 0)";

                var result = await con.ExecuteScalarAsync<bool>(sql, new { Id = id, TenantId = tenantId });
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "CheckExistsUserNameAsync UserRepository Error.");
                return false;
            }
        }

        public async Task<int> UpdateLockAsync(string id, bool isLock)
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
                    param.Add("@IsLock", isLock);
                    rowAffected = await con.ExecuteAsync("[dbo].[spUser_Update_Lock]", param, commandType: CommandType.StoredProcedure);
                }
                return rowAffected;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[dbo].[spUser_Update_Lock] UpdateNoteCardAsync UserRepository Error.");
                return -1;
            }
        }

        public async Task<int> UpdatePhoneNumberAsync(string userId, string phoneNumber)
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
                    param.Add("@PhoneNumber", phoneNumber);

                    rowAffected = await con.ExecuteAsync("[dbo].[spUser_Update_PhoneNumber]", param, commandType: CommandType.StoredProcedure);
                }
                return rowAffected;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[dbo].[spUser_Update_PhoneNumber] UpdateByUserUsedAsync  UserRepository Error.");
                return -1;
            }
        }

        public async Task<int> UpdateUserPaperInfoAsync(UserPaperInfo user)
        {
            try
            {
                int rowAffected = 0;
                using (SqlConnection con = new(_connectionString))
                {
                    if (con.State == ConnectionState.Closed)
                        await con.OpenAsync();

                    DynamicParameters param = new();
                    param.Add("@Id", user.Id);
                    param.Add("@TenantId", user.TenantId);
                    param.Add("@CCHNNumber", user.CCHNNumber);
                    if (user.CCHNDateOfIssue != null && user.CCHNDateOfIssue != DateTime.MinValue)
                    {
                        param.Add("@CCHNDateOfIssue", user.CCHNDateOfIssue);
                    }
                    param.Add("@CCHNPlaceOfIssue", user.CCHNPlaceOfIssue);
                    param.Add("@TypeCard", user.TypeCard);
                    param.Add("@IdCardNumber", user.IdCardNumber);
                    if (user.IdCardDateOfIssue != null && user.IdCardDateOfIssue != DateTime.MinValue)
                    {
                        param.Add("@IdCardDateOfIssue", user.IdCardDateOfIssue);
                    }
                    param.Add("@IdCardPlaceOfIssue", user.IdCardPlaceOfIssue);
                    rowAffected = await con.ExecuteAsync("[dbo].[spUser_Update_PaperInfo]", param, commandType: CommandType.StoredProcedure);
                }
                return rowAffected;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[dbo].[spUser_Update_PaperInfo] UpdateAsync UserRepository Error.");
                return -1;
            }
        }


        public async Task<int> UpdateUserOtherInfoAsync(UserOtherInfo user)
        {
            try
            {
                int rowAffected = 0;
                using (SqlConnection con = new(_connectionString))
                {
                    if (con.State == ConnectionState.Closed)
                        await con.OpenAsync();

                    DynamicParameters param = new();
                    param.Add("@Id", user.Id);
                    param.Add("@TenantId", user.TenantId);
                    param.Add("@Address", user.Address);
                    param.Add("@PermanentAddress", user.PermanentAddress);
                    param.Add("@TemporaryAddress", user.TemporaryAddress);
                    param.Add("@MarriedStatus", user.MarriedStatus);
                    param.Add("@PhoneNumber", user.PhoneNumber);
                    param.Add("@EnrollNumberNeo", user.EnrollNumberNeo);
                    param.Add("@EnrollNumberTT", user.EnrollNumberTT);
                    param.Add("@TaxCode", user.TaxCode);
                    rowAffected = await con.ExecuteAsync("[dbo].[spUser_Update_OtherInfo]", param, commandType: CommandType.StoredProcedure);
                }
                return rowAffected;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[dbo].[spUser_Update_OtherInfo] UpdateUserOtherInfoAsync UserRepository Error.");
                return -1;
            }
        }

       
        public async Task<List<UserDetailAllViewModel>> GetUsersByCompanyIdAndRoleId(string tenantId, string companyId, string roleId)
        {
            try
            {
                using SqlConnection con = new(_connectionString);
                if (con.State == ConnectionState.Closed)
                    await con.OpenAsync();

                DynamicParameters param = new();
                param.Add("@TenantId", tenantId);
                param.Add("@CompanyId", companyId);
                param.Add("@RoleId", roleId);
                var results = await con.QueryAsync<UserDetailAllViewModel>("[dbo].[spUser_SelectAllByCompanyIdAndRoleId]", param, commandType: CommandType.StoredProcedure);
                return results.ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[dbo].[spUser_SelectAllByCompanyIdAndRoleId] SelectAllAsync UserRepository Error.");
                return new List<UserDetailAllViewModel>();
            }
        }

        // danh sach user dang lam viec
        public async Task<List<UserSearchAllViewModel>> SelectAllCurrentAsync(string tenantId, string companyId)
        {
            try
            {
                using SqlConnection con = new(_connectionString);
                if (con.State == ConnectionState.Closed)
                    await con.OpenAsync();

                DynamicParameters param = new();
                param.Add("@TenantId", tenantId);
                param.Add("@CompanyId", companyId);
                var results = await con.QueryAsync<UserSearchAllViewModel>("[dbo].[spUser_SelectAllCurrent]", param, commandType: CommandType.StoredProcedure);
                return results.ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[dbo].[spUser_SelectAllCurrent] SelectAllUsersAsync UserRepository Error.");
                return new List<UserSearchAllViewModel>();
            }
        }

        public async Task<int> UpdateManagerUserAysnc(string userId)
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
                    rowAffected = await con.ExecuteAsync("[dbo].[spUser_UpdateManagerUser]", param, commandType: CommandType.StoredProcedure);
                }
                return rowAffected;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[dbo].[spUser_UpdateManagerUser] UpdateManagerUserAsync UserRepository Error.");
                return -1;
            }
        }


        public async Task<int> DayOffsUpdateUsedAsync(string tenantId, string userId, DateTime date, int minutes)
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
                    param.Add("@UserId", userId);
                    param.Add("@Date", date);
                    param.Add("@Minutes", minutes);
                    rowAffected = await con.ExecuteAsync("[dbo].[spDayOffs_Update_Used]", param, commandType: CommandType.StoredProcedure);
                }
                return rowAffected;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[dbo].[spDayOffs_Update_Used] DayOffsUpdateUsedAsync UserRepository Error.");
                return -1;
            }
        }

        public async Task<bool> DayOffsCheckUserExist(string tenantId, string companyId, string userId)
        {
            try
            {
                using SqlConnection con = new(_connectionString);
                if (con.State == ConnectionState.Closed)
                    await con.OpenAsync();

                var sql = @"
					SELECT IIF (EXISTS (SELECT 1 FROM DayOffs WHERE TenantId = @TenantId AND CompanyId = @CompanyId AND UserId = @UserId), 1, 0)"
                ;

                var result = await con.ExecuteScalarAsync<bool>(sql, new { TenantId = tenantId, CompanyId = companyId, UserId = userId });
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "DayOffsCheckUserExist UserRepository Error.");
                return false;
            }
        }

        public async Task<int> DayOffsUpdateOldExpiryDateAsync(string tenantId, string companyId, string id, DateTime? expiryDate, DateTime? oldExpiryDate, string lastUpdateUserId, string lastUpdateFullName)
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
                    param.Add("@Id", id);
                    param.Add("@ExpiryDate", expiryDate);
                    param.Add("@OldExpiryDate", oldExpiryDate);
                    param.Add("@LastUpdateUserId", lastUpdateUserId);
                    param.Add("@LastUpdateFullName", lastUpdateFullName);
                    rowAffected = await con.ExecuteAsync("[dbo].[spDayOffs_UpdateOldExpiryDate]", param, commandType: CommandType.StoredProcedure);
                }
                return rowAffected;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[dbo].[spDayOffs_UpdateOldExpiryDate] UpdateOldExpiryDateAsync UserRepository Error.");
                return -1;
            }
        }

        public async Task<int> DayOffsUpdateRemainingLeaveDaysAsync(string tenantId, string companyId, string userId, int unusedLeave, string lastUpdateUserId, string lastUpdateFullName)
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
                    param.Add("@UserId", userId);
                    param.Add("@UnusedLeave", unusedLeave);
                    param.Add("@LastUpdateUserId", lastUpdateUserId);
                    param.Add("@LastUpdateFullName", lastUpdateFullName);
                    rowAffected = await con.ExecuteAsync("[dbo].[spDayOffs_UpdateRemainingLeaveDays]", param, commandType: CommandType.StoredProcedure);
                }
                return rowAffected;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[dbo].[spDayOffs_UpdateRemainingLeaveDays] UpdateRemainingLeaveDaysAsync UserRepository Error.");
                return -1;
            }
        }

        public async Task<int> DayOffsUpdateUsedLeaveDaysAsync(string tenantId, string companyId, string userId, decimal totalDate)
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
                    param.Add("@UserId", userId);
                    param.Add("@TotalDate", totalDate);
                    rowAffected = await con.ExecuteAsync("[dbo].[spDayOffs_UpdateUsedLeaveDays]", param, commandType: CommandType.StoredProcedure);
                }
                return rowAffected;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[dbo].[spDayOffs_UpdateUsedLeaveDays] UpdateUsedLeaveDaysAsync UserRepository Error.");
                return -1;
            }
        }

        public async Task<int> DayOffs_DeleteByUserAsync(string tenantId, string companyId, string userId)
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
                    param.Add("@UserId", userId);
                    rowAffected = await con.ExecuteAsync("[dbo].[spUsersDayOffs_DeleteByUserId]", param, commandType: CommandType.StoredProcedure);
                }
                return rowAffected;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[dbo].[spUsersDayOffs_DeleteByUserId] DayOffs_DeleteByUserIdAsync WorkScheduleUsersRepository Error.");
                return -1;
            }
        }

        public async Task<List<TempTable>> GetAllUsers(string tenantId, string companyId, string userId, string managerUserId, string managerUserIdOld, string creatorId, string creatorFullName)
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
                param.Add("@ManagerUserId", managerUserId);
                param.Add("@ManagerUserIdOld", managerUserIdOld);
                param.Add("@CreatorId", creatorId);
                param.Add("@CreatorFullName", creatorFullName);
                var results = await con.QueryAsync<TempTable>("[dbo].[spUser_CheckRoleQLTTTP]", param, commandType: CommandType.StoredProcedure);
                return results.ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[dbo].[spUser_CheckRoleQLTTTP] SelectAllByUserIdAsync MultipleCompanyRepository Error.");
                return new List<TempTable>();
            }
        }

        public async Task<User> GetInfoMultiAsync(string companyId, string userId)
        {
            try
            {
                using SqlConnection con = new(_connectionString);
                if (con.State == ConnectionState.Closed)
                    await con.OpenAsync();

                DynamicParameters param = new();
                param.Add("@CompanyId", companyId);
                param.Add("@UserId", userId);
                return await con.QuerySingleOrDefaultAsync<User>("[dbo].[spUser_SelectMultiByID]", param, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[dbo].[spUser_SelectMultiByID] GetInfoMultiAsync UserRepository Error.");
                return null;
            }
        }

        public async Task<List<string>> GetListQLNSAsync(string tenantId, string companyId)
        {
            try
            {
                using SqlConnection con = new(_connectionString);
                if (con.State == ConnectionState.Closed)
                    await con.OpenAsync();

                DynamicParameters param = new();
                param.Add("@TenantId", tenantId);
                param.Add("@CompanyId", companyId);
                var result = await con.QueryAsync<string>("[dbo].[spUser_GetListQLNS]", param, commandType: CommandType.StoredProcedure);
                return result.ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[dbo].[spUser_GetListQLNS] GetListQLNS UserRepository Error.");
                return null;
            }
        }

        public async Task<int> UpdateDepartmentIsActiveAsync(string tenantId, string companyId, int departmentId)
        {
            try
            {
                using SqlConnection con = new(_connectionString);
                if (con.State == ConnectionState.Closed)
                    await con.OpenAsync();

                var sql = @"UPDATE Users
                            SET DepartmentId = NULL, DepartmentName = NULL
                            WHERE TenantId=@TenantId AND CompanyId=@CompanyId AND DepartmentId=@DepartmentId";
                var result = await con.ExecuteAsync(sql, new { TenantId = tenantId, CompanyId = companyId, DepartmentId = departmentId });
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "UpdateDepartmentIsActive UserRepository Error.");
                return -1;
            }
        }

        public async Task<User> GetUserByDoctorCodeAsync(string companyId, string doctorCode)
        {
            try
            {
                using SqlConnection con = new(_connectionString);
                if (con.State == ConnectionState.Closed)
                    await con.OpenAsync();

                DynamicParameters param = new();
                param.Add("@CompanyId", companyId);
                param.Add("@DoctorCode", doctorCode);
                return await con.QuerySingleOrDefaultAsync<User>("[dbo].[spMultipleCompanys_GetUserByDoctorCode]", param, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[dbo].[spMultipleCompanys_GetUserByDoctorCode] GetUserByDoctorCodeAsync UserRepository Error.");
                return null;
            }
        }

        public async Task<bool> CheckSameDepartmentAsync(string tenantId, string userId1, string userId2)
        {
            try
            {
                using SqlConnection con = new(_connectionString);
                if (con.State == ConnectionState.Closed)
                    await con.OpenAsync();

                var sql = @"
                    SELECT IIF(EXISTS (
                        SELECT 1
                        FROM [dbo].[Users] u1
                        INNER JOIN [dbo].[Users] u2
                            ON  u1.CompanyId    = u2.CompanyId
                            AND u1.DepartmentId = u2.DepartmentId
                        WHERE u1.TenantId = @TenantId AND u1.Id = @UserId1 AND u1.IsDelete = 0
                          AND u2.TenantId = @TenantId AND u2.Id = @UserId2 AND u2.IsDelete = 0
                    ), 1, 0)";

                return await con.ExecuteScalarAsync<bool>(sql, new { TenantId = tenantId, UserId1 = userId1, UserId2 = userId2 });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "CheckSameDepartmentAsync UserRepository Error.");
                return false;
            }
        }

        public async Task<List<(int, int)?>> CountByRelationshipAsync(string companyId, BriefUser currentUser, CancellationToken cancellationToken)
        {
            const string sql = """
                               SELECT u.Status, COUNT(u.Id) AS Total 
                               FROM [dbo].[Users] u
                               WHERE u.CompanyId = @CompanyId
                                 AND u.TenantId = @TenantId
                                 AND u.IsDelete = 0
                               GROUP BY u.Status;
                               """;
            var parameters = new DynamicParameters();
            parameters.Add("@CompanyId", companyId);
            parameters.Add("@TenantId", currentUser.TenantId);

            await using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync(cancellationToken);
            var result = await connection.QueryAsync<(int Status, int Total)?>(sql, parameters);
            return result.ToList();
        }
    }
}
