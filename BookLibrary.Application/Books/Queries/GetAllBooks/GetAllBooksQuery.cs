using MediatR;
using BookLibrary.Domain.Entities;

namespace BookLibrary.Application.Books.Queries.GetAllBooks
{
    public class GetAllBooksQuery : IRequest<List<Book>>
    {
    }
}
