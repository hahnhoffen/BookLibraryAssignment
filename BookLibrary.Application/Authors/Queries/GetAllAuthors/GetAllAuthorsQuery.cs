using MediatR;
using BookLibrary.Domain.Entities;

namespace BookLibrary.Application.Authors.Queries.GetAllAuthors
{
    public class GetAllAuthorsQuery : IRequest<List<Author>>
    {
    }
}
