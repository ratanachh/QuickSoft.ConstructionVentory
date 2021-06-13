using System.Data;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace QuickSoft.ConstructionVentory.Infrastructure.Configurations
{
    public class DbConnectionFactory : IDbConnectionFactory
    {
        private readonly IConfiguration _configuration;

        public DbConnectionFactory(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        private IDbTransaction _transaction;
        private IDbConnection Connection => new SqlConnection(_configuration.GetConnectionString("ASPNETCORE_QuickSoft_ConnectionString"));

        public IDbConnection GetConnection()
        {
            return Connection;
        }
        
        #region Transaction Handler
        public void BeginTransaction()
        {
            if (_transaction != null)
            {
                return;
            }
            Connection.Open();
            _transaction = Connection.BeginTransaction(IsolationLevel.ReadCommitted);
        }
        public void CommitTransaction()
        {
            try
            {
                _transaction?.Commit();
            }
            catch
            {
                RollbackTransaction();
                throw;
            }
            finally
            {
                if (_transaction != null)
                {
                    _transaction.Dispose();
                    _transaction = null;
                    if (Connection != null && (Connection.State & ConnectionState.Open) != 0)
                    {
                        Connection.Close();
                    }
                }
            }
        }
        public void RollbackTransaction()
        {
            try
            {
                _transaction?.Rollback();
            }
            finally
            {
                if (_transaction != null)
                {
                    _transaction.Dispose();
                    _transaction = null;
                    if (Connection != null && (Connection.State & ConnectionState.Open) != 0)
                    {
                        Connection.Close();
                    }
                }
            }
        }
        #endregion
    }
}