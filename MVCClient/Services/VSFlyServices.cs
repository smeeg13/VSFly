using MVCClient.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace MVCClient.Services
{
    public class VSFlyServices : IVSFlyServices
    {
        private readonly HttpClient _client;
        private readonly string _baseuri;

        public VSFlyServices(HttpClient client)
        {
            _client = client;
            _baseuri = "https://localhost:44373/api/";
        }
        public async Task<IEnumerable<FlightM>> GetFlights()
        {
            var uri = _baseuri + "Flights";

            var responseString = await _client.GetStringAsync(uri); //Ask for the JSON

            var flightList = JsonConvert.DeserializeObject<IEnumerable<FlightM>>(responseString); //Deserialized what received in a list
            
            return flightList;
        }

        public async Task<IEnumerable<PilotM>> GetPilots()
        {
            var uri = _baseuri + "Pilots";

            var responseString = await _client.GetStringAsync(uri); //Ask for the JSON

            var pilotList = JsonConvert.DeserializeObject<IEnumerable<PilotM>>(responseString); //Deserialized what received in a list

            return pilotList;
        }
    }
}
