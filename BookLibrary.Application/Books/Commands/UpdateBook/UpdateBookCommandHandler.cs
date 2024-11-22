using MediatR;
using BookLibrary.Domain.Entities;
using BookLibrary.Infrastructure.DataBase;

namespace BookLibrary.Application.Books.Commands.UpdateBook
{
    public class UpdateBookCommandHandler : IRequestHandler<UpdateBookCommand, List<Book>>
    {
        private readonly FakeDatabase _fakeDatabase;

        public UpdateBookCommandHandler(FakeDatabase fakeDatabase)
        {
            _fakeDatabase = fakeDatabase;
        }
        public Task<List<Book>> Handle(UpdateBookCommand request, CancellationToken cancellationToken)
        {
            var book = _fakeDatabase.Books.FirstOrDefault(book => book.Id == request.UpdatedBook.Id);
            if (book == null)
                throw new KeyNotFoundException("Book not found.");

            book.Title = request.UpdatedBook.Title;
            book.AuthorId = request.UpdatedBook.AuthorId;
            book.Year = request.UpdatedBook.Year;

            return Task.FromResult(_fakeDatabase.Books);
        }
    }
}
