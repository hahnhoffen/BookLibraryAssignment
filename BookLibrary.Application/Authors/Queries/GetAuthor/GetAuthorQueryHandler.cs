using MediatR;
using BookLibrary.Domain.Entities;
using BookLibrary.Domain.Interface;

namespace BookLibrary.Application.Authors.Queries.GetAuthor
{
    public class GetAuthorQueryHandler : IRequestHandler<GetAuthorQuery, Author>
    {
        private readonly IAuthorRepository _authorRepository;

        public GetAuthorQueryHandler(IAuthorRepository authorRepository)
        {
            _authorRepository = authorRepository;
        }

        public async Task<Author> Handle(GetAuthorQuery request, CancellationToken cancellationToken)
        {
            var author = await _authorRepository.GetByIdAsync(request.AuthorId);
            if (author == null)
            {
                throw new ArgumentException($"Author with ID {request.AuthorId} not found.");
            }

            return author;
        }
    }
}