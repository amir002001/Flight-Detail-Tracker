using System;
using System.Collections.Generic;
using System.Text;

namespace ANCAviationLib.COVID
{
    public class LiveCountryCovidStatus
    {
        private List<LiveRegionCovidStatus> _regions = new List<LiveRegionCovidStatus>();
        enum totals
        {
            confirmed,
            recovered,
            deaths,
            actives
        }
        public Dictionary<totals, int> TotalDailies
        {
            get
            {
                Dictionary<totals, int> returnDict = new Dictionary<totals, int>(4);
                int confirmed = 0, recovered = 0, deaths = 0, actives = 0;
                foreach (LiveRegionCovidStatus l in _regions)
                {
                    confirmed += l.Confirmed;
                    actives += l.Actives;
                    recovered += l.Recovered;
                    deaths += l.Deaths;

                }
                returnDict[totals.confirmed] = confirmed;
                returnDict[totals.recovered] = recovered;
                returnDict[totals.deaths] = deaths;
                returnDict[totals.actives] = actives;

                return returnDict;
            }
        }

        public void Clear()
        {
            _regions.Clear();
        }

        public void AddRegionToList(string value)
        {
            JsonSerializerOptions options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            LiveRegionCovidStatus region = JsonSerializer.Deserialize<LiveRegionCovidStatus>(value, options);
            _regions.Add(region);
        }

    }

}

