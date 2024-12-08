using BookLibrary.Domain.Entities;
using BookLibrary.Domain.Interface;
using MediatR;
using BookLibrary.Application.Common;
using Microsoft.Extensions.Logging;

namespace BookLibrary.Application.Users.Queries.GetUser
{
    public class GetUserQueryHandler : IRequestHandler<GetUserQuery, OperationResult<User>>
    {
        private readonly IUserRepository _userRepository;
        private readonly ILogger<GetUserQueryHandler> _logger;

        public GetUserQueryHandler(IUserRepository userRepository, ILogger<GetUserQueryHandler> logger)
        {
            _userRepository = userRepository;
            _logger = logger;
        }

        public async Task<OperationResult<User>> Handle(GetUserQuery request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Attempting to fetch user with ID {UserId}.", request.UserId);

                var user = await _userRepository.GetByIdAsync(request.UserId);

                if (user == null)
                {
                    _logger.LogWarning("User with ID {UserId} not found.", request.UserId);
                    return OperationResult<User>.FailureResult("User not found.");
                }

                _logger.LogInformation("User with ID {UserId} successfully retrieved.", request.UserId);
                return OperationResult<User>.SuccessResult(user, "User retrieved successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving user with ID {UserId}.", request.UserId);
                return OperationResult<User>.FailureResult("An error occurred while retrieving the user.");
            }
        }
    }
}