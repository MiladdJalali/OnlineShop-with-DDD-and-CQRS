using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Project.Application.Aggregates.Users.Queries.GetUserCollection
{
    public sealed class GetUserCollectionQueryHandler :
        IRequestHandler<GetUserCollectionQuery, BaseCollectionResult<UserQueryResult>>
    {
        private readonly IUserReadRepository userReadRepository;

        public GetUserCollectionQueryHandler(IUserReadRepository userReadRepository)
        {
            this.userReadRepository = userReadRepository;
        }

        public async Task<BaseCollectionResult<UserQueryResult>> Handle(
            GetUserCollectionQuery request,
            CancellationToken cancellationToken)
        {
            var result = userReadRepository.GetAll().OrderBy(i => i.Username);

            var resultWithPaging = await result
                .ApplyPaging(request.PageSize, request.PageIndex)
                .ToArrayAsync(cancellationToken);

            return new BaseCollectionResult<UserQueryResult>
            {
                Result = resultWithPaging,
                TotalCount = resultWithPaging.Length
            };
        }
    }
}