using MediatR;
using BookLibrary.Domain.Entities;
using BookLibrary.Domain.Interface;

namespace BookLibrary.Application.Books.Commands.AddBook
{
    public class AddBookCommandHandler : IRequestHandler<AddBookCommand, List<Book>>
    {
        private readonly IBookRepository _bookRepository;

        public AddBookCommandHandler(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        public async Task<List<Book>> Handle(AddBookCommand request, CancellationToken cancellationToken)
        {
            // Check for duplicate books by title
            var books = await _bookRepository.GetAllAsync();
            if (books.Any(book => book.Title == request.NewBook.Title))
                throw new Exception("A book with this title already exists.");

            await _bookRepository.AddAsync(request.NewBook);

            books = await _bookRepository.GetAllAsync();
            return books.ToList();
        }
    }
}
