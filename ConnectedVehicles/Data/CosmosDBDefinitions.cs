using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ConnectedVehicles.Models.CosmosDB;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using MvcControlsToolkit.Business.DocumentDB;

namespace ConnectedVehicles.Data
{
    public class CosmosDBDefinitions
    {
        private static string accountURI = "https://connectedvehiclesdb.documents.azure.com:443/";
        private static string accountKey = "aPuaVySJ4piErT4e0MBOjL1dLHJpF3fIpF4SSzMDjDlJ4bhTK7qfHzKizmOGasiI0NznpenSeLU0LbfDsvcU6A==";
        public static string DatabaseId { get; private set; } = "ConnectedVehiclesList";
        public static string ConnectedVehiclesId { get; private set; } = "ConnectedVehicleItems";

        public static IDocumentDBConnection GetConnection()
        {
            return new DefaultDocumentDBConnection(accountURI, accountKey, DatabaseId);
        }

        public static async Task Initialize()
        {
            var connection = GetConnection();

            await connection.Client
                .CreateDatabaseIfNotExistsAsync(
                    new Database { Id = DatabaseId });

            DocumentCollection myCollection = new DocumentCollection();
            myCollection.Id = ConnectedVehiclesId;
            myCollection.IndexingPolicy =
               new IndexingPolicy(new RangeIndex(DataType.String)
               { Precision = -1 });
            myCollection.IndexingPolicy.IndexingMode = IndexingMode.Consistent;
            var res = await connection.Client.CreateDocumentCollectionIfNotExistsAsync(
                UriFactory.CreateDatabaseUri(DatabaseId),
                myCollection);
            if (res.StatusCode == System.Net.HttpStatusCode.Created)
                await InitData(connection);
        }
        private static async Task InitData(IDocumentDBConnection connection)
        {
            //Populating Database
            List<ConnectedVehicleItem> InitialDBItems = new List<ConnectedVehicleItem>();
                //Populating Customer Data
                var cr = new ConnectedVehicleItem();
                cr.Id = Guid.NewGuid().ToString();
                cr.Customer = "Kalles Grustransporter AB";
                cr.Address = "Cementvägen 8, 111 11 Södertälje";
                
                    //Populating Vehicles
                    var innerlList = new List<Vehicle>();
                    innerlList.Add(new Vehicle
                    {
                        Vin = "YS2R4X20005399401",
                        RegNum = "ABC123",
                        Status = "Unknown"
                    });
                    innerlList.Add(new Vehicle
                    {
                        Vin = "VLUR4X20009093588",
                        RegNum = "DEF456",
                        Status = "Unknown"
                    });
                    innerlList.Add(new Vehicle
                    {
                        Vin = "VLUR4X20009048066",
                        RegNum = "GHI789",
                        Status = "Unknown"
                    });
            cr.Vehicles = innerlList;

            //Adding Customer Document to Database
            InitialDBItems.Add(cr);
            cr = new ConnectedVehicleItem();
            //Cleaning InnerList to avoid duplication of vehicles for next Customer
            
            innerlList = new List<Vehicle>();

            cr.Id = Guid.NewGuid().ToString();
            cr.Customer = "Johans Bulk AB";
            cr.Address = "Balkvägen 12, 222 22 Stockholm";

            //Populating Vehicles
            innerlList.Add(new Vehicle
            {
                Vin = "YS2R4X20005388011",
                RegNum = "JKL012",
                Status = "Unknown"
            });
            innerlList.Add(new Vehicle
            {
                Vin = "YS2R4X20005387949",
                RegNum = "MNO345",
                Status = "Unknown"
            });
            
            cr.Vehicles = innerlList;

            //Adding Customer Documents to Database 
            InitialDBItems.Add(cr);
            cr = new ConnectedVehicleItem();
            //Cleaning InnerList to avoid duplication of vehicles for next Customer
           
            innerlList = new List<Vehicle>();

            cr.Id = Guid.NewGuid().ToString();
            cr.Customer = "Haralds Värdetransporter AB";
            cr.Address = "Budgetvägen 1, 333 33 Uppsala";

            //Populating Vehicles
            innerlList.Add(new Vehicle
            {
                Vin = "VLUR4X20009048066",
                RegNum = "PQR678",
                Status = "Unknown"
            });
            innerlList.Add(new Vehicle
            {
                Vin = "YS2R4X20005387055",
                RegNum = "STU901",
                Status = "Unknown"
            });

            cr.Vehicles = innerlList;

            //Adding Customer Documents to Database 
            InitialDBItems.Add(cr);

            foreach (var item in InitialDBItems)
            {
                await connection.Client
                    .CreateDocumentAsync(
                    UriFactory
                        .CreateDocumentCollectionUri(
                            DatabaseId, ConnectedVehiclesId),
                    item);

            }
        }
    }
}
