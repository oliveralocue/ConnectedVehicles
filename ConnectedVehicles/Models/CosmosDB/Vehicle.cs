using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ConnectedVehicles.Models.CosmosDB
{

    public class Vehicle
    {
        [JsonProperty(PropertyName = "vin")]
        public string Vin { get; set; }

        [JsonProperty(PropertyName = "regnum")]
        public string RegNum { get; set; }

        [JsonProperty(PropertyName = "status")]
        public string Status { get; set; }
    }
}
