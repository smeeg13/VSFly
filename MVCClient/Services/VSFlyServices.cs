using Microsoft.AspNetCore.Mvc;
using MVCClient.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
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

        //Get All Flight Available ( with free seats)
        public async Task<IEnumerable<FlightM>> GetFlights()
        {
            var uri = _baseuri + "Flights";

            var responseString = await _client.GetStringAsync(uri); //Ask for the JSON

            var flightList = JsonConvert.DeserializeObject<IEnumerable<FlightM>>(responseString); //Deserialized what received in a list
            
            return flightList;
        }
        public async Task<IEnumerable<PassengerM>> GetPassengers()
        {
            var uri = _baseuri + "Passengers";

            var responseString = await _client.GetStringAsync(uri); //Ask for the JSON

            var passengerList = JsonConvert.DeserializeObject<IEnumerable<PassengerM>>(responseString); //Deserialized what received in a list

            return passengerList;
        }


        //Get One Flight by ID
        public async Task<FlightM> GetFlight(int id)
        {
            var uri = _baseuri + "Flights/"+id;

            var responseString = await _client.GetStringAsync(uri); //Ask for the JSON

            var flight = JsonConvert.DeserializeObject<FlightM>(responseString); //Deserialized what received in a list

            return flight;
        }

        //Get All Flights for One Pilot
        public async Task<IEnumerable<FlightM>> GetFlightsByPilotId(int id)
        {
            var uri = _baseuri + "Flights/PilotId/" + id;

            var responseString = await _client.GetStringAsync(uri); //Ask for the JSON

            var flightsForPilot = JsonConvert.DeserializeObject<IEnumerable<FlightM>>(responseString); //Deserialized what received in a list

            IEnumerable<FlightM> query = flightsForPilot.OrderBy(f => f.Date);
            return query;
        }

        //Get All Booking for One Flight
        public async Task<IEnumerable<BookingM>> GetBookingByFlightNo(int id)
        {
            var uri = _baseuri + "FlightNo/" + id;

            var responseString = await _client.GetStringAsync(uri); //Ask for the JSON

            var bookingsForFlight = JsonConvert.DeserializeObject<IEnumerable<BookingM>>(responseString); //Deserialized what received in a list

            return bookingsForFlight;
        }

         //Get All Booking for One Passenger
        public async Task<IEnumerable<BookingM>> GetBookingByPassengerId(int id)
        {
            var uri = _baseuri + "Bookings/PassengerId/" + id;

            var responseString = await _client.GetStringAsync(uri); //Ask for the JSON

            var bookingsFoPassenger = JsonConvert.DeserializeObject<IEnumerable<BookingM>>(responseString); //Deserialized what received in a list

            return bookingsFoPassenger;
        }

        //Get All Booking for One Passenger
        public async Task<BookingM> GetSpecificBooking(int FlightNo, int PersonId)
        {
            var uri = _baseuri + "Bookings/Find/" + FlightNo+"/"+PersonId;

            var responseString = await _client.GetStringAsync(uri); //Ask for the JSON

            var bookingsFoPassenger = JsonConvert.DeserializeObject<BookingM>(responseString); //Deserialized what received in a list

            return bookingsFoPassenger;
        }

       
        //Get One Passenger by Email
        public async Task<PassengerM> GetPassengerByPassportID(string passportId)
        {
            var uri = _baseuri + "Passengers/Find/" + passportId;

            var responseString = await _client.GetStringAsync(uri); //Ask for the JSON

            var booking = JsonConvert.DeserializeObject<PassengerM>(responseString); //Deserialized what received in a list

            return booking;
        }

        //Get One Passenger by Id
        public async Task<PassengerM> GetPassenger(int Id)
        {
            var uri = _baseuri + "Passengers/" + Id;

            var responseString = await _client.GetStringAsync(uri); //Ask for the JSON

            var booking = JsonConvert.DeserializeObject<PassengerM>(responseString); //Deserialized what received in a list

            return booking;
        }


        //Get One Passenger by Id
        public async Task<PilotM> GetPilot(int Id)
        {
            var uri = _baseuri + "Pilots/" + Id;

            var responseString = await _client.GetStringAsync(uri); //Ask for the JSON

            var booking = JsonConvert.DeserializeObject<PilotM>(responseString); //Deserialized what received in a list

            return booking;
        }

        //Get Sum Sale Price of one Flight
        public async Task<double> GetSumSalePrice(int id)
        {
            var uri = _baseuri + "Sum/" + id;

            var responseString = await _client.GetStringAsync(uri); //Ask for the JSON

            var sumSalePrice = JsonConvert.DeserializeObject<double>(responseString); //Deserialized what received in a list

            return sumSalePrice;
        }

        //Get Average Sale Price of one/many Flight for one Destination
        public async Task<double> GetAvgSalePrice(string destination)
        {
            var uri = _baseuri + "Avg/" + destination;

            var responseString = await _client.GetStringAsync(uri); //Ask for the JSON

            var avgSalePrice = JsonConvert.DeserializeObject<double>(responseString); //Deserialized what received in a list

            return avgSalePrice;
        }



        //POST METHOD
        //Create New Booking
        [HttpPost]
        public Boolean CreateBooking(BookingM booking)
        {

            var uri = _baseuri + "Bookings";

            //HTTP POST
            var postTask = _client.PostAsJsonAsync<BookingM>(uri, booking);
            postTask.Wait();

            var result = postTask.Result;
            if (result.IsSuccessStatusCode)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //Create New Passenger
        [HttpPost]
        public Boolean CreatePassenger(PassengerM student)
        {

            var uri = _baseuri + "Passengers";

            //HTTP POST
            var postTask = _client.PostAsJsonAsync<PassengerM>(uri, student);
            postTask.Wait();

            var result = postTask.Result;
            if (result.IsSuccessStatusCode)
            {
                return true;
            }
            else
            {
                return false;
            }
        }






        public async Task<IEnumerable<PilotM>> GetPilots()
        {
            var uri = _baseuri + "Pilots";

            var responseString = await _client.GetStringAsync(uri); //Ask for the JSON

            var pilotList = JsonConvert.DeserializeObject<IEnumerable<PilotM>>(responseString); //Deserialized what received in a list

            return pilotList;
        }


        public async Task<IEnumerable<BookingM>> GetBookings()
        {
            var uri = _baseuri + "Bookings";

            var responseString = await _client.GetStringAsync(uri); //Ask for the JSON

            var bookingList = JsonConvert.DeserializeObject<IEnumerable<BookingM>>(responseString); //Deserialized what received in a list

            return bookingList;
        }
    }
}
