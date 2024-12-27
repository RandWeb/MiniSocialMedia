using CQRS.Core.Commands;

namespace Post.Cmd.Api.Commands;

public record UpdateMessageCommand(string Message) : CommandBase;

