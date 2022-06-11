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

//------------------------FLIGHTS METHOD----------------------------------

        //Get All Flight Available ( with free seats)
        public async Task<IEnumerable<FlightM>> GetFlights()
        {
            var uri = _baseuri + "Flights";

            var responseString = await _client.GetStringAsync(uri); //Ask for the JSON

            var flightList = JsonConvert.DeserializeObject<IEnumerable<FlightM>>(responseString); //Deserialized what received in a list

            return flightList;
        }


        //Get All Flights
        public async Task<IEnumerable<FlightM>> GetAllFlights()
        {
            var uri = _baseuri + "Flights/All";

            var responseString = await _client.GetStringAsync(uri); //Ask for the JSON

            var flightList = JsonConvert.DeserializeObject<IEnumerable<FlightM>>(responseString); //Deserialized what received in a list


            return flightList;
        }



        //Get One Flight by ID
        public async Task<FlightM> GetFlight(int id)
        {
            var uri = _baseuri + "Flights/"+id;

            var responseString = await _client.GetStringAsync(uri); //Ask for the JSON

            var flight = JsonConvert.DeserializeObject<FlightM>(responseString); //Deserialized what received in a list

            return flight;
        }

        //Get All Flight ADMIN
        public async Task<IEnumerable<FlightAdminM>> GetAllAdminFlights()
        {
            var uri = _baseuri + "Flights/Admin/All";

            var responseString = await _client.GetStringAsync(uri); //Ask for the JSON

            var flightList = JsonConvert.DeserializeObject<IEnumerable<FlightAdminM>>(responseString); //Deserialized what received in a list


            return flightList;
        }
        //Get One Flight ADMIN by ID
        public async Task<FlightAdminM> GetAdminFlight(int id)
        {
            var uri = _baseuri + "Flights/Admin/FlightNo/" + id;

            var responseString = await _client.GetStringAsync(uri); //Ask for the JSON

            var flight = JsonConvert.DeserializeObject<FlightAdminM>(responseString); //Deserialized what received in a list

            return flight;
        }

        //Create New Flight
        [HttpPost]
        public Boolean CreateFlight(FlightAdminM f)
        {

            var uri = _baseuri + "Flights/Admin/CreateFlight";

            //HTTP POST
            var postTask = _client.PostAsJsonAsync<FlightAdminM>(uri, f);
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

        //Update Flight
        [HttpPut]
        public Boolean UpdateFlight(FlightAdminM f)
        {

            var uri = _baseuri + "Flights/Admin/UpdateFlight/" + f.FlightNo;

            //HTTP POST
            var postTask = _client.PutAsJsonAsync<FlightAdminM>(uri, f);
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

        //Delete Flight
        public Boolean DeleteFlight(int id)
        {

            var uri = _baseuri + "Flights/Admin/DeleteFlight/" + id;

            //HTTP DELETE
            var deleteTask = _client.DeleteAsync(uri);
            deleteTask.Wait();

            var result = deleteTask.Result;
            if (result.IsSuccessStatusCode)
                return true;

            return false;
        }


        //------------------------BOOKING & TICKETS METHOD----------------------------------



        //Get All Booking for One Flight
        public async Task<IEnumerable<Ticket>> GetTicketsByFlightNo(int id)
        {
            var uri = _baseuri + "Bookings/TicketsForFlight/" + id;

            var responseString = await _client.GetStringAsync(uri); //Ask for the JSON

            var bookingsForFlight = JsonConvert.DeserializeObject<IEnumerable<Ticket>>(responseString); //Deserialized what received in a list

            return bookingsForFlight;
        }

         //Get All Booking for One Passenger
        public async Task<IEnumerable<BookingM>> GetBookingsByPassengerId(int id)
        {
            var uri = _baseuri + "Bookings/PassengerId/" + id;

            var responseString = await _client.GetStringAsync(uri); //Ask for the JSON

            var bookingsFoPassenger = JsonConvert.DeserializeObject<IEnumerable<BookingM>>(responseString); //Deserialized what received in a list

            return bookingsFoPassenger;
        }
        //Get All Tickets for One Passenger
        public async Task<IEnumerable<Ticket>> GetTicketsByPassengerId(int id)
        {
            var uri = _baseuri + "Bookings/Tickets/PassengerId/" + id;

            var responseString = await _client.GetStringAsync(uri); //Ask for the JSON

            var bookingsFoPassenger = JsonConvert.DeserializeObject<IEnumerable<Ticket>>(responseString); //Deserialized what received in a list

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

        //Create New Booking
        [HttpPost]
        public Boolean CreateBooking(BookingM booking)
        {

            var uri = _baseuri + "Bookings/Create";

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


        //Get one specific ticket
        public async Task<Ticket> GetTicket(int flightNo, int personId)
        {
            var uri = _baseuri + "Bookings/Tickets/" + flightNo+"/"+personId;

            var responseString = await _client.GetStringAsync(uri); //Ask for the JSON

            var bookingsFoPassenger = JsonConvert.DeserializeObject<IEnumerable<Ticket>>(responseString); //Deserialized what received in a list

            
            return bookingsFoPassenger.FirstOrDefault(); ;
        }



        //-------------------DESTINATIONS METHODS------------------------------

        //Get All Destinations and their Flights
        public async Task<IEnumerable<Destination>> GetAllDestinations()
        {
            var uri = _baseuri + "Flights/Destinations";

            var responseString = await _client.GetStringAsync(uri); //Ask for the JSON

            var Destinations = JsonConvert.DeserializeObject<IEnumerable<Destination>>(responseString); //Deserialized what received in a list

            return Destinations;
        }

        //Get all available flights for one destination
        public async Task<IEnumerable<FlightAdminM>> GetFlightsForDestination(string destinationName)
        {
            var uri = _baseuri + "Flights/Destinations/Flights/" + destinationName;

            var responseString = await _client.GetStringAsync(uri); //Ask for the JSON

            var FlightsForDestinations = JsonConvert.DeserializeObject<IEnumerable<FlightAdminM>>(responseString); //Deserialized what received in a list

            return FlightsForDestinations;
        }


        //Get All Tickets for One Destination
        public async Task<IEnumerable<Ticket>> GetTicketsByDestination(string dest)
        {
            var uri = _baseuri + "Bookings/Tickets/Destination/" + dest;

            var responseString = await _client.GetStringAsync(uri); //Ask for the JSON

            var ticketsFoDestination = JsonConvert.DeserializeObject<IEnumerable<Ticket>>(responseString); //Deserialized what received in a list

            return ticketsFoDestination;
        }


        //------------------------PASSENGERS METHOD----------------------------------

        //Get all Passengers
        public async Task<IEnumerable<PassengerM>> GetPassengers()
        {
            var uri = _baseuri + "Passengers";

            var responseString = await _client.GetStringAsync(uri); //Ask for the JSON

            var passengerList = JsonConvert.DeserializeObject<IEnumerable<PassengerM>>(responseString); //Deserialized what received in a list

            return passengerList;
        }

        //Get One Passenger by PassportID
        public async Task<PassengerM> GetPassengerByPassportID(string passportId)
        {
            var uri = _baseuri + "Passengers/Find/" + passportId;

            var responseString = await _client.GetStringAsync(uri); //Ask for the JSON

            var passenger = JsonConvert.DeserializeObject<PassengerM>(responseString); //Deserialized what received in a list

            return passenger;
        }

        //Get One Passenger by Id
        public async Task<PassengerM> GetPassenger(int Id)
        {
            var uri = _baseuri + "Passengers/" + Id;

            var responseString = await _client.GetStringAsync(uri); //Ask for the JSON

            var passenger = JsonConvert.DeserializeObject<PassengerM>(responseString); //Deserialized what received in a list

            return passenger;
        }


        //Create New Passenger
        [HttpPost]
        public Boolean CreatePassenger(PassengerM student)
        {

            var uri = _baseuri + "Passengers/CreatePassenger";

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

        //Update Passenger
        [HttpPut]
        public Boolean UpdatePassenger(PassengerM p)
        {

            var uri = _baseuri + "Passengers/UpdatePassenger/" + p.PersonId;

            //HTTP POST
            var postTask = _client.PutAsJsonAsync<PassengerM>(uri, p);
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

        //Delete Passenger
        public Boolean DeletePassenger(int id)
        {

            var uri = _baseuri + "Passengers/Admin/DeletePassenger/" + id;

            //HTTP DELETE
            var deleteTask = _client.DeleteAsync(uri);
            deleteTask.Wait();

            var result = deleteTask.Result;
            if (result.IsSuccessStatusCode)
                return true;

            return false;
        }


        //------------------------PILOTS METHOD----------------------------------

        //Get All Pilots
        public async Task<IEnumerable<PilotAdminM>> GetPilots()
        {
            var uri = _baseuri + "Pilots";

            var responseString = await _client.GetStringAsync(uri); //Ask for the JSON

            var pilotList = JsonConvert.DeserializeObject<IEnumerable<PilotAdminM>>(responseString); //Deserialized what received in a list

            return pilotList;
        }

        //Get One Pilot by PassportID
        public async Task<PilotAdminM> GetPilotByPassportID(string passportId)
        {
            var uri = _baseuri + "Pilots/Find/" + passportId;

            var responseString = await _client.GetStringAsync(uri); //Ask for the JSON

            var pilotAdmin = JsonConvert.DeserializeObject<PilotAdminM>(responseString); //Deserialized what received in a list

            return pilotAdmin;
        }

        //Get One Passenger by Id
        public async Task<PilotAdminM> GetPilot(int Id)
        {
            var uri = _baseuri + "Pilots/" + Id;

            var responseString = await _client.GetStringAsync(uri); //Ask for the JSON

            var pilotAdmin = JsonConvert.DeserializeObject<PilotAdminM>(responseString); //Deserialized what received in a list

            return pilotAdmin;
        }

        //Update Pilot
        [HttpPut]
        public Boolean UpdatePilot(PilotAdminM p)
        {

            var uri = _baseuri + "Pilots/Admin/UpdatePilot/" + p.PersonId;

            //HTTP POST
            var postTask = _client.PutAsJsonAsync<PilotAdminM>(uri, p);
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

        //Create New Pilot
        [HttpPost]
        public Boolean CreatePilot(PilotAdminM f)
        {

            var uri = _baseuri + "Pilots/Admin/CreatePilot";

            //HTTP POST
            var postTask = _client.PostAsJsonAsync<PilotAdminM>(uri, f);
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
        //Delete a given Pilot
        public Boolean DeletePilot(int id)
        {

            var uri = _baseuri + "Pilots/Admin/DeletePilot/" + id;

            //HTTP DELETE
            var deleteTask = _client.DeleteAsync(uri);
            deleteTask.Wait();

            var result = deleteTask.Result;
            if (result.IsSuccessStatusCode)
                return true;

            return false;
        }


        //Get All Flights for One Pilot
        public async Task<IEnumerable<FlightAdminM>> GetFlightsByPilotId(int id)
        {
            var uri = _baseuri + "Flights/PilotId/" + id;

            var responseString = await _client.GetStringAsync(uri); //Ask for the JSON

            var flightsForPilot = JsonConvert.DeserializeObject<IEnumerable<FlightAdminM>>(responseString); //Deserialized what received in a list

            IEnumerable<FlightAdminM> query = flightsForPilot.OrderBy(f => f.Date);
            return query;
        }

        //Get All Flights assigned to One CoPilot
        public async Task<IEnumerable<FlightAdminM>> GetFlightsByCoPilotId(int id)
        {
            var uri = _baseuri + "Flights/CoPilotId/" + id;

            var responseString = await _client.GetStringAsync(uri); //Ask for the JSON

            var flightsForPilot = JsonConvert.DeserializeObject<IEnumerable<FlightAdminM>>(responseString); //Deserialized what received in a list

            IEnumerable<FlightAdminM> query = flightsForPilot.OrderBy(f => f.Date);
            return query;
        }

        ////Get Sum Sale Price of one Flight
        //public async Task<double> GetSumSalePrice(int id)
        //{
        //    var uri = _baseuri + "Sum/" + id;

        //    var responseString = await _client.GetStringAsync(uri); //Ask for the JSON

        //    var sumSalePrice = JsonConvert.DeserializeObject<double>(responseString); //Deserialized what received in a list

        //    return sumSalePrice;
        //}

        ////Get Average Sale Price of one/many Flight for one Destination
        //public async Task<double> GetAvgSalePrice(string destination)
        //{
        //    var uri = _baseuri + "Avg/" + destination;

        //    var responseString = await _client.GetStringAsync(uri); //Ask for the JSON

        //    var avgSalePrice = JsonConvert.DeserializeObject<double>(responseString); //Deserialized what received in a list

        //    return avgSalePrice;
        //}




    }
}
