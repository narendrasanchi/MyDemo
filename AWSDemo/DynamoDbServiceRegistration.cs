using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2;
using AWSDemo.Common.Contract;
using AWSDemo.Common;
using AWSDemo.DynamoDB;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using AWSDemo.Entities;

namespace AWSDemo
{
    public static class DynamoDbServiceRegistration
    {
        public static IServiceCollection AddDynamoDbServiceRegistration(this IServiceCollection services)
        {
            services.AddTransient(typeof(IDynamoDbRepository<>), typeof(DyamoDbRepository<>));
            var config = new DynamoDBContextConfig { Conversion = DynamoDBEntryConversion.V2 };
            var region =  "us-east-1";
            var dbContext = new DynamoDBContext(new AmazonDynamoDBClient(Amazon.RegionEndpoint.GetBySystemName(region)), config);
            services.AddSingleton<IDynamoDBContext>(dbContext);
            services.AddScoped< IDynamoDbRepository <Book>, DyamoDbRepository<Book>>();
            services.AddScoped<IRepostoryImplementions, RepostoryImplementions>();
            return services;
        }
    } 
}
