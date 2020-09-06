using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MvcControlsToolkit.Core.DataAnnotations;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Identity;

namespace ConnectedVehicles.Models.CosmosDB
{

    public class ConnectedVehicleItem
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "customer")]
        public string Customer { get; set; }

        [JsonProperty(PropertyName = "address")]
        public string Address { get; set; }
        //if there is an error try changing CollectionKey value from Vin to vin
        [JsonProperty(PropertyName = "vehicles"),
            CollectionKey("Vin")]
        public IEnumerable<Vehicle> Vehicles { get; set; }


    }

    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
    }
}