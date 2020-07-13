using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;

namespace API.DataAccess
{
    public class UserDataAccess : IUserDataAccess
    {
        /// The Azure Cosmos DB endpoint for running this GetStarted sample.
        private string EndpointUrl = "https://test-api-example.documents.azure.com:443/";//Environment.GetEnvironmentVariable("EndpointUrl");

        /// The primary key for the Azure DocumentDB account.
        private string PrimaryKey = "KTaPwqRLR4p72Sr4nhsfrgqUasuBhebeghaDgOYmXLt5cRdif6CQU343Pau9WiKJqA4StcBn7UetBVaGaRmjrQ==";//Environment.GetEnvironmentVariable("PrimaryKey");

        // The Cosmos client instance
        private CosmosClient cosmosClient;

        // The database we will create
        private Database database;

        // The container we will create.
        private Container _container;

        // The name of the database and container we will create
        private string databaseId = "UserDatabase";
        private string containerId = "UserContainer";

        public UserDataAccess()
        {
            cosmosClient = new CosmosClient(EndpointUrl, PrimaryKey,
                new CosmosClientOptions()
                {
                    ApplicationRegion = Regions.WestUS,
                });

            this.database = cosmosClient.CreateDatabaseIfNotExistsAsync(databaseId).Result;

            _container = database.CreateContainerIfNotExistsAsync(containerId, "/LastName").Result;
        }


        public async Task<User> CreateAsync(User user)
        {
                user.Id = Guid.NewGuid().ToString();
                ItemResponse<User> response = await this._container.CreateItemAsync<User>(user, new PartitionKey(user.LastName));
                Console.WriteLine("Created item in database with id: {0} Operation consumed {1} RUs.\n", response.Resource.Id, response.RequestCharge);
            return user;
        }

        public async Task DeleteAsync(User user)
        { 
            var response = await _container.DeleteItemAsync<User>(user.Id, new PartitionKey(user.LastName));
        }

        public async Task<IEnumerable<User>> GetAsync(string emailAddress)
        {
            var result = new List<User>();
            QueryDefinition queryDefinition = new QueryDefinition($"select * from Users c where c.EmailAddress = '{emailAddress}'");
            //todo: figure out why with param doesn't work
            //QueryDefinition queryDefinition = new QueryDefinition($"select * from Users c where c.EmailAddress = '@email'")
                //.WithParameter("@email", emailAddress);
            FeedIterator<User> feedIterator = _container.GetItemQueryIterator<User>(
                queryDefinition);

            while (feedIterator.HasMoreResults)
            {
                foreach(var item in await feedIterator.ReadNextAsync())
                {
                    result.Add(item);
                }
            }
            return result;
        }
    }
}
