using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ConnectedVehicles.Data;
using ConnectedVehicles.DTOs;
using ConnectedVehicles.Models.CosmosDB;
using MvcControlsToolkit.Business.DocumentDB;
using MvcControlsToolkit.Core.Business.Utilities;


namespace ConnectedVehicles.Repository
{
    public class VehiclesRepository: DocumentDBCRUDRepository<ConnectedVehicleItem>
    {
        /*private string loggedUser;
        public VehiclesRepository(
            IDocumentDBConnection connection,
            string userName
            ) : base(connection,
                CosmosDBDefinitions.ConnectedVehiclesId,
                m => m.Customer == userName
                )
        {
            loggedUser = userName;
        }
        */
        private string loggedUser;
        public VehiclesRepository(
            IDocumentDBConnection connection
            
            ) : base(connection,
                CosmosDBDefinitions.ConnectedVehiclesId)
        {
            loggedUser = "";
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
            
            DocumentDBCRUDRepository<Vehicle>
                .DeclareProjection
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

        public async Task<IList<VehicleDTO>> AllSubItems()
        {
            var query = Table(100)
                .Where(SelectFilter)
                .SelectMany(m => m.Vehicles);
            return await ToList<VehicleDTO,Vehicle>(query);
        }

    }
}
