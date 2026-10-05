using Dapper;
using GHM.HR.API.Domain.IRepository;
using GHM.HR.API.Domain.ViewModels;
using GHM.HR.Domain.ViewModels;
using Microsoft.Data.SqlClient;
using System.Data;

namespace GHM.HR.API.Infrastructure.Repository
{
    public class WorkShiftUsersRepository : IWorkShiftUsersRepository
    {
        private readonly string _connectionString;
        private readonly ILogger<WorkShiftUsersRepository> _logger;

        public WorkShiftUsersRepository(string connectionString, ILogger<WorkShiftUsersRepository> logger)
        {
            _connectionString = connectionString;
            _logger = logger;
        }

        public async Task<List<ShiftOfUser>> CheckShiftOfUserAsync(string tenantId, string companyId, string userId, DateTime startDate, DateTime endDate)
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
                param.Add("@StartDate", startDate);
                param.Add("@EndDate", endDate);
                var results = await con.QueryAsync<ShiftOfUser>("[dbo].[spWorkShiftUsers_CheckShiftOfUser]", param, commandType: CommandType.StoredProcedure);
                return results.ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[dbo].[spWorkShiftUsers_CheckShiftOfUser] CheckShiftOfUserAsync WorkShiftUsersRepository Error.");
                return [];
            }
        }

        public async Task<WorkShiftRangeResponse> GetWorkShiftDetails(
            string tenantId, string companyId, string userId, DateTime date, TimeSpan startTime, TimeSpan endTime)
        {
            try
            {
                var response = new WorkShiftRangeResponse();

                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    // Định nghĩa tham số truyền vào
                    var param = new DynamicParameters();
                    param.Add("TenantId", tenantId);
                    param.Add("CompanyId", companyId);
                    param.Add("UserId", userId);
                    param.Add("Date", date, DbType.Date);
                    param.Add("StartTime", startTime, DbType.Time);
                    param.Add("EndTime", endTime, DbType.Time);

                    // Sử dụng QueryMultiple để đọc nhiều result set
                    using (var multi = await connection.QueryMultipleAsync(
                        "spWorkShiftUsers_GetByDateAndTimeRange",
                        param,
                        commandType: CommandType.StoredProcedure))
                    {
                        // Đọc Result Set 1: Danh sách các ca
                        response.Shifts = (await multi.ReadAsync<ShiftOverlapDetail>()).ToList();

                        // Đọc Result Set 2: Object Code/Message
                        response.Status = await multi.ReadFirstOrDefaultAsync<WorkShiftStatus>()
                                          ?? new WorkShiftStatus { Code = 0, Message = "No response from DB" };
                    }
                }

                return response;
            }

            catch (Exception ex)
            {
                _logger.LogError(ex, "[dbo].[spWorkShiftUsers_GetByDateAndTimeRange] GetWorkShiftDetails WorkShiftUsersRepository Error.");
                return new WorkShiftRangeResponse();
            }
        }
    }
}
