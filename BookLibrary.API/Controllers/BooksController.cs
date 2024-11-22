using MediatR;
using Microsoft.AspNetCore.Mvc;
using BookLibrary.Application.Books.Commands.AddBook;
using BookLibrary.Application.Books.Commands.UpdateBook;
using BookLibrary.Application.Books.Commands.DeleteBook;
using BookLibrary.Application.Books.Queries.GetBook;
using BookLibrary.Application.Books.Queries.GetAllBooks;
using BookLibrary.Domain.Entities;

namespace BookLibrary.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BooksController : ControllerBase
    {
        private readonly IMediator _mediator;

        public BooksController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> AddBook([FromBody] Book book)
        {
            var command = new AddBookCommand(book);
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBook(Guid id, [FromBody] Book book)
        {
            book.Id = id;
            var command = new UpdateBookCommand(book);
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook(Guid id)
        {
            var command = new DeleteBookCommand(id);
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBook(Guid id)
        {
            var query = new GetBookQuery(id);
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllBooks()
        {
            var query = new GetAllBooksQuery();
            var result = await _mediator.Send(query);
            return Ok(result);
        }
    }
}
