using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.BuildingEndpoints
{
    public class BuildingResult
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
    }
}
