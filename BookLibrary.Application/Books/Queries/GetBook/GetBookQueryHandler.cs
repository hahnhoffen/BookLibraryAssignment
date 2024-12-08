using MediatR;
using BookLibrary.Domain.Entities;
using BookLibrary.Domain.Interface;
using BookLibrary.Application.Books.Queries.GetBook;
using BookLibrary.Application.Common;

public class GetBookQueryHandler : IRequestHandler<GetBookQuery, OperationResult<Book>>
{
    private readonly IBookRepository _bookRepository;

    public GetBookQueryHandler(IBookRepository bookRepository)
    {
        _bookRepository = bookRepository;
    }

    public async Task<OperationResult<Book>> Handle(GetBookQuery request, CancellationToken cancellationToken)
    {
        var book = await _bookRepository.GetByIdAsync(request.BookId);
        if (book == null)
        {
            return OperationResult<Book>.FailureResult("Book not found.");
        }

        return OperationResult<Book>.SuccessResult(book, "Book retrieved successfully.");
    }
}
