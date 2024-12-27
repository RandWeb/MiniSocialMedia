using CQRS.Core.Messages;

namespace CQRS.Core.Commands;
public abstract record CommandBase() : MessageBase(Guid.NewGuid());
