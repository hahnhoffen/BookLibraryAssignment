using MediatR;
using BookLibrary.Domain.Entities;
using BookLibrary.Domain.Interface;
using BookLibrary.Application.Common;

namespace BookLibrary.Application.Authors.Queries.GetAllAuthors
{
    public class GetAllAuthorsQueryHandler : IRequestHandler<GetAllAuthorsQuery, OperationResult<List<Author>>>
    {
        private readonly IAuthorRepository _authorRepository;

        public GetAllAuthorsQueryHandler(IAuthorRepository authorRepository)
        {
            _authorRepository = authorRepository;
        }

        public async Task<OperationResult<List<Author>>> Handle(GetAllAuthorsQuery request, CancellationToken cancellationToken)
        {
            var authors = await _authorRepository.GetAllAsync();
            if (!authors.Any())
            {
                return OperationResult<List<Author>>.FailureResult("No authors found.");
            }

            return OperationResult<List<Author>>.SuccessResult(authors.ToList(), "Authors retrieved successfully.");
        }
    }
}