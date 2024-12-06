using MediatR;
using Microsoft.AspNetCore.Mvc;
using BookLibrary.Application.Authors.Commands.AddAuthor;
using BookLibrary.Application.Authors.Commands.UpdateAuthor;
using BookLibrary.Application.Authors.Commands.DeleteAuthor;
using BookLibrary.Application.Authors.Queries.GetAuthor;
using BookLibrary.Application.Authors.Queries.GetAllAuthors;
using BookLibrary.Domain.Entities;
using System;

namespace BookLibrary.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthorsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuthorsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> AddAuthor([FromBody] Author author)
        {
            try
            {
                var command = new AddAuthorCommand(author);
                var result = await _mediator.Send(command);
                return CreatedAtAction(nameof(GetAuthor), new { id = result.Id }, result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAuthor(Guid id, [FromBody] Author author)
        {
            try
            {
                author.Id = id;
                var command = new UpdateAuthorCommand(author);
                var result = await _mediator.Send(command);
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAuthor(Guid id)
        {
            try
            {
                var command = new DeleteAuthorCommand(id);
                await _mediator.Send(command);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetAuthor(Guid id)
        {
            try
            {
                var query = new GetAuthorQuery(id);
                var result = await _mediator.Send(query);
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }


        [HttpGet]
        public async Task<IActionResult> GetAllAuthors()
        {
            try
            {
                var query = new GetAllAuthorsQuery();
                var result = await _mediator.Send(query);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An unexpected error occurred.", details = ex.Message });
            }
        }

    }
}
