using MediatR;
using BookLibrary.Domain.Entities;
using BookLibrary.Domain.Interface;

namespace BookLibrary.Application.Authors.Commands.AddAuthor
{
    public class AddAuthorCommandHandler : IRequestHandler<AddAuthorCommand, Author>
    {
        private readonly IAuthorRepository _authorRepository;

        public AddAuthorCommandHandler(IAuthorRepository authorRepository)
        {
            _authorRepository = authorRepository;
        }

        public async Task<Author> Handle(AddAuthorCommand request, CancellationToken cancellationToken)
        {
            // Check for duplicates
            var authors = await _authorRepository.GetAllAsync();
            if (authors.Any(author => author.Name == request.NewAuthor.Name))
                throw new ArgumentException("An author with this name already exists.");

            // Create and add the new author
            var newAuthor = new Author
            {
                Id = Guid.NewGuid(),
                Name = request.NewAuthor.Name
            };

            await _authorRepository.AddAsync(newAuthor);
            return newAuthor;
        }
    }
}
