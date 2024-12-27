using CQRS.Core.Commands;

namespace Post.Cmd.Api.Commands;

public record AddPostCommand(string Author, string Message) : CommandBase;

