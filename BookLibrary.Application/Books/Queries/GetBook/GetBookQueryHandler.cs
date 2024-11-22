using MediatR;
using BookLibrary.Domain.Entities;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BookLibrary.Infrastructure.DataBase;

namespace BookLibrary.Application.Books.Queries.GetBook
{
    public class GetBookQueryHandler : IRequestHandler<GetBookQuery, Book>
    {
        private readonly FakeDatabase _fakeDatabase;

        public GetBookQueryHandler(FakeDatabase fakeDatabase)
        {
            _fakeDatabase = fakeDatabase;
        }

        public Task<Book> Handle(GetBookQuery request, CancellationToken cancellationToken)
        {
            var book = _fakeDatabase.Books.FirstOrDefault(book => book.Id == request.BookId);
            if (book == null)
                throw new KeyNotFoundException("Book not found.");

            return Task.FromResult(book);
        }
    }
}
