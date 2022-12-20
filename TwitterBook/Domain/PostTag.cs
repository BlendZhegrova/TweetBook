using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace TwitterBook.Domain;

public class PostTag
{
    [Key]
    public int Id { get; set; }
    [ForeignKey(nameof(TagName))]
    public virtual Tags Tags { get; set; }
    public string TagName { get; set; }
    public virtual Post Post { get; set; }
    public Guid PostId { get; set; }
}