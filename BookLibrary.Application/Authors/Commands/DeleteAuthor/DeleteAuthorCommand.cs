using MediatR;
using BookLibrary.Domain.Entities;

namespace BookLibrary.Application.Authors.Commands.DeleteAuthor
{
    public class DeleteAuthorCommand : IRequest<List<Author>>
    {
        public DeleteAuthorCommand(Guid authorId)
        {
            AuthorId = authorId;
        }

        public Guid AuthorId { get; set; }
    }
}
