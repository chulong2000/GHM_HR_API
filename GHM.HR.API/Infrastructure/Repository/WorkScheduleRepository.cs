using Dapper;
using GHM.HR.API.Domain.IRepository;
using GHM.HR.API.Domain.Models;
using GHM.HR.API.Domain.ViewModels;
using GHM.HR.Domain.Models;
using GHM.HR.Infrastructure.Repository;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Caching.Memory;
using System.Data;

namespace GHM.HR.API.Infrastructure.Repository
{
    public class WorkScheduleRepository : IWorkScheduleRepository
    {
        private readonly string _connectionString;
        private readonly ILogger<WorkScheduleRepository> _logger;
        private readonly IMemoryCache _cache;

        public WorkScheduleRepository(string connectionString, ILogger<WorkScheduleRepository> logger, IMemoryCache cache)
        {
            _connectionString = connectionString;
            _logger = logger;
            _cache = cache;
        }

        public async Task<int> InsertAsync(WorkSchedule workSchedule)
        {
            try
            {
                int rowAffected = 0;
                using (SqlConnection con = new(_connectionString))
                {
                    if (con.State == ConnectionState.Closed)
                        await con.OpenAsync();

                    DynamicParameters param = new();
                    param.Add("@Id", workSchedule.Id);
                    param.Add("@WorkDay", workSchedule.WorkDay);
                    param.Add("@ShiftId", workSchedule.ShiftId);
                    param.Add("@UserId", workSchedule.UserId);
                    param.Add("@CreateTime", workSchedule.CreateTime);
                    param.Add("@CreateId", workSchedule.CreateId);
                    param.Add("@CreateFullName", workSchedule.CreateFullName);
                    param.Add("@CompanyId", workSchedule.CompanyId);
                    param.Add("@TenantId", workSchedule.TenantId);
                    param.Add("@Year", workSchedule.Year);
                    
                    rowAffected = await con.ExecuteAsync("[dbo].[spWorkSchedule_Insert]", param, commandType: CommandType.StoredProcedure);
                }
                return rowAffected;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[dbo].[spWorkSchedule_Insert] InsertAsync WorkSchedule Error.");
                return -1;
            }
        }
    }
}
