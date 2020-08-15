using System;
using System.Collections.Generic;
using System.Text;

namespace ANCAviationLib.COVID
{
    public class LiveRegionCovidStatus
    {
        public int Confirmed { get; set; }
        public int Recovered { get; set; }
        public int Deaths { get; set; }
        public int Actives { get; set; }
    }
}
