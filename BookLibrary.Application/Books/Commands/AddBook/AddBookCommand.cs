using MediatR;
using BookLibrary.Domain.Entities;
using BookLibrary.Application.Common;

namespace BookLibrary.Application.Books.Commands.AddBook
{
    public class AddBookCommand : IRequest<OperationResult<List<Book>>>
    {
        public AddBookCommand(Book bookToAdd)
        {
            NewBook = bookToAdd;
        }
        public Book NewBook { get; set; }
    }
}
