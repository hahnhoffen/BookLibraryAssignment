using MediatR;
using BookLibrary.Domain.Entities;
using System.Collections.Generic;

namespace BookLibrary.Application.Books.Commands.UpdateBook
{
    public class UpdateBookCommand : IRequest<List<Book>>
    {
        public UpdateBookCommand(Book updatedBook)
        {
            UpdatedBook = updatedBook;
        }

        public Book UpdatedBook { get; set; }
    }
}
