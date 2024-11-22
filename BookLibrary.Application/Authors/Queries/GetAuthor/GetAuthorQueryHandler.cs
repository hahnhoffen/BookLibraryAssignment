using MediatR;
using BookLibrary.Domain.Entities;
using BookLibrary.Infrastructure.DataBase;

namespace BookLibrary.Application.Authors.Queries.GetAuthor
{
    public class GetAuthorQueryHandler : IRequestHandler<GetAuthorQuery, Author>
    {
        private readonly FakeDatabase _fakeDatabase;

        public GetAuthorQueryHandler(FakeDatabase fakeDatabase)
        {
            _fakeDatabase = fakeDatabase;
        }

        public Task<Author> Handle(GetAuthorQuery request, CancellationToken cancellationToken)
        {
            // Find the author by AuthorId
            var author = _fakeDatabase.Authors.FirstOrDefault(author => author.Id == request.AuthorId);
            if (author == null)
            {
                throw new ArgumentException($"Author with ID {request.AuthorId} not found.");
            }

            return Task.FromResult(author);
        }
    }
}


