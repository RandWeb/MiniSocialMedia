using CQRS.Core.Commands;

namespace Post.Cmd.Api.Commands;

public record RemovePostCommand(string UserName) : CommandBase;

