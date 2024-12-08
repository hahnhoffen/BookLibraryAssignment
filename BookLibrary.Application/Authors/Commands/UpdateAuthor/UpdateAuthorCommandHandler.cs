using BookLibrary.Application.Authors.Commands.UpdateAuthor;
using BookLibrary.Application.Common;
using BookLibrary.Domain.Entities;
using BookLibrary.Domain.Interface;
using MediatR;

public class UpdateAuthorCommandHandler : IRequestHandler<UpdateAuthorCommand, OperationResult<List<Author>>>
{
    private readonly IAuthorRepository _authorRepository;

    public UpdateAuthorCommandHandler(IAuthorRepository authorRepository)
    {
        _authorRepository = authorRepository;
    }

    public async Task<OperationResult<List<Author>>> Handle(UpdateAuthorCommand request, CancellationToken cancellationToken)
    {
        var author = await _authorRepository.GetByIdAsync(request.UpdatedAuthor.Id);
        if (author == null)
        {
            return OperationResult<List<Author>>.FailureResult("Author not found.");
        }

        // Update the author's properties
        author.Name = request.UpdatedAuthor.Name;

        // Save changes
        await _authorRepository.AddAsync(author);

        // Return the updated list of authors
        var authors = await _authorRepository.GetAllAsync();
        return OperationResult<List<Author>>.SuccessResult(authors.ToList(), "Author successfully updated.");
    }
}

