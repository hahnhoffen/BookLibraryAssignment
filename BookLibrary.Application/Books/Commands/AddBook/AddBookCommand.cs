using MediatR;
using BookLibrary.Domain.Entities;

namespace BookLibrary.Application.Books.Commands.AddBook
{
    public class AddBookCommand : IRequest<List<Book>>
    {
        public AddBookCommand(Book bookToAdd)
        {
            NewBook = bookToAdd;
        }
        public Book NewBook { get; set; }
    }
}
