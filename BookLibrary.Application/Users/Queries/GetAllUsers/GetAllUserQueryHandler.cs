using BookLibrary.Domain.Entities;
using BookLibrary.Domain.Interface;
using MediatR;
using BookLibrary.Application.Common;
using Microsoft.Extensions.Logging;

namespace BookLibrary.Application.Users.Queries.GetAllUsers
{
    public class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, OperationResult<List<User>>>
    {
        private readonly IUserRepository _userRepository;
        private readonly ILogger<GetAllUsersQueryHandler> _logger;

        public GetAllUsersQueryHandler(IUserRepository userRepository, ILogger<GetAllUsersQueryHandler> logger)
        {
            _userRepository = userRepository;
            _logger = logger;
        }

        public async Task<OperationResult<List<User>>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Fetching all users...");

                var users = await _userRepository.GetAllAsync();
                _logger.LogInformation("Successfully retrieved {Count} users.", users.Count());

                return OperationResult<List<User>>.SuccessResult(users.ToList(), "Users retrieved successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving users.");
                return OperationResult<List<User>>.FailureResult("An error occurred while retrieving users.");
            }
        }
    }
}
