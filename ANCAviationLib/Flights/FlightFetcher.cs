using ANCAviationLib.COVID;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        private FlightDetailRepository _flightRepo = new FlightDetailRepository();
        private const string _defaultAccessKey = "89553d518f272e5652d22808fdee046c";
        private string _accessKey = _defaultAccessKey;
        private string LastFetchRaw { get; set; }
        public string AccessKey
        {
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new InvalidOperationException("Access Key can't be empty");
                _accessKey = value;
            }
        }
        public ObservableCollection<FlightDetails> FlightCollection
        {
            get
            {
                return _flightRepo._flightDetailRepo;            }
        }
        private string Address
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
        public FlightFetcher RevertToDefaultAccessKey()
        {
            AccessKey = _defaultAccessKey;
            return this;
        }
        public FlightFetcher FilterByFlightNumber(string flightNumber)
        {
            if (string.IsNullOrWhiteSpace(flightNumber))
                return this;
            _filters.Add("flight_number", flightNumber);
            return this;
        }
        public FlightFetcher FilterByAirlineIata(string airlineIata)
        {
            if (string.IsNullOrWhiteSpace(airlineIata))
                return this;
            _filters.Add("airline_iata", airlineIata);
            return this;
        }
        public FlightFetcher FilterByEndpointIata(Endpoints endPoint, string endpointIata)
        {
            if (string.IsNullOrWhiteSpace(endpointIata))
                return this;
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
        public FlightFetcher FilterByFlightDate(DateTime? date)
        {
            if (date == null)
            {
                return this;
            }
            _filters.Add("flight_date", date?.ToString("yyyy-MM-dd"));
            return this;
        }
        public FlightFetcher FetchRawFromApi()
        {
            HttpWebRequest request = HttpWebRequest.CreateHttp(Address);
            try
            {
                using (StreamReader reader = new StreamReader(request.GetResponse().GetResponseStream(), Encoding.UTF8))
                {
                    LastFetchRaw = reader.ReadToEnd();
                }
            }
            catch(WebException ex)
            {
                if (ex.Message.Contains("401"))
                {
                    AccessKey = _defaultAccessKey;
                    throw new WebException("Key was invalid. Reverting to default key");
                }
                throw ex;
            }
            return this;
        }
        public FlightFetcher ProcessFetch()
        {
            string[] jsonStringSplit = LastFetchRaw.Split('[');
            jsonStringSplit = jsonStringSplit[1].Split(']');
            string jsonString = jsonStringSplit[0];

            foreach (Match match in MatchFinder(jsonString))
            {
                _flightRepo.Add(match.Value);
            }
            return this;
        }
        private MatchCollection MatchFinder(string jsonString)
        {
            const string pattern = @"[{].+?[{].+?[}].+?[{].+?[}].+?[{].+?[}].+?[{].+?[}].+?[}]";

            MatchCollection matches = Regex.Matches(jsonString, pattern);
            return matches;
        }
        public FlightFetcher ClearFetcher()
        {
            _flightRepo.Clear();
            _filters.Clear();
            LastFetchRaw = "";
            return this;
        }
    }
}

