using CQRS.Core.Commands;

namespace CQRS.Core.Infrastructure;
public interface ICommandDispatcher
{
    void RegisterHandler<TCommand>(Func<TCommand,Task> handler) where TCommand : CommandBase;


    Task SendAsync(CommandBase command);


    // or Task SendAsync<TCommand>(TCommand command) where TCommand : CommandBase;
}
