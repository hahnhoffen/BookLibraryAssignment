using BookLibrary.Domain.Entities;
using MediatR;
using BookLibrary.Application.Common;

namespace BookLibrary.Application.Users.Queries.GetUser
{
    public class GetUserQuery : IRequest<OperationResult<User>>
    {
        public Guid UserId { get; }

        public GetUserQuery(Guid userId)
        {
            UserId = userId;
        }
    }
}