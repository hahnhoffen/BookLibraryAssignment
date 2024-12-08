using MediatR;
using BookLibrary.Domain.Entities;
using BookLibrary.Application.Common;

namespace BookLibrary.Application.Authors.Commands.DeleteAuthor
{
    public class DeleteAuthorCommand : IRequest<OperationResult<List<Author>>>
    {
        public DeleteAuthorCommand(Guid authorId)
        {
            AuthorId = authorId;
        }

        public Guid AuthorId { get; set; }
    }
}
