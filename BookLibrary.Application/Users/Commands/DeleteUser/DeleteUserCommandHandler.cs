using MediatR;
using BookLibrary.Application.Common;
using BookLibrary.Domain.Interface;
using BookLibrary.Domain.Entities;
using Microsoft.Extensions.Logging;
using BookLibrary.Infrastructure.Repositories;

namespace BookLibrary.Application.Users.Commands.DeleteUser
{
    public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, OperationResult<List<User>>>
    {
        private readonly IUserRepository _userRepository;
        private readonly ILogger<DeleteUserCommandHandler> _logger;
        private readonly FakeUserRepository fakeUserRepository;
        private RealUserRepository realUserRepository;

        public DeleteUserCommandHandler(FakeUserRepository fakeUserRepository)
        {
            this.fakeUserRepository = fakeUserRepository;
        }

        public DeleteUserCommandHandler(RealUserRepository realUserRepository)
        {
            this.realUserRepository = realUserRepository;
        }

        public DeleteUserCommandHandler(IUserRepository userRepository, ILogger<DeleteUserCommandHandler> logger)
        {
            _userRepository = userRepository;
            _logger = logger;

        }

        public async Task<OperationResult<List<User>>> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling DeleteUserCommand for UserId: {UserId}", request.UserId);

            // Fetch the user
            var user = await _userRepository.GetByIdAsync(request.UserId);
            if (user == null)
            {
                _logger.LogWarning("User not found for UserId: {UserId}", request.UserId);
                return OperationResult<List<User>>.FailureResult("User not found.");
            }

            await _userRepository.DeleteAsync(request.UserId);
            _logger.LogInformation("User with UserId: {UserId} deleted successfully.", request.UserId);

            var users = (await _userRepository.GetAllAsync()).ToList();
            _logger.LogInformation("Returning updated list of users. Total count: {UserCount}", users.Count);

            return OperationResult<List<User>>.SuccessResult(users, "User successfully deleted.");
        }
    }
}

