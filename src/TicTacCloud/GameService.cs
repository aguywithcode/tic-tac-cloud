using System;
using System.Threading.Tasks;
using System.Configuration;
using System.Collections.Generic;
using System.Net;
using Microsoft.Azure.Cosmos;

namespace TicTacCloud
{
    public class GameService
    {

        /// The Azure Cosmos DB endpoint for running this GetStarted sample.
        private string EndpointUrl = "https://localhost:8081";

        /// The primary key for the Azure DocumentDB account.
        private string PrimaryKey = 
            "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbI"+
            "ZnqyMsEcaGQy67XIw/Jw==";

        // The name of the database and container we will create
        private string databaseId = "TicTacCloudDb";
        private string containerId = "Games";

        // The Cosmos client instance
        private CosmosClient cosmosClient;

        // The database we will create
        private Database database;

        // The container we will create.
        private Container container;

        public async Task GetStartedDemoAsync()
        {
            try
            {
                cosmosClient = new CosmosClient(EndpointUrl, PrimaryKey);
                await CreateDatabaseAsync();
                await CreateContainerAsync();
                await AddItemsToContainerAsync();
                await QueryItemsAsync();
            }
            catch (CosmosException de)
            {
                Exception baseException = de.GetBaseException();
                Console.WriteLine("{0} error occurred: {1}", de.StatusCode, de);
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: {0}", e);
            }
        }

        public async Task CreateDatabaseAsync()
        {
            // Create a new database
            database = await cosmosClient.CreateDatabaseIfNotExistsAsync(databaseId);
            Console.WriteLine("Created Database: {0}\n", database.Id);
        }

        /// Create the container if it does not exist. 
        /// Specifiy "/LastName" as the partition key since we're storing family information, to 
        /// ensure good distribution of requests and storage.
         public async Task CreateContainerAsync()
         {
             // Create a new container
             container = 
                 await database.CreateContainerIfNotExistsAsync(containerId, "/GameId");
             Console.WriteLine("Created Container: {0}\n", container.Id);
         }

         public async Task AddItemsToContainerAsync()
         {
             var game = new Game();
             try
             {
                 // Create an item in the container representing the game. Note we provide the
                 // value of the partition key for this item, which is the GameId.
                 var gameResponse = 
                     await container.CreateItemAsync(game, new PartitionKey(game.GameId));
                 // Note that after creating the item, we can access the body of the item with the
                 // Resource property of the ItemResponse. We can also access the RequestCharge
                 // property to see the amount of RUs consumed on this request.
                 Console.WriteLine(
                         $"Created item in database with id: {gameResponse.Resource.GameId}");
                 Console.WriteLine(
                         $"Operation consumed {gameResponse.RequestCharge} RUs.");
             }
             catch (CosmosException ex) when (ex.StatusCode == HttpStatusCode.Conflict)
             {
                 Console.WriteLine("Item in database with id: {0} already exists\n", game.GameId);
             }
         }

         public async Task QueryItemsAsync()
         {
             var sqlQueryText = "SELECT * FROM c ";

             Console.WriteLine($"Running query: {sqlQueryText}");

             QueryDefinition queryDefinition = new QueryDefinition(sqlQueryText);
             var queryResultSetIterator = container.GetItemQueryIterator<Game>(queryDefinition);

             var games = new List<Game>();

             while (queryResultSetIterator.HasMoreResults)
             {
                 FeedResponse<Game> currentResultSet = await queryResultSetIterator.ReadNextAsync();
                 foreach (Game game in currentResultSet)
                 {
                     games.Add(game);
                     Console.WriteLine("\tRead {0}\n", game.State);
                 }
             }
         }

         public async Task DeleteDatabaseAndCleanupAsync()
         {
             DatabaseResponse databaseResourceResponse = await database.DeleteAsync();

             Console.WriteLine("Deleted Database: {0}\n", databaseId);

             //Dispose of CosmosClient
             cosmosClient.Dispose();
         }
    }
}
