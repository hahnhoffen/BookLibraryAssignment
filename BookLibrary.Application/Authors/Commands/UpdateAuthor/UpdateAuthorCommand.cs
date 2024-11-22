using MediatR;
using BookLibrary.Domain.Entities;
namespace BookLibrary.Application.Authors.Commands.UpdateAuthor
{
    public class UpdateAuthorCommand : IRequest<List<Author>>
    {
        public UpdateAuthorCommand(Author updatedAuthor)
        {
            UpdatedAuthor = updatedAuthor;
        }
        public Author UpdatedAuthor { get; set; }
    }
}
