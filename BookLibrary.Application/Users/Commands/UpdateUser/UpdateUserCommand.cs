using MediatR;
using BookLibrary.Application.Common;
using BookLibrary.Domain.Entities;

namespace BookLibrary.Application.Users.Commands.UpdateUser
{
    public class UpdateUserCommand : IRequest<OperationResult<User>>
    {
        public User User { get; }

        public UpdateUserCommand(User user)
        {
            User = user;
        }
    }
}