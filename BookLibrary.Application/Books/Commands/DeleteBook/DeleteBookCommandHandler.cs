using MediatR;
using BookLibrary.Domain.Entities;
using BookLibrary.Infrastructure.DataBase;

namespace BookLibrary.Application.Books.Commands.DeleteBook
{
    public class DeleteBookCommandHandler : IRequestHandler<DeleteBookCommand, List<Book>>
    {
        private readonly FakeDatabase _fakeDatabase;
        public DeleteBookCommandHandler(FakeDatabase fakeDatabase)
        {
            _fakeDatabase = fakeDatabase;
        }
        public Task<List<Book>> Handle(DeleteBookCommand request, CancellationToken cancellationToken)
        {
            var book = _fakeDatabase.Books.FirstOrDefault(book => book.Id == request.BookId);
            if (book == null)
                throw new KeyNotFoundException("Book not found.");

            _fakeDatabase.Books.Remove(book);

            return Task.FromResult(_fakeDatabase.Books);
        }
    }
}
