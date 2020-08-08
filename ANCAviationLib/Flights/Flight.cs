using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace ANCAviationLib.Flights
{
    [DataContract]
    public class Flight
    {
        [DataMember]
        public string Iata { set; get; }
        [DataMember]
        public string Number { set; get; }
    }
}
