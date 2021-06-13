using System.Threading;
using System.Threading.Tasks;
using MediatR;
using QuickSoft.ConstructionVentory.Infrastructure.Configurations;

namespace QuickSoft.ConstructionVentory.Infrastructure
{
    /// <summary>
    /// Adds transaction to the processing pipeline
    /// </summary>
    /// <typeparam name="TRequest"></typeparam>
    /// <typeparam name="TResponse"></typeparam>
    public class DbTransactionPipeLineBehavior <TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public DbTransactionPipeLineBehavior(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }
        
        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            TResponse response;
            try
            {
                _connectionFactory.BeginTransaction();
                response = await next();
                _connectionFactory.CommitTransaction();
            }
            catch
            {
                _connectionFactory.RollbackTransaction();
                throw;
            }

            return response;
        }
    }
}