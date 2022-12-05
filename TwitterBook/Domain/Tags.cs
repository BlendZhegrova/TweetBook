using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TwitterBook.Domain;

public class Tags
{
    [Key]
    public string Id { get; set; }
    public string TagName { get; set; }
    public DateTime CreatedOn { get; set; }= DateTime.UtcNow;
    public string CreatorId { get; set; }
    public string PostId { get; set; }
    [ForeignKey(nameof(PostId))]
    public Post Post { get; set; }
}