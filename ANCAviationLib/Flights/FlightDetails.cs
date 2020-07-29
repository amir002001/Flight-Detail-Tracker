using System;
using System.Collections.Generic;
using System.Text;

namespace ANCAviationLib.Flights
{
    class FlightDetails
    {
        public string Flight_Date { set; get; }
        public string Flight_Status { set; get; }
        public Endpoint Departure { set; get; }
        public Endpoint Arrival { set; get; }
        public Dictionary<string, string> Airline { set; get; }
        public Flight Flight { set; get; }
    }
}
