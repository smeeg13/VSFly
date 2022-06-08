using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVCClient.Models
{
    public class PassengerAdmin
    {

        public IEnumerable<PassengerM> Passengers { get; set; }
        public string searchFullName { get; set; }
        public string PassportIdChoosed { get; set; }
        public SelectList PassportIdList { get; set; }


    }
}
