using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Post.Query.Domain.Entities;

[Table("Comments")]
public class CommentEntity
{
    [Key]
    public Guid CommentId { get; set; }
    public string UserName { get; set; }

    public string Comment { get; set; }
    public DateTime Created_At { get; set; }
    public DateTime Modified_At { get; set; }

    public bool IsEdited { get; set; }

    public Guid PostId { get; set; }

    [System.Text.Json.Serialization.JsonIgnore]
    public virtual PostEntity Post { get; set; }
}
