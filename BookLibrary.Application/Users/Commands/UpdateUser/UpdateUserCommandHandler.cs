using MediatR;
using BookLibrary.Application.Common;
using BookLibrary.Domain.Entities;
using BookLibrary.Domain.Interface;
using Microsoft.Extensions.Logging;

namespace BookLibrary.Application.Users.Commands.UpdateUser
{
    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, OperationResult<User>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordService _passwordService;
        private readonly ILogger<UpdateUserCommandHandler> _logger;

        public UpdateUserCommandHandler(IUserRepository userRepository, IPasswordService passwordService, ILogger<UpdateUserCommandHandler> logger)
        {
            _userRepository = userRepository;
            _passwordService = passwordService;
            _logger = logger;
        }

        public async Task<OperationResult<User>> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling UpdateUserCommand for UserId: {UserId}", request.User.Id);

            // Fetch the user
            var user = await _userRepository.GetByIdAsync(request.User.Id);
            if (user == null)
            {
                _logger.LogWarning("User not found for UserId: {UserId}", request.User.Id);
                return OperationResult<User>.FailureResult("User not found.");
            }

            // Check if the email is already taken by another user
            if (await _userRepository.IsUsernameOrEmailTakenAsync(request.User.Username, request.User.Email) && user.Email != request.User.Email)
            {
                _logger.LogWarning("Email conflict for UserId: {UserId} with Email: {Email}", request.User.Id, request.User.Email);
                return OperationResult<User>.FailureResult("Email is already taken.");
            }

            // Update user details
            user.Username = request.User.Username;
            user.Email = request.User.Email;

            if (user.PasswordHash != request.User.PasswordHash)
            {
                _logger.LogInformation("Updating password for UserId: {UserId}", user.Id);
                user.PasswordHash = _passwordService.HashPassword(request.User.PasswordHash);
            }

            // Save the updated user
            await _userRepository.UpdateAsync(user);
            _logger.LogInformation("User updated successfully for UserId: {UserId}", user.Id);

            return OperationResult<User>.SuccessResult(user, "User updated successfully.");
        }
    }
}
