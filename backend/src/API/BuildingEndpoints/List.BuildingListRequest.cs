using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.BuildingEndpoints
{
    public class BuildingListRequest
    {
        public int Page { get; set; }
        public int PerPage { get; set; }
    }
}
