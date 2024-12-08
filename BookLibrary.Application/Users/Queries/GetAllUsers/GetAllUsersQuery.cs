using BookLibrary.Domain.Entities;
using MediatR;
using BookLibrary.Application.Common;

namespace BookLibrary.Application.Users.Queries.GetAllUsers
{
    public class GetAllUsersQuery : IRequest<OperationResult<List<User>>>
    {
    }
}