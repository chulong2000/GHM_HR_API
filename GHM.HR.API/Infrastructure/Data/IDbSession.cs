using Microsoft.Data.SqlClient;

namespace GHM.HR.API.Infrastructure.Data
{
    public interface IDbSession : IAsyncDisposable
    {
        Task<SqlConnection> GetConnectionAsync(string name = DbConnectionNames.Default);
        SqlTransaction Transaction { get; }
        Task BeginTransactionAsync();
        Task CommitAsync();
        Task RollbackAsync();
    }

    public sealed class DbSession : IDbSession
    {
        private readonly IDbConnectionFactory _factory;
        private SqlConnection _connection;
        private string _connectionName;

        public SqlTransaction Transaction { get; private set; }

        public DbSession(IDbConnectionFactory factory)
        {
            _factory = factory;
        }

        public async Task<SqlConnection> GetConnectionAsync(string name = DbConnectionNames.Default)
        {
            if (_connection == null)
            {
                _connection = await _factory.CreateAsync(name);
                _connectionName = name;
            }
            else if (_connectionName != name)
            {
                // A session owns a single connection. Mixing databases in one session
                // (especially inside a transaction) is not supported.
                throw new InvalidOperationException(
                    $"Session already bound to '{_connectionName}'; cannot also use '{name}'.");
            }

            return _connection;
        }

        public async Task BeginTransactionAsync()
        {
            if (Transaction != null)
                throw new InvalidOperationException("A transaction is already active for this session.");

            var connection = await GetConnectionAsync();
            Transaction = (SqlTransaction)await connection.BeginTransactionAsync();
        }

        public async Task CommitAsync()
        {
            if (Transaction == null)
                return;

            await Transaction.CommitAsync();
            await Transaction.DisposeAsync();
            Transaction = null;
        }

        public async Task RollbackAsync()
        {
            if (Transaction == null)
                return;

            await Transaction.RollbackAsync();
            await Transaction.DisposeAsync();
            Transaction = null;
        }

        public async ValueTask DisposeAsync()
        {
            if (Transaction != null)
                await Transaction.DisposeAsync(); // disposing an uncommitted transaction rolls it back
            if (_connection != null)
                await _connection.DisposeAsync();
        }
    }
}
