using CQRS.Core.Messages;

namespace CQRS.Core.Commands;
public abstract record CommandBase : MessageBase
{
    protected CommandBase() : base()
    {

    }
    protected CommandBase(Guid id) : base(id)
    {
    }
}
