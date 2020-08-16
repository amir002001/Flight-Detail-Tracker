using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Text.Json;

namespace ANCAviationLib.Airports
{
    /// <summary>
    /// An aiport fetcher class that contacts the airport API.
    /// </summary>
    public class AirportFetcher : IFetcher<AirportFetcher>
    {
        private string _code;
        private string _lastFetchRaw;
        /// <summary>
        /// last fetched airport.
        /// </summary>
        public Airport FetchedAirport { private set; get; }
        /// <summary>
        /// IATA code to fetch with
        /// </summary>
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
        /// <summary>
        /// computed property returning the proper address based on code.
        /// </summary>
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
                    throw new KeyNotFoundException("Airport Not Found");
                _lastFetchRaw = value;
            }
        }
        /// <summary>
        /// Contacts the API, and reads the response into LastFetchRaw
        /// </summary>
        /// <returns></returns>
        public AirportFetcher FetchRawFromApi()
        {
            try
            {
                var request = HttpWebRequest.CreateHttp(Address);
                request.Method = "GET";
                request.Headers.Add("x-rapidapi-host", "airport-info.p.rapidapi.com");
                request.Headers.Add("x-rapidapi-key", "383c61b71fmsh2b94095b69c26c7p1c25b9jsn2e3321270ba4");
                using (StreamReader reader = new StreamReader(request.GetResponse().GetResponseStream(), Encoding.UTF8))
                {
                    LastFetchRaw = reader.ReadToEnd();
                }
            }
            catch(WebException ex)
            {
                throw new WebException("There was an error contacting the API.", ex);
            }
            return this;
        }

        /// <summary>
        /// Processes the fetch by deserializing it.
        /// </summary>
        /// <returns></returns>
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
