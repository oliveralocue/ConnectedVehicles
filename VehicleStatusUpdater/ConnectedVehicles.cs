using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace VehicleStatusUpdater
{
    public class ConnectedVehicles
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

       
        public string Customer { get; set; }

      
        public string Address { get; set; }
        
        
        public Vehicle[] Vehicles { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }

    public class Vehicle
    {
        
        public string Vin { get; set; }

        public string RegNum { get; set; }

        public string Status { get; set; }
    }
}
