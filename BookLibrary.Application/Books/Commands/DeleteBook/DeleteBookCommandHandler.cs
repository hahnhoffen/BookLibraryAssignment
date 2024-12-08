using MediatR;
using BookLibrary.Domain.Entities;
using BookLibrary.Domain.Interface;
using BookLibrary.Application.Common;

namespace BookLibrary.Application.Books.Commands.DeleteBook
{
    public class DeleteBookCommandHandler : IRequestHandler<DeleteBookCommand, OperationResult<List<Book>>>
    {
        private readonly IBookRepository _bookRepository;

        public DeleteBookCommandHandler(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        public async Task<OperationResult<List<Book>>> Handle(DeleteBookCommand request, CancellationToken cancellationToken)
        {
            var book = await _bookRepository.GetByIdAsync(request.BookId);
            if (book == null)
            {
                return OperationResult<List<Book>>.FailureResult("Book not found.");
            }

            await _bookRepository.DeleteAsync(request.BookId);

            var books = await _bookRepository.GetAllAsync();
            return OperationResult<List<Book>>.SuccessResult(books.ToList(), "Book successfully deleted.");
        }
    }
}
