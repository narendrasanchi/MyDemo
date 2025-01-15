using Amazon.DynamoDBv2.DataModel;
using Common.Enum;
using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AWSDemo.Common.Contract;
using Amazon.Runtime;
using Amazon.DynamoDBv2.DocumentModel;

namespace AWSDemo.Common
{
    public class DyamoDbRepository<T> : IDynamoDbRepository<T>
    {
        private readonly IDynamoDBContext dynamoDBContext;
        public DyamoDbRepository(IDynamoDBContext dynamoDBContext)
        {
            this.dynamoDBContext = dynamoDBContext;
        }
        public async Task<Response<T?>> Get(string id, DynamoDBOperationConfig dynamoDbOperationConfig = null)
        {
            //dynamo db demo to get the data 
            T dbRecord;
            if (dynamoDbOperationConfig != null)
            {
                //This is for custom config.
                dbRecord = await dynamoDBContext.LoadAsync<T>(id, dynamoDbOperationConfig);
            }
            else
            {
                dbRecord = await dynamoDBContext.LoadAsync<T>(id);
            }
            return new Response<T?>(dbRecord, ResponseCode.Ok);
        }

        public async Task<Response<List<T>>> FromQueryAsync(QueryOperationConfig queryOperationConfig, DynamoDBOperationConfig dynamoDbOperationConfig = null)
        {
            List<T> dbrecord = await dynamoDBContext.FromQueryAsync<T>(queryOperationConfig, dynamoDbOperationConfig).GetRemainingAsync();
            return new Response<List<T>>(dbrecord, ResponseCode.Ok);
        }

        public async Task<Response<bool>> Save(T item, DynamoDBOperationConfig dynamoDbOperationConfig = null)
        {
            if (item != null)
            {
                await dynamoDBContext.SaveAsync(item, dynamoDbOperationConfig);
                return new Response<bool>(true);
            }
            return new Response<bool>(false);
        }
        public async Task<Response<List<T>>> Scan(DynamoDBOperationConfig dynamoDbOperationConfig)
        {
            List<T> dbrecord = await dynamoDBContext.ScanAsync<T>(dynamoDbOperationConfig.QueryFilter, dynamoDbOperationConfig).GetRemainingAsync();
            return new Response<List<T>>(dbrecord, ResponseCode.Ok);
        }
        public async Task<Response<List<T>>> Query(string id, DynamoDBOperationConfig dynamoDbOperationConfig)
        {
            List<T> dbrecord = await dynamoDBContext.QueryAsync<T>(id, dynamoDbOperationConfig).GetRemainingAsync();
            return new Response<List<T>>(dbrecord, ResponseCode.Ok);
        }

        public async Task<Response<bool>> Remove(string id, string sortkey, DynamoDBOperationConfig dynamoDbOperationConfig = null)
        {
            if (string.IsNullOrWhiteSpace(id) && string.IsNullOrWhiteSpace(sortkey))
            {
                return new Response<bool>(false);
            }

            if (dynamoDbOperationConfig != null)
                await dynamoDBContext.DeleteAsync<T>(id, sortkey, dynamoDbOperationConfig);
            else
                await dynamoDBContext.DeleteAsync<T>(id, sortkey);

            return new Response<bool>(true);
        }
    }
}
