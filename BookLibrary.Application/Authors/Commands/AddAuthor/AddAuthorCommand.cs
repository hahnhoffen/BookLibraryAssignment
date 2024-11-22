using MediatR;
using BookLibrary.Domain.Entities;

namespace BookLibrary.Application.Authors.Commands.AddAuthor
{
    public class AddAuthorCommand : IRequest<Author>
    {
        public Author NewAuthor { get; }

        public AddAuthorCommand(Author newAuthor)
        {
            NewAuthor = newAuthor;
        }
    }
}

