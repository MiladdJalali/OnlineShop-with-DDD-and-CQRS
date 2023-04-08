using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace Project.Application.Behaviors
{
    public class TransactionBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly IUnitOfWork unitOfWork;

        public TransactionBehavior(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<TResponse> Handle(
            TRequest request,
            CancellationToken cancellationToken,
            RequestHandlerDelegate<TResponse> next)
        {
            // We do not need to begin transaction for queries.
            if (request.GetType().Name.StartsWith("Get"))
                return await next().ConfigureAwait(false);

            try
            {
                await unitOfWork.BeginTransaction(cancellationToken).ConfigureAwait(false);
                var response = await next().ConfigureAwait(false);
                await unitOfWork.CommitTransaction(cancellationToken).ConfigureAwait(false);

                return response;
            }
            catch
            {
                await unitOfWork.RollbackTransaction(cancellationToken).ConfigureAwait(false);
                throw;
            }
        }
    }
}