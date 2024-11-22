using MediatR;
using BookLibrary.Domain.Entities;

namespace BookLibrary.Application.Authors.Queries.GetAuthor
{
    public class GetAuthorQuery : IRequest<Author>
    {
        public GetAuthorQuery(Guid authorId)
        {
            AuthorId = authorId;
        }
        public Guid AuthorId { get; set; }
    }
}
