using Amazon.DynamoDBv2.DataModel;

namespace AWSDemo.Entities
{
    public class Book
    {
        [DynamoDBHashKey("id")]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        [DynamoDBProperty("title")]
        public string? Title { get; set; }
        [DynamoDBProperty("published_year")]
        public int PublishedYear { get; set; }
        [DynamoDBProperty("pages")]
        public int Pages { get; set; }
        [DynamoDBProperty("summary")]
        public string? Summary { get; set; }
    }
}
