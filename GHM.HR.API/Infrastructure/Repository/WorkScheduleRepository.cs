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

        
    }
}
