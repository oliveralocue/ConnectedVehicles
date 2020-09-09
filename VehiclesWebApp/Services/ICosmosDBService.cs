using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VehiclesWebApp.Models;

namespace VehiclesWebApp.Services
{
    public interface ICosmosDbService
    {
        Task<IEnumerable<ConnectedVehicles>> GetItemsAsync(string query);
        Task<ConnectedVehicles> GetItemAsync(string id);
        Task AddItemAsync(ConnectedVehicles item);
        Task UpdateItemAsync(string id, ConnectedVehicles item);
        Task DeleteItemAsync(string id);
    }
}
