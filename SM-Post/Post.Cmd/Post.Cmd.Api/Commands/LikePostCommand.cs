﻿using CQRS.Core.Commands;

namespace Post.Cmd.Api.Commands;

public record LikePostCommand(Guid PostId) : CommandBase(PostId);

