using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace ANCAviationLib.Flights
{
    [DataContract]
    public class Endpoint
    {
        [DataMember]
        public string Airport { set; get; }
        [DataMember]
        public string Iata { set; get; }
        [DataMember]
        public string Timezone { set; get; }
        [DataMember]
        public string Scheduled { set; get; }
    }
}
