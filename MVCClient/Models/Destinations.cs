using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVCClient.Models
{
    public class Destinations
    {
        public SelectList Names { get; set; }
        public string NameChoosed { get; set; }
        public IEnumerable<Destination> DestinationsAll { get; set; }
    }
}
