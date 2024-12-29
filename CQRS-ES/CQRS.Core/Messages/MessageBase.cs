namespace CQRS.Core.Messages;
public abstract record MessageBase
{
    public Guid Id { get; set; }
    protected MessageBase(Guid id)
    {
        Id = id;
    }
    protected MessageBase()
    {
        
    }

}
