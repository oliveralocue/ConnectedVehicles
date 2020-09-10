using System;
using System.Threading.Tasks;
using System.Configuration;
using System.Collections.Generic;
using System.Net;
using System.Timers;
using Microsoft.Azure.Cosmos;

namespace VehicleStatusUpdater
{
    class Program
    {
        // The primary key for the Azure Cosmos DB account.
        private static readonly string EndpointUri = ConfigurationManager.AppSettings["EndPointUri"];
        private static readonly string PrimaryKey = ConfigurationManager.AppSettings["PrimaryKey"];
        
        private CosmosClient cosmosClient;
        private Database database;
        private Container container;
        //Database info
        private string databaseId = "ConnectedVehiclesList";
        private string containerId = "ConnectedVehicleItems";
        public double rus,plusrus = 0;
        public static async Task Main(string[] args)
        {

            try
            {
                Console.WriteLine("Starting Vehicle Status Simulation...\n");
                Program p = new Program();
                await p.DBMethods();

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
            finally
            {
                Console.WriteLine("Connected to Database");
                Console.ReadKey();
            }
        }

        public async Task DBMethods()
        {
            // Create a new instance of the Cosmos Client
            this.cosmosClient = new CosmosClient(EndpointUri, PrimaryKey);
            this.database = await this.cosmosClient.CreateDatabaseIfNotExistsAsync(databaseId);
            this.container = await this.database.CreateContainerIfNotExistsAsync(containerId, "/Customer");
            
            while (true)
            { 
                await this.ReplaceVehicleStatusItemAsync();
                Console.WriteLine("Vehicles Status update consumed DB RUs: {0}", rus);
                Console.WriteLine("Total DB RUs Used so far: {0}", plusrus);
                Console.WriteLine("Changing Vehicles status again in 10 seconds...\n\n\n");
                await Task.Delay(10000);

            }
            

        }
        

        private async Task ReplaceVehicleStatusItemAsync()
        {
            //Querying the clients in the Database
            var sqlQueryText = "SELECT * FROM c";

            Console.WriteLine("Loading Database data...\n");

            QueryDefinition queryDefinition = new QueryDefinition(sqlQueryText);
            FeedIterator<ConnectedVehicles> queryResultSetIterator = this.container.GetItemQueryIterator<ConnectedVehicles>(queryDefinition);

            List<ConnectedVehicles> customers = new List<ConnectedVehicles>();

            while (queryResultSetIterator.HasMoreResults)
            {
                FeedResponse<ConnectedVehicles> currentResultSet = await queryResultSetIterator.ReadNextAsync();
                foreach (ConnectedVehicles customer in currentResultSet)
                {
                    customers.Add(customer);
                }
            }

            for(int c=0; c <customers.Count; c++)
            {
                ItemResponse<ConnectedVehicles> customerdata = await this.container.ReadItemAsync<ConnectedVehicles>(customers[c].Id, new PartitionKey(customers[c].Customer));
                Random rand = new Random();
                var itemBody = customerdata.Resource;
                Console.WriteLine("Updating Customer [ID: {0}, Name: {1}]\t", itemBody.Id, itemBody.Customer);
                for (int i = 0; i < itemBody.Vehicles.Length; i++)
                {
                    Vehicle veh = itemBody.Vehicles[i];
                    int num = rand.Next(1000);
                    int mod = 0;
                    mod = num % 3;
                    
                    // Vehicle Status Update based on Remainder op result
                    if (mod == 1)
                    {
                        veh.Status = "Vehicle Disconnected";

                    }
                    else
                    {
                        veh.Status = "Vehicle Connected";
                    }
                    
                    Console.WriteLine("VIN: {0} \t RegNum: {1}\t Status: {2}\t", veh.Vin, veh.RegNum, veh.Status);
                }
                Console.WriteLine("\n");
                customerdata = await this.container.ReplaceItemAsync<ConnectedVehicles>(itemBody, itemBody.Id, new PartitionKey(itemBody.Customer));
                rus=customerdata.RequestCharge;
               
            }
            plusrus = plusrus + rus;


        }
    }
}
