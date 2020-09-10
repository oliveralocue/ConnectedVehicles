using System;
using System.Threading.Tasks;
using System.Configuration;
using System.Collections.Generic;
using System.Net;
using Microsoft.Azure.Cosmos;

namespace DatabaseCreator
{
    class Program
    {
        private static readonly string EndpointUri = ConfigurationManager.AppSettings["EndPointUri"];
        private static readonly string PrimaryKey = ConfigurationManager.AppSettings["PrimaryKey"];

        private CosmosClient cosmosClient;
        private Database database;
        private Container container;
        //Database info
        private string databaseId = "ConnectedVehiclesList";
        private string containerId = "ConnectedVehicleItems";

        double rus, plusrus = 0;
        public static async Task Main(string[] args)
        {

            try
            {
                Console.WriteLine("Creating Database...\n");
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
                
            }
        }

        public async Task DBMethods()
        {
            // Create a new instance of the Cosmos Client
            this.cosmosClient = new CosmosClient(EndpointUri, PrimaryKey);
            this.database = await this.cosmosClient.CreateDatabaseIfNotExistsAsync(databaseId);
            this.container = await this.database.CreateContainerIfNotExistsAsync(containerId, "/Customer");
            await this.AddItemsToContainerAsync();

            Console.WriteLine("DB Has been created and consumed {0} RUs in Total", plusrus);



        }

        private async Task AddItemsToContainerAsync()
        {
            ConnectedVehicles cv1 = new ConnectedVehicles
            {
                Id = "1",
                Customer = "Kalles Grustransporter AB",
                Address = "Cementvägen 8, 111 11 Södertälje",
                Vehicles = new Vehicle[]
                {
                    new Vehicle
                    {
                        Vin = "YS2R4X20005399401",
                        RegNum = "ABC123",
                        Status = "Unknown"
                    },
                    new Vehicle
                    {
                        Vin = "VLUR4X20009093588",
                        RegNum = "DEF456",
                        Status = "Unknown"
                    },
                    new Vehicle
                    {
                        Vin = "VLUR4X20009048066",
                        RegNum = "GHI789",
                        Status = "Unknown"
                    }
                }
            };
            try
            {
                // Read the item to see if it exists.  
                ItemResponse<ConnectedVehicles> checker = await this.container.ReadItemAsync<ConnectedVehicles>(cv1.Id, new PartitionKey(cv1.Customer));
                rus = checker.RequestCharge;
                Console.WriteLine("Customer: {0} already exists in the Database\n", checker.Resource.Customer);
            }
            catch (CosmosException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
            {
                
                ItemResponse<ConnectedVehicles> writter = await this.container.CreateItemAsync<ConnectedVehicles>(cv1, new PartitionKey(cv1.Customer));
                rus = writter.RequestCharge;
                Console.WriteLine("Created Customer: {0} Operation consumed {1} RUs.\n", writter.Resource.Customer, writter.RequestCharge);
            }
            plusrus = plusrus + rus;
            ConnectedVehicles cv2 = new ConnectedVehicles
            {
                Id = "2",
                Customer = "Johans Bulk AB",
                Address = "Balkvägen 12, 222 22 Stockholm",
                Vehicles = new Vehicle[]
                {
                    new Vehicle
                    {
                        Vin = "YS2R4X20005388011",
                        RegNum = "JKL012",
                        Status = "Unknown"
                    },
                    new Vehicle
                    {
                        Vin = "YS2R4X20005387949",
                        RegNum = "MNO345",
                        Status = "Unknown"
                    }
                }
            };
            try
            {
                // Read the item to see if it exists.  
                ItemResponse<ConnectedVehicles> checker = await this.container.ReadItemAsync<ConnectedVehicles>(cv2.Id, new PartitionKey(cv2.Customer));
                rus = checker.RequestCharge;
                Console.WriteLine("Customer: {0} already exists in the Database\n", checker.Resource.Customer);
            }
            catch (CosmosException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
            {

                ItemResponse<ConnectedVehicles> writter = await this.container.CreateItemAsync<ConnectedVehicles>(cv2, new PartitionKey(cv2.Customer));
                rus = writter.RequestCharge;
                Console.WriteLine("Created Customer: {0} Operation consumed {1} RUs.\n", writter.Resource.Id, writter.RequestCharge);
            }

            plusrus = plusrus + rus;

            ConnectedVehicles cv3 = new ConnectedVehicles
            {
                Id = "3",
                Customer = "Haralds Värdetransporter AB",
                Address = "Budgetvägen 1, 333 33 Uppsala",
                Vehicles = new Vehicle[]
                {
                    new Vehicle
                    {
                        Vin = "VLUR4X20009048066",
                        RegNum = "PQR678",
                        Status = "Unknown"
                    },
                    new Vehicle
                    {
                        Vin = "YS2R4X20005387055",
                        RegNum = "STU901",
                        Status = "Unknown"
                    }
                }
            };
            try
            {
                // Read the item to see if it exists.  
                ItemResponse<ConnectedVehicles> checker = await this.container.ReadItemAsync<ConnectedVehicles>(cv3.Id, new PartitionKey(cv3.Customer));
                rus = checker.RequestCharge;
                Console.WriteLine("Customer: {0} already exists in the Database\n", checker.Resource.Customer);
            }
            catch (CosmosException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
            {

                ItemResponse<ConnectedVehicles> writter = await this.container.CreateItemAsync<ConnectedVehicles>(cv3, new PartitionKey(cv3.Customer));
                rus = writter.RequestCharge;
                Console.WriteLine("Created Customer: {0} Operation consumed {1} RUs.\n", writter.Resource.Id, writter.RequestCharge);
            }
            plusrus = plusrus + rus;

        }// The primary key for the Azure Cosmos DB account.
    }
}
