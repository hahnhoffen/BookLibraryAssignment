using MediatR;
using BookLibrary.Domain.Entities;
using BookLibrary.Application.Common;

namespace BookLibrary.Application.Users.Commands.AddUser
{
    public class AddUserCommand : IRequest<OperationResult<User>>
    {
        public User User { get; }

        public AddUserCommand(User user)
        {
            User = user;
        }
    }
}

