using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Fluent;
using Microsoft.Extensions.Configuration;
using VehiclesWebApp.Models;
using VehiclesWebApp.Services;

namespace VehiclesWebApp.Services
{
    
    public class CosmosDbService : ICosmosDbService
    {
        private Container _container;

        public CosmosDbService(
            CosmosClient dbClient,
            string databaseName,
            string containerName)
        {
            this._container = dbClient.GetContainer(databaseName, containerName);
        }

        public async Task AddItemAsync(ConnectedVehicles item)
        {
            await this._container.CreateItemAsync<ConnectedVehicles>(item, new PartitionKey(item.Id));
        }

        public async Task DeleteItemAsync(string id)
        {
            await this._container.DeleteItemAsync<ConnectedVehicles>(id, new PartitionKey(id));
        }

        public async Task<ConnectedVehicles> GetItemAsync(string id)
        {
            try
            {
                ItemResponse<ConnectedVehicles> response = await this._container.ReadItemAsync<ConnectedVehicles>(id, new PartitionKey(id));
                return response.Resource;
            }
            catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return null;
            }

        }

        public async Task<IEnumerable<ConnectedVehicles>> GetItemsAsync(string queryString)
        {
            var query = this._container.GetItemQueryIterator<ConnectedVehicles>(new QueryDefinition(queryString));
            List<ConnectedVehicles> results = new List<ConnectedVehicles>();
            while (query.HasMoreResults)
            {
                var response = await query.ReadNextAsync();

                results.AddRange(response.ToList());
            }

            return results;
        }

        public async Task UpdateItemAsync(string id, ConnectedVehicles item)
        {
            await this._container.UpsertItemAsync<ConnectedVehicles>(item, new PartitionKey(id));
        }
    }
}
