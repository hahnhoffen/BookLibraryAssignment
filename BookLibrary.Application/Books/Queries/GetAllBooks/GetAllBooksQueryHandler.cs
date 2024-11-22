using MediatR;
using BookLibrary.Domain.Entities;
using BookLibrary.Infrastructure.DataBase;

namespace BookLibrary.Application.Books.Queries.GetAllBooks
{
    public class GetAllBooksQueryHandler : IRequestHandler<GetAllBooksQuery, List<Book>>
    {
        private readonly FakeDatabase _fakeDatabase;
        public GetAllBooksQueryHandler(FakeDatabase fakeDatabase)
        {
            _fakeDatabase = fakeDatabase;
        }
        public Task<List<Book>> Handle(GetAllBooksQuery request, CancellationToken cancellationToken)
        {
            return Task.FromResult(_fakeDatabase.Books);
        }
    }
}
