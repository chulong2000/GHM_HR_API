using GHM.HR.API.Domain.IProviders;
using Microsoft.Data.SqlClient;

namespace GHM.HR.API.Infrastructure.Data
{
    public interface IDbConnectionFactory
    {
        /// <summary>Open a connection to a statically-configured database (QLPK main or HR).</summary>
        Task<SqlConnection> CreateAsync(string name);
    }

    public sealed class SqlConnectionFactory : IDbConnectionFactory
    {
        private readonly IReadOnlyDictionary<string, string?> _connectionStrings;
 

        public SqlConnectionFactory(
            IReadOnlyDictionary<string, string?> connectionStrings)
        {
            _connectionStrings = connectionStrings;
        }

        public async Task<SqlConnection> CreateAsync(string name)
        {
            if (!_connectionStrings.TryGetValue(name, out var connectionString))
                throw new InvalidOperationException($"No connection string configured for '{name}'.");

            var connection = new SqlConnection(connectionString);
            await connection.OpenAsync();
            return connection;
        }

     
    }
}
