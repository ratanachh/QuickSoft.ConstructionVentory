using System.Data;

namespace QuickSoft.ConstructionVentory.Infrastructure.Configurations
{
    public interface IDbConnectionFactory
    {
        IDbConnection GetConnection();
        void BeginTransaction();
        void CommitTransaction();
        void RollbackTransaction();
    }
}