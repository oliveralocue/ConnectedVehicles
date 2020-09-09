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
            
        }
        
    }
}
