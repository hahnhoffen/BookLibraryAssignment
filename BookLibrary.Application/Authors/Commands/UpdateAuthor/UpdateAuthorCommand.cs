using MediatR;
using BookLibrary.Domain.Entities;
using BookLibrary.Application.Common;
namespace BookLibrary.Application.Authors.Commands.UpdateAuthor
{
    public class UpdateAuthorCommand : IRequest<OperationResult<List<Author>>>
    {
        public UpdateAuthorCommand(Author updatedAuthor)
        {
            UpdatedAuthor = updatedAuthor;
        }
        public Author UpdatedAuthor { get; set; }
    }
}
