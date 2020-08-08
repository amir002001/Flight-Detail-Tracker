using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace ANCAviationLib.Flights
{
    [DataContract]
    public class FlightDetails
    {
        [DataMember]
        public string Flight_Date { set; get; }
        [DataMember]
        public string Flight_Status { set; get; }
        [DataMember]
        public Endpoint Departure { set; get; }
        [DataMember]
        public Endpoint Arrival { set; get; }
        [DataMember]
        public Airline Airline { set; get; }
        [DataMember]
        public Flight Flight { set; get; }
    }
}
