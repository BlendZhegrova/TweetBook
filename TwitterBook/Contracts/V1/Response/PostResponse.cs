using TwitterBook.Controllers.V1;

namespace TwitterBook.Contracts.V1.Response
{
    public class PostResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string userId { get; set; }
        public List<Tags> Tags { get; set; }
    }
}
