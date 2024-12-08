using MediatR;
using BookLibrary.Domain.Entities;
using BookLibrary.Domain.Interface;
using BookLibrary.Application.Common;

namespace BookLibrary.Application.Authors.Commands.AddAuthor
{
    public class AddAuthorCommandHandler : IRequestHandler<AddAuthorCommand, OperationResult<Author>>
    {
        private readonly IAuthorRepository _authorRepository;

        public AddAuthorCommandHandler(IAuthorRepository authorRepository)
        {
            _authorRepository = authorRepository;
        }

        public async Task<OperationResult<Author>> Handle(AddAuthorCommand request, CancellationToken cancellationToken)
        {
            // Check for duplicates
            var authors = await _authorRepository.GetAllAsync();
            if (authors.Any(author => author.Name == request.NewAuthor.Name))
            {
                return OperationResult<Author>.FailureResult("An author with this name already exists.");
            }
            var newAuthor = new Author
            {
                Id = Guid.NewGuid(),
                Name = request.NewAuthor.Name
            };

            await _authorRepository.AddAsync(newAuthor);
            return OperationResult<Author>.SuccessResult(newAuthor, "Author successfully added.");
        }
    }
}
