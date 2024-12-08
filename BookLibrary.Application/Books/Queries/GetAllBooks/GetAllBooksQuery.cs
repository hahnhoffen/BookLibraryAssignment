using MediatR;
using BookLibrary.Domain.Entities;
using BookLibrary.Application.Common;

namespace BookLibrary.Application.Books.Queries.GetAllBooks
{
    public class GetAllBooksQuery : IRequest<OperationResult<List<Book>>>
    {
    }
}
