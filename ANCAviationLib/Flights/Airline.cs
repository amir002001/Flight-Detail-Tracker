using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace ANCAviationLib.Flights
{
    [DataContract]
    public class Airline
    {
        [DataMember]
        public string Name { set; get; }
        [DataMember]
        public string Iata { set; get; }
    }
}
