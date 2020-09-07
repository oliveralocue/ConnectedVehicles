using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ConnectedVehicles.Data;
using ConnectedVehicles.DTOs;
using ConnectedVehicles.Models.CosmosDB;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using MvcControlsToolkit.Business.DocumentDB;
using MvcControlsToolkit.Core.Business.Utilities;


namespace ConnectedVehicles.Repository
{
    public class VehiclesRepository: DocumentDBCRUDRepository<ConnectedVehicleItem>
    {
        public VehiclesRepository(
            IDocumentDBConnection connection
            
            ) : base(connection,
                CosmosDBDefinitions.ConnectedVehiclesId)
        {
            
        }

        static VehiclesRepository()
        {
            DeclareProjection
                (m =>
                new DetailVehicleDTO
                {

                    Vehicles = m.Vehicles
                        .Select(l => new VehicleDTO { })
                }, m => m.Id
            );
            DeclareProjection
                (m =>
                new VehicleDTO
                {


                }, m => m.Vin
            );


        }
        public async Task<DataPage<DetailVehicleDTO>> GetAllItems()
        {
            var vm = await GetPage<DetailVehicleDTO>
                (null,
                 x => x.OrderBy(m => m.Id),
                 -1, 100);
            return vm;
        }

        


    }
}
