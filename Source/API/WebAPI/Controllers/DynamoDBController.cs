using AWSDemo.Common;
using AWSDemo.Common.Contract;
using AWSDemo.DynamoDB;
using AWSDemo.Entities;
using Common;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DynamoDBController : ControllerBase
    {
        private readonly IRepostoryImplementions _dynamoDbRepository;
        public DynamoDBController(IRepostoryImplementions dynamoDbRepository)
        {
            _dynamoDbRepository = dynamoDbRepository;
        }
        private readonly ILogger<DynamoDBController> _logger;
        [HttpGet("books")]
        public async Task<Response<Book?>> BookById(string id)
        {
            return await _dynamoDbRepository.GetBookAsync(id);
        }

        [HttpGet("bookList")]
        public async Task<Response<List<Book>>> BookList()
        {
            return await _dynamoDbRepository.GetBookListAsync();
        }

        [HttpDelete("delete-book")]
        public async Task<Response<bool>> DeleteBook(string id)
        {
            return await _dynamoDbRepository.deleteBookAsync(id);
        }

        [HttpPost("savebook")]
        public async Task<Response<Book>> SaveBook(Book request)
        {
            return await _dynamoDbRepository.SaveBookAsync(request);
        }


    }
}
