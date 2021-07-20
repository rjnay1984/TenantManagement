﻿using System.Collections.Generic;

namespace Core.Entities
{
    public class Building
    {
        public int BuildingId { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public List<Unit> Units { get; set; }
    }
}