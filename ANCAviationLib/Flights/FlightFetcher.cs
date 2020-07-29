using ANCAviationLib.COVID;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Text.RegularExpressions;

namespace ANCAviationLib.Flights
{
    public class FlightFetcher : Fetcher<FlightFetcher>
    {
        private Dictionary<string, string> _filters = new Dictionary<string, string>();
        public FlightDetailRepository _flightDetailRepo = new FlightDetailRepository();
        private string _accessKey = "89553d518f272e5652d22808fdee046c";
        public string LastFetchRaw { get; private set; }
        public string AccessKey
        {
            set
            {
                _accessKey = value;
            }
            get
            {
                return _accessKey;
            }
        }
        public string Address
        {
            get
            {
                string returnString = $"http://api.aviationstack.com/v1/flights?access_key={_accessKey}";
                foreach (KeyValuePair<string, string> entry in _filters)
                {
                    returnString += $"&{entry.Key}={entry.Value}";
                }
                return returnString;
            }
        }
        public FlightFetcher FilterByFlightNumber(string flightNumber)
        {
            _filters.Add("flight_number", flightNumber);
            return this;
        }
        public FlightFetcher FilterByAirlineIata(string airlineIata)
        {
            _filters.Add("airline_iata", airlineIata);
            return this;
        }
        public FlightFetcher FilterByEndpointIATA(Endpoints endPoint, string endpointIata)
        {
            switch (endPoint)
            {
                case Endpoints.Arrival:
                    _filters.Add("arr_iata", endpointIata);
                    break;
                case Endpoints.Departure:
                    _filters.Add("dep_iata", endpointIata);
                    break;
            }
            return this;
        }
        public FlightFetcher FilterByFlightDate(DateTime date)
        {
            _filters.Add("flight_date", date.ToString("yyyy-MM-dd"));
            return this;
        }
        public FlightFetcher FetchRawFromApi()
        {
            HttpWebRequest request = HttpWebRequest.CreateHttp(Address);
            Stream responseStream = request.GetResponse().GetResponseStream();
            StreamReader reader = new StreamReader(responseStream, Encoding.UTF8);
            LastFetchRaw = reader.ReadToEnd();
            return this;
        }
        public FlightFetcher ProcessFetch()
        {
            string[] jsonStringSplit = LastFetchRaw.Split('[');
            jsonStringSplit = jsonStringSplit[1].Split(']');
            string jsonString = jsonStringSplit[0];

            foreach (Match match in MatchFinder(jsonString))
            {
                _flightDetailRepo.Add(match.Value);
            }
            return this;
        }
        public FlightFetcher SaveFetch(Uri directory)
        {
            throw new NotImplementedException();
        }
        private MatchCollection MatchFinder(string jsonString)
        {
            const string pattern = @"[{].+?[{].+?[}].+?[{].+?[}].+?[{].+?[}].+?[{].+?[}].+?[}]";

            MatchCollection matches = Regex.Matches(jsonString, pattern);
            return matches;
        }
        public FlightFetcher ClearFilters()
        {
            _filters.Clear();
            return this;
        }
        public FlightFetcher ClearRepository()
        {
            _flightDetailRepo.Clear();
            return this;
        }
    }
}

