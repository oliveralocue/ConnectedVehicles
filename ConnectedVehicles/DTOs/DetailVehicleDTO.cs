using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConnectedVehicles.DTOs
{
    public class DetailVehicleDTO
    {
        public string Id { get; set; }
        
        public string Customer { get; set; }

        public string Address { get; set; }
        
        public IEnumerable<VehicleDTO> Vehicles { get; set; }
    }

    public class VehicleDTO
    {
        public string Vin { get; set; }

        public string RegNum { get; set; }

       public string Status { get; set; }
    }

}
