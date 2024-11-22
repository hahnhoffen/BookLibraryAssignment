using MediatR;
using BookLibrary.Domain.Entities;

namespace BookLibrary.Application.Books.Queries.GetBook
{
    public class GetBookQuery : IRequest<Book>
    {
        public GetBookQuery(Guid bookId)
        {
            BookId = bookId;
        }

        public Guid BookId { get; set; }
    }
}