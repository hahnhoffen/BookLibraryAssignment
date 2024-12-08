using MediatR;
using BookLibrary.Domain.Entities;
using System.Collections.Generic;
using BookLibrary.Application.Common;

namespace BookLibrary.Application.Books.Commands.UpdateBook
{
    public class UpdateBookCommand : IRequest<OperationResult<List<Book>>>
    {
        public UpdateBookCommand(Book updatedBook)
        {
            UpdatedBook = updatedBook;
        }

        public Book UpdatedBook { get; set; }
    }
}
