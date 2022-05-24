using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MVCClient.Models;

namespace MVCClient.Services
{
   public interface IVSFlyServices
    {
        public Task<IEnumerable<FlightM>> GetFlights();

        public Task<IEnumerable<PilotM>> GetPilots();

    }
}
