using MediatR;
using BookLibrary.Domain.Entities;
using System.Collections.Generic;

namespace BookLibrary.Application.Books.Commands.DeleteBook
{
    public class DeleteBookCommand : IRequest<List<Book>>
    {
        public DeleteBookCommand(Guid bookId)
        {
            BookId = bookId;
        }

        public Guid BookId { get; set; }
    }
}
