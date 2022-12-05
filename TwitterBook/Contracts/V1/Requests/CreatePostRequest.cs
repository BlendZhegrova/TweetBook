using TwitterBook.Controllers.V1;

namespace TwitterBook.Contracts.V1.Requests
{
    public class CreatePostRequest
    {
        public string Name { get; set; }
        public List<string> TagsName { get; set; }

    }
}
