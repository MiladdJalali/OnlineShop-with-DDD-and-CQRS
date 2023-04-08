using MediatR;

namespace Project.Application.Aggregates.Users.Queries.GetUserCollection
{
    public class GetUserCollectionQuery :
        BaseCollectionQuery, IRequest<BaseCollectionResult<UserQueryResult>>
    {
    }
}