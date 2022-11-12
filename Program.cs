using System;

namespace HotelDBConnection
{
    class Program
    {
        static void Main(string[] args)
        {
            DBClientHotels dbcHotels = new DBClientHotels();
            dbcHotels.Start();
            Console.WriteLine("--------------------------------------------------------------------");
            DBClientFacilities dbcFacilities = new DBClientFacilities();
            dbcFacilities.Start();
        }
    }
}
