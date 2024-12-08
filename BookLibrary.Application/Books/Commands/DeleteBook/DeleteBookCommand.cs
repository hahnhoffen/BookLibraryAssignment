using MediatR;
using BookLibrary.Domain.Entities;
using System.Collections.Generic;
using BookLibrary.Application.Common;

namespace BookLibrary.Application.Books.Commands.DeleteBook
{
    public class DeleteBookCommand : IRequest<OperationResult<List<Book>>>
    {
        public DeleteBookCommand(Guid bookId)
        {
            BookId = bookId;
        }

        public Guid BookId { get; set; }
    }
}
