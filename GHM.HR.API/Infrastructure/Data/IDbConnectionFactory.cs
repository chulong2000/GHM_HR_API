using GHM.HR.API.Domain.IProviders;
using Microsoft.Data.SqlClient;

namespace GHM.HR.API.Infrastructure.Data
{
    public interface IDbConnectionFactory
    {
        /// <summary>Open a connection to a statically-configured database (QLPK main or HR).</summary>
        Task<SqlConnection> CreateAsync(string name);

        /// <summary>Open a connection to the HIS database of a specific company.</summary>
        Task<SqlConnection> CreateForCompanyAsync(string companyId);
    }

    public sealed class SqlConnectionFactory : IDbConnectionFactory
    {
        private readonly IReadOnlyDictionary<string, string> _connectionStrings;
        private readonly IConnectionProvider _hisConnectionProvider;

        public SqlConnectionFactory(
            IReadOnlyDictionary<string, string> connectionStrings,
            IConnectionProvider hisConnectionProvider)
        {
            _connectionStrings = connectionStrings;
            _hisConnectionProvider = hisConnectionProvider;
        }

        public async Task<SqlConnection> CreateAsync(string name)
        {
            if (!_connectionStrings.TryGetValue(name, out var connectionString))
                throw new InvalidOperationException($"No connection string configured for '{name}'.");

            var connection = new SqlConnection(connectionString);
            await connection.OpenAsync();
            return connection;
        }

        public async Task<SqlConnection> CreateForCompanyAsync(string companyId)
        {
            // Resolves (and caches) the company's HIS connection string; throws if none configured.
            var connectionString = await _hisConnectionProvider.GetConnectionStringAsync(companyId);

            var connection = new SqlConnection(connectionString);
            await connection.OpenAsync();
            return connection;
        }
    }
}
