using MediatR;
using BookLibrary.Domain.Entities;
using BookLibrary.Domain.Interface;
using BookLibrary.Application.Common;

namespace BookLibrary.Application.Authors.Commands.DeleteAuthor
{
    public class DeleteAuthorCommandHandler : IRequestHandler<DeleteAuthorCommand, OperationResult<List<Author>>>
    {
        private readonly IAuthorRepository _authorRepository;

        public DeleteAuthorCommandHandler(IAuthorRepository authorRepository)
        {
            _authorRepository = authorRepository;
        }

        public async Task<OperationResult<List<Author>>> Handle(DeleteAuthorCommand request, CancellationToken cancellationToken)
        {
            var author = await _authorRepository.GetByIdAsync(request.AuthorId);
            if (author == null)
            {
                return OperationResult<List<Author>>.FailureResult("Author not found.");
            }
            await _authorRepository.DeleteAsync(request.AuthorId);

            var authors = await _authorRepository.GetAllAsync();
            return OperationResult<List<Author>>.SuccessResult(authors.ToList(), "Author successfully deleted.");
        }
    }
}