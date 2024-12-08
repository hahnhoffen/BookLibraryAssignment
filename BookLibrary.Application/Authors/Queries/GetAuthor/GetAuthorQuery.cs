using MediatR;
using BookLibrary.Domain.Entities;
using BookLibrary.Application.Common;

namespace BookLibrary.Application.Authors.Queries.GetAuthor
{
    public class GetAuthorQuery : IRequest<OperationResult<List<Author>>>
    {
        public GetAuthorQuery(Guid authorId)
        {
            AuthorId = authorId;
        }
        public Guid AuthorId { get; set; }
    }
}
