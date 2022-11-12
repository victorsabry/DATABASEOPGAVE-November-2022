using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace HotelDBConnection
{
    internal class Facility
    {
        public int Facility_id { get; set; }

        public string Name { get; set; }

        public override string ToString()
        {
            return $"ID: {Facility_id}, Name: {Name}";
        }
    }
}
