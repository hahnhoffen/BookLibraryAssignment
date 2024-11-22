using MediatR;
using BookLibrary.Domain.Entities;
using BookLibrary.Infrastructure.DataBase;

namespace BookLibrary.Application.Authors.Commands.DeleteAuthor
{
    public class DeleteAuthorCommandHandler : IRequestHandler<DeleteAuthorCommand, List<Author>>
    {
        private readonly FakeDatabase _fakeDatabase;

        public DeleteAuthorCommandHandler(FakeDatabase fakeDatabase)
        {
            _fakeDatabase = fakeDatabase;
        }

        public Task<List<Author>> Handle(DeleteAuthorCommand request, CancellationToken cancellationToken)
        {
            var author = _fakeDatabase.Authors.FirstOrDefault(author => author.Id == request.AuthorId);
            if (author == null)
                throw new KeyNotFoundException("Author not found.");

            _fakeDatabase.Authors.Remove(author);

            return Task.FromResult(_fakeDatabase.Authors);
        }
    }
}
