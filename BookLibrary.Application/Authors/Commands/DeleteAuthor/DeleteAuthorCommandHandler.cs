using MediatR;
using BookLibrary.Domain.Entities;
using BookLibrary.Domain.Interface;

namespace BookLibrary.Application.Authors.Commands.DeleteAuthor
{
    public class DeleteAuthorCommandHandler : IRequestHandler<DeleteAuthorCommand, List<Author>>
    {
        private readonly IAuthorRepository _authorRepository;

        public DeleteAuthorCommandHandler(IAuthorRepository authorRepository)
        {
            _authorRepository = authorRepository;
        }

        public async Task<List<Author>> Handle(DeleteAuthorCommand request, CancellationToken cancellationToken)
        {
            var author = await _authorRepository.GetByIdAsync(request.AuthorId);
            if (author == null)
                throw new KeyNotFoundException("Author not found.");

            await _authorRepository.DeleteAsync(request.AuthorId);

            var authors = await _authorRepository.GetAllAsync();
            return authors.ToList();
        }
    }
}