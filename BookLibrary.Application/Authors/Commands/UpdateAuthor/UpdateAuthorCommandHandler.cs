using MediatR;
using BookLibrary.Domain.Entities;
using BookLibrary.Infrastructure.DataBase;

namespace BookLibrary.Application.Authors.Commands.UpdateAuthor
{
    public class UpdateAuthorCommandHandler : IRequestHandler<UpdateAuthorCommand, List<Author>>
    {
        private readonly FakeDatabase _fakeDatabase;

        public UpdateAuthorCommandHandler(FakeDatabase fakeDatabase)
        {
            _fakeDatabase = fakeDatabase;
        }
        public Task<List<Author>> Handle(UpdateAuthorCommand request, CancellationToken cancellationToken)
        {
            var author = _fakeDatabase.Authors.FirstOrDefault(a => a.Id == request.UpdatedAuthor.Id);
            if (author == null)
                throw new KeyNotFoundException("Author not found.");

            author.Name = request.UpdatedAuthor.Name;

            return Task.FromResult(_fakeDatabase.Authors);
        }
    }
}
