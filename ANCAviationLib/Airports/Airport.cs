using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace ANCAviationLib.Airports
{
    [DataContract]
    public class Airport
    {
        [DataMember]
        public string Iata { set; get; }
        [DataMember]
        public string Name { set; get; }
        [DataMember]
        public string Location { set; get; }
        [DataMember]
        public double Latitude { set; get; }
        [DataMember]
        public double Longitude { set; get; }
    }
}
