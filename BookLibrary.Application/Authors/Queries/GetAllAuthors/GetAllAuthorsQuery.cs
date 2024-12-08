using MediatR;
using BookLibrary.Domain.Entities;
using BookLibrary.Application.Common;

namespace BookLibrary.Application.Authors.Queries.GetAllAuthors
{
    public class GetAllAuthorsQuery : IRequest<OperationResult<List<Author>>>
    {
    }
}
