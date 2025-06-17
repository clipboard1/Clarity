using Clarity.Application.Abstractions;
using Clarity.Core.Models;
using Clarity.Persistence.Abstractions;

namespace Clarity.Application.Tags.Delete;

public record DeleteTagCommand(
    int Id)
    : ICommand;
    
public class DeleteTagCommandHandler : ICommandHandler<DeleteTagCommand>
{
    private readonly ITagRepository _repository;

    public DeleteTagCommandHandler(ITagRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result> Handle(DeleteTagCommand command, 
        CancellationToken cancellationToken = default)
    {
        var deleteResult = await _repository.Delete(command.Id, cancellationToken);
        if (!deleteResult.IsSuccess)
            return Result.Failure(deleteResult.Errors);
        
        return Result.Success();
    }
}