using MediatR;
using BookLibrary.Domain.Entities;
using BookLibrary.Application.Common;

namespace BookLibrary.Application.Authors.Commands.AddAuthor
{
    public class AddAuthorCommand : IRequest<OperationResult<Author>>
    {
        public Author NewAuthor { get; }

        public AddAuthorCommand(Author newAuthor)
        {
            NewAuthor = newAuthor;
        }
    }
}

