using MediatR;
using BookLibrary.Domain.Entities;
using BookLibrary.Domain.Interface;
using BookLibrary.Application.Common;
using System.Collections.Generic;

namespace BookLibrary.Application.Authors.Queries.GetAuthor
{
    public class GetAuthorQueryHandler : IRequestHandler<GetAuthorQuery, OperationResult<List<Author>>>
    {
        private readonly IAuthorRepository _authorRepository;

        public GetAuthorQueryHandler(IAuthorRepository authorRepository)
        {
            _authorRepository = authorRepository;
        }

        public async Task<OperationResult<List<Author>>> Handle(GetAuthorQuery request, CancellationToken cancellationToken)
        {
            var author = await _authorRepository.GetByIdAsync(request.AuthorId);
            if (author == null)
            {
                return OperationResult<List<Author>>.FailureResult($"Author with ID {request.AuthorId} not found.");
            }

            return OperationResult<List<Author>>.SuccessResult(new List<Author> { author }, "Test Author");
        }
    }
}