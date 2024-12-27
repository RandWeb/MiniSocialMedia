using CQRS.Core.Commands;
using CQRS.Core.Infrastructure;

namespace Post.Cmd.Infrastructure.Dispatchers;
public sealed class CommandDispatcher : ICommandDispatcher
{
    private readonly Dictionary<Type, Func<CommandBase, Task>> _handlers = [];

    public void RegisterHandler<TCommand>(Func<TCommand, Task> handler) where TCommand : CommandBase
    {
        if (_handlers.ContainsKey(typeof(TCommand)))
        {
            throw new InvalidOperationException($"Handler for command '{typeof(TCommand).Name}' was already registered.");
        }
        _handlers.TryAdd(typeof(TCommand), command => handler((TCommand)command));
    }
    public async Task SendAsync(CommandBase command)
    {
        ArgumentNullException.ThrowIfNull(command, "Command can not be null.");

        // var handler = _handlers.GetValueOrDefault(command.GetType());
        if (_handlers.TryGetValue(command.GetType(), out Func<CommandBase, Task>? handler))
        {
            await handler(command);
        }
        else
        {
            throw new InvalidOperationException($"Handler for command '{command.GetType().Name}' was not registered.");
        }
    }
}
