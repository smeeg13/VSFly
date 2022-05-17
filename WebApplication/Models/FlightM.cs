﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Models
{
    public class FlightM
    {
        //These is models that are going to be displayed for everybody
                //We don't want to display all infos, only wh'at the user will need

        public int FlightNo { get; set; }
        public string Departure { get; set; }
        public string Destination { get; set; }
        public DateTime Date { get; set; }
    }

}
