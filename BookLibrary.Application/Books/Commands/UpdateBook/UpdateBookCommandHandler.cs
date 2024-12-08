using MediatR;
using BookLibrary.Domain.Entities;
using BookLibrary.Domain.Interface;
using BookLibrary.Application.Common;

namespace BookLibrary.Application.Books.Commands.UpdateBook
{
    public class UpdateBookCommandHandler : IRequestHandler<UpdateBookCommand, OperationResult<List<Book>>>
    {
        private readonly IBookRepository _bookRepository;

        public UpdateBookCommandHandler(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        public async Task<OperationResult<List<Book>>> Handle(UpdateBookCommand request, CancellationToken cancellationToken)
        {
            var book = await _bookRepository.GetByIdAsync(request.UpdatedBook.Id);
            if (book == null)
            {
                return OperationResult<List<Book>>.FailureResult("Book not found.");
            }

            book.Title = request.UpdatedBook.Title;
            book.AuthorId = request.UpdatedBook.AuthorId;
            book.Year = request.UpdatedBook.Year;

            await _bookRepository.UpdateAsync(book);

            var books = await _bookRepository.GetAllAsync();
            return OperationResult<List<Book>>.SuccessResult(books.ToList(), "Book successfully updated.");
        }
    }
}
