using MediatR;
using BookLibrary.Application.Common;
using BookLibrary.Domain.Entities;

namespace BookLibrary.Application.Users.Commands.DeleteUser
{
    public class DeleteUserCommand : IRequest<OperationResult<List<User>>>
    {
        public Guid UserId { get; }

        public DeleteUserCommand(Guid userId)
        {
            UserId = userId;
        }
    }
}