using TwitterBook.Domain;

namespace TwitterBook.Contracts.V1.Requests;

public class UpdateTagRequest
{
    public string Id { get; set; }
    public string TagName { get; set; }
    public string CreatorId { get; set; }
}