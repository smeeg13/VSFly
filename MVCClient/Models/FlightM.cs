using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVCClient.Models
{
    public class FlightM
    {
        //These is models that are going to be displayed for everybody
                //We don't want to display all infos, only wh'at the user will need

        public int FlightNo { get; set; }
        public string Departure { get; set; }
        public string Destination { get; set; }
        public DateTime Date { get; set; }
        public double Price { get; set; }
        public double SalePrice { get; set; }
    }

}
