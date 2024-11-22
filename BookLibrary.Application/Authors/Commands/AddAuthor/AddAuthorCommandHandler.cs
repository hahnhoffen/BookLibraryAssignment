using MediatR;
using BookLibrary.Domain.Entities;
using BookLibrary.Infrastructure.DataBase;

namespace BookLibrary.Application.Authors.Commands.AddAuthor
{
    public class AddAuthorCommandHandler : IRequestHandler<AddAuthorCommand, Author>
    {
        private readonly FakeDatabase _fakeDatabase;

        public AddAuthorCommandHandler(FakeDatabase fakeDatabase)
        {
            _fakeDatabase = fakeDatabase;
        }

        public Task<Author> Handle(AddAuthorCommand request, CancellationToken cancellationToken)
        {
            // Check for duplicates
            if (_fakeDatabase.Authors.Any(author => author.Name == request.NewAuthor.Name))
            {
                throw new ArgumentException("An author with this name already exists.");
            }

            // Create and add the new author
            var newAuthor = new Author
            {
                Id = Guid.NewGuid(),
                Name = request.NewAuthor.Name
            };
            _fakeDatabase.Authors.Add(newAuthor);

            return Task.FromResult(newAuthor);
        }
    }
}


