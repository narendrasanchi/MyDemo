using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using Common;

namespace AWSDemo.Common.Contract
{
    public interface IDynamoDbRepository<T>
    {
        Task<Response<T?>> Get(string id, DynamoDBOperationConfig dynamoDbOperationConfig = null);
        Task<Response<List<T>>> FromQueryAsync(QueryOperationConfig queryOperationConfig, DynamoDBOperationConfig dynamoDbOperationConfig = null);

        Task<Response<bool>> Save(T item, DynamoDBOperationConfig dynamoDbOperationConfig = null);
        Task<Response<List<T>>> Scan(DynamoDBOperationConfig dynamoDbOperationConfig);
        Task<Response<bool>> Remove(string id, string sortkey, DynamoDBOperationConfig dynamoDbOperationConfig = null);
    }
}
