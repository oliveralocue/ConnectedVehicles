using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace VehiclesWebApp.Models
{
    public class ConnectedVehicles
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "customer")]
        public string Customer { get; set; }

        [JsonProperty(PropertyName = "address")]
        public string Address { get; set; }

        [JsonProperty(PropertyName = "vehicles")]
        public List<Vehicle> Vehicles { get; set; }

        
    }

    public class Vehicle
    {

        public string Vin { get; set; }

        public string RegNum { get; set; }

        public string Status { get; set; }
    }
}
