using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Extensions
{
    public static class ConverterExtensions
    {

        //Converting models to DB

        public static Models.FlightM ConvertToFlightM(this VSFly.Flight f)
        {
            Models.FlightM fm = new Models.FlightM();
            fm.FlightNo = f.FlightNo;
            fm.Departure = f.Departure;
            fm.Destination = f.Destination;
            fm.Date = f.Date;
            return fm;
        }

        public static VSFly.Flight ConvertToFlight(this Models.FlightM fm)
        {
            VSFly.Flight f = new VSFly.Flight();
            f.FlightNo = fm.FlightNo;
            f.Departure = fm.Departure;
            f.Destination = fm.Destination;
            f.Date = fm.Date;
            return f;
        }

        public static Models.PilotM ConvertToPilotM(this VSFly.Pilot p)
        {
            Models.PilotM pm = new Models.PilotM();
            pm.PersonId = p.PersonId;
            pm.FlightHours = p.FlightHours;
           
            return pm;
        }

        public static VSFly.Pilot ConvertToPilot(this Models.PilotM pm)
        {
            VSFly.Pilot p = new VSFly.Pilot();
            p.PersonId = pm.PersonId;
            p.FlightHours = pm.FlightHours;
            
            return p;
        }
    }
}
