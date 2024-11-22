using BookLibrary.Domain.Entities;
using MediatR;
using BookLibrary.Infrastructure.DataBase;

namespace BookLibrary.Application.Books.Commands.AddBook
{
    public class AddBookCommandHandler : IRequestHandler<AddBookCommand, List<Book>>
    {
        private readonly FakeDatabase _fakeDatabase;

        public AddBookCommandHandler(FakeDatabase fakeDatabase)
        {
            _fakeDatabase = fakeDatabase;
        }
        public Task<List<Book>> Handle(AddBookCommand request, CancellationToken cancellationToken)
        {
            // Check for duplicate books by title
            if (_fakeDatabase.Books.Any(book => book.Title == request.NewBook.Title))
            {
                throw new Exception("A book with this title already exists.");
            }
            // Add the new book
            _fakeDatabase.Books.Add(request.NewBook);

            return Task.FromResult(_fakeDatabase.Books);
        }
    }
}
