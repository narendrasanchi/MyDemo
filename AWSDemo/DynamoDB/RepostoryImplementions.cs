using Amazon.DynamoDBv2.DocumentModel;
using Common.Enum;
using Common;
using AWSDemo.Entities;
using AWSDemo.Common.Contract;
using System.Security.Principal;
using AWSDemo.Common;
using Amazon.DynamoDBv2.DataModel;
using Amazon.Runtime.Internal;

namespace AWSDemo.DynamoDB
{
    public class RepostoryImplementions: IRepostoryImplementions
    {
        private readonly IDynamoDbRepository<Book> _repository;
        public static List<string> SelectedColumnsList => new List<string> { "id", "title", "published_year", "pages", "summary" };

        public RepostoryImplementions(IDynamoDbRepository<Book> repository) {
            _repository = repository;
        }
        public DynamoDBOperationConfig GetDynamoDBTableName(string dynamoDbBaseName,string environmentName)
        {
            return new DynamoDBOperationConfig() { OverrideTableName = $"{dynamoDbBaseName}{environmentName.ToLower()}" };
        }

        public async Task<Response<Book?>> GetBookAsync(string id)
        {
            //code to get data from dynamo db 
            return await _repository.Get(id, GetDynamoDBTableName("books", ""));
        }

        public async Task<Response<List<Book>>> GetBookListAsync()
        {
            return await _repository.Scan(GetDynamoDBTableName("books", ""));
        }
        public async Task<Response<IEnumerable<Book>>> GetBooksByIdAsync(string id)
        {
            QueryFilter filter = new QueryFilter("id", QueryOperator.Equal, id);

            QueryOperationConfig config = new QueryOperationConfig()
            {
                Filter = filter,
                Select = SelectValues.SpecificAttributes,
                AttributesToGet = SelectedColumnsList,
                ConsistentRead = true
            };

            var queryResponse = await _repository.FromQueryAsync(config, GetDynamoDBTableName("books",""));

            return new Response<IEnumerable<Book>>(queryResponse.Model, ResponseCode.DBFailed);
        }
        public async Task<Response<Book>> SaveBookAsync(Book request)
        {
            var book = new Book()
            {
                Id = Guid.NewGuid().ToString(),
                Pages = request.Pages,
                Title = request.Title,
                Summary = "test summary",
                PublishedYear = request.PublishedYear   
            };
             var rs = _repository.Save(book, GetDynamoDBTableName("books", ""));
            return new Response<Book>(book);
        }

        public async Task<Response<bool>> deleteBookAsync(string id)
        {
            return await _repository.Remove(id,null, GetDynamoDBTableName("books", ""));
        }
    }

    public interface IRepostoryImplementions
    {
        Task<Response<Book?>> GetBookAsync(string id);
        Task<Response<List<Book>>> GetBookListAsync();
        Task<Response<IEnumerable<Book>>> GetBooksByIdAsync(string id);
        Task<Response<Book>> SaveBookAsync(Book request);
        Task<Response<bool>> deleteBookAsync(string id);
    }
}
