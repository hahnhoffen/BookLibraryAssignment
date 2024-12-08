using MediatR;
using BookLibrary.Domain.Entities;
using BookLibrary.Application.Common;

namespace BookLibrary.Application.Books.Queries.GetBook
{
    public class GetBookQuery : IRequest<OperationResult<Book>>
    {
        public GetBookQuery(Guid bookId)
        {
            BookId = bookId;
        }

        public Guid BookId { get; set; }
    }
}