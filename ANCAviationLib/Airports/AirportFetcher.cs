using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Text.Json;

namespace ANCAviationLib.Airports
{
    public class AirportFetcher : Fetcher<AirportFetcher>
    {
        private string _code;
        private string _lastFetchRaw;
        public Airport FetchedAirport { private set; get; }
        public string Code
        {
            get => _code; set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new InvalidOperationException("airport IATA can't be empty");
                if (value.Length != 3)
                    throw new InvalidOperationException("airport IATA has to have 3 characters");
                _code = value;
            }
        }
        private string Address
        {
            get
            {
                return $"https://airport-info.p.rapidapi.com/airport?iata={Code}";
            }
        }

        private string LastFetchRaw
        {
            get => _lastFetchRaw;
            set
            {
                if (value.Contains("No airport found"))
                    throw new WebException("No airport found");
                _lastFetchRaw = value;
            }
        }
        public AirportFetcher FetchRawFromApi()
        {
            var request = HttpWebRequest.CreateHttp(Address);
            request.Method = "GET";
            request.Headers.Add("x-rapidapi-host", "airport-info.p.rapidapi.com");
            request.Headers.Add("x-rapidapi-key", "383c61b71fmsh2b94095b69c26c7p1c25b9jsn2e3321270ba4");
            using (StreamReader reader = new StreamReader(request.GetResponse().GetResponseStream(), Encoding.UTF8))
            {
                LastFetchRaw = reader.ReadToEnd();
            }
            return this;
        }

        public AirportFetcher ProcessFetch()
        {
            JsonSerializerOptions options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            FetchedAirport = JsonSerializer.Deserialize<Airport>(LastFetchRaw, options);
            return this;
        }
    }
}
