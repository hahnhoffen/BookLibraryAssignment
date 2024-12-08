using BookLibrary.Domain.Entities;
using BookLibrary.Domain.Interface;
using MediatR;
using BookLibrary.Application.Common;
using Microsoft.Extensions.Logging;

namespace BookLibrary.Application.Users.Commands.AddUser
{
    public class AddUserCommandHandler : IRequestHandler<AddUserCommand, OperationResult<User>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordService _passwordService;
        private readonly ILogger<AddUserCommandHandler> _logger;

        public AddUserCommandHandler(IUserRepository userRepository, IPasswordService passwordService, ILogger<AddUserCommandHandler> logger)
        {
            _userRepository = userRepository;
            _passwordService = passwordService;
            _logger = logger;
        }

        public async Task<OperationResult<User>> Handle(AddUserCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var isTaken = await _userRepository.IsUsernameOrEmailTakenAsync(request.User.Username, request.User.Email);
                if (isTaken)
                {
                    _logger.LogWarning("Failed to add user. Username or email is already taken: {Username}, {Email}",
                        request.User.Username, request.User.Email);

                    return OperationResult<User>.FailureResult("Username or email is already taken.");
                }

                request.User.PasswordHash = _passwordService.HashPassword(request.User.PasswordHash);

                await _userRepository.AddAsync(request.User);
                _logger.LogInformation("User added successfully: {Username}", request.User.Username);

                return OperationResult<User>.SuccessResult(request.User, "User added successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while adding a user.");
                throw;
            }
        }
    }
}
