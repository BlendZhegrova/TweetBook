using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace TwitterBook.Domain;

public class PostTag
{
    [Key]
    public int Id { get; set; }
    public string TagName { get; set; }
    public Guid PostId { get; set; }
    [ForeignKey(nameof(PostId))]
    public Post Post { get; set; }
}