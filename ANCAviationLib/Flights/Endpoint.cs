using System;
using System.Collections.Generic;
using System.Text;

namespace ANCAviationLib.Flights
{
    class Endpoint
    {
        public string Airport { set; get; }
        public string Iata { set; get; }
        public string Timezone { set; get; }
        public string Scheduled { set; get; }
    }
}
