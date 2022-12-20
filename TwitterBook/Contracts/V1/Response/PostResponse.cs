using TwitterBook.Controllers.V1;

namespace TwitterBook.Contracts.V1.Response
{
    public class PostResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string UserId { get; set; }
        public IEnumerable<TagResponse> Tags { get; set; }
    }
}
