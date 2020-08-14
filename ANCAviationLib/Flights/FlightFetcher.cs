using ANCAviationLib.COVID;
using ANCAviationLib.DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Text.RegularExpressions;

namespace ANCAviationLib.Flights
{
    /// <summary>
    /// A class that contacts the aviationstack api and retrieves a set of flights.
    /// </summary>
    public class FlightFetcher : Fetcher<FlightFetcher>
    {
        private Dictionary<string, string> _filters = new Dictionary<string, string>();
        private FlightDetailRepository _flightRepo = new FlightDetailRepository();
        private const string _defaultAccessKey = "89553d518f272e5652d22808fdee046c";
        private string _accessKey = _defaultAccessKey;
        /// <summary>
        /// the last fetch of the fetcher.
        /// </summary>
        private string LastFetchRaw { get; set; }
        /// <summary>
        /// The api access key.
        /// </summary>
        public string AccessKey
        {
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new InvalidOperationException("Access Key can't be empty");
                _accessKey = value;
            }
        }
        /// <summary>
        /// An observable collection holding all newly-fetched flights.
        /// </summary>
        public ObservableCollection<FlightDetails> FlightCollection
        {
            get
            {
                return _flightRepo._flightDetailRepo;
            }
        }
        /// <summary>
        /// Computed property returning the address the http request should use.
        /// </summary>
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
        /// <summary>
        /// Goes back to default access key.
        /// </summary>
        /// <returns></returns>
        public FlightFetcher RevertToDefaultAccessKey()
        {
            AccessKey = _defaultAccessKey;
            return this;
        }
        /// <summary>
        /// adds a flight number filter to the filters dict.
        /// </summary>
        /// <param name="flightNumber">flight number, no IATA or ICAO attached.</param>
        /// <returns></returns>
        public FlightFetcher FilterByFlightNumber(string flightNumber)
        {
            if (string.IsNullOrWhiteSpace(flightNumber))
                return this;
            _filters.Add("flight_number", flightNumber);
            return this;
        }
        /// <summary>
        /// adds an Airline IATA filter to the filters dict.
        /// </summary>
        /// <param name="airlineIata"></param>
        /// <returns></returns>
        public FlightFetcher FilterByAirlineIata(string airlineIata)
        {
            if (string.IsNullOrWhiteSpace(airlineIata))
                return this;
            _filters.Add("airline_iata", airlineIata);
            return this;
        }
        /// <summary>
        /// Adds an endpoint IATA filter based on the end point.
        /// </summary>
        /// <param name="endPoint">whether it is the departure or the arrival point</param>
        /// <param name="endpointIata"></param>
        /// <returns></returns>
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
        /// <summary>
        /// Contacts the API and sets LastFetchRaw
        /// </summary>
        /// <returns></returns>
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
            catch (WebException ex)
            {
                if (ex.Message.Contains("401"))
                {
                    AccessKey = _defaultAccessKey;
                    throw new WebException("Key was invalid. Reverting to default key.", ex);
                }
                throw new WebException("There was an error contacting the API.", ex);

            }
            return this;
        }
        /// <summary>
        /// Processes the fetch by splitting it into parsable texts.
        /// </summary>
        /// <returns></returns>
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
        /// <summary>
        /// uses regex to find the right pattern in the fetched flights
        /// </summary>
        /// <param name="jsonString"></param>
        /// <returns></returns>
        private MatchCollection MatchFinder(string jsonString)
        {
            //const string pattern = @"[{].+?[{].+?[}].+?[{].+?[}].+?[{].+?[}].+?[{].+?[}].+?[}]+?";
            const string pattern = "[{].+?\"live\":.+?[}]+";
            MatchCollection matches = Regex.Matches(jsonString, pattern);
            return matches;
        }
        /// <summary>
        /// Clears the fetcher.
        /// </summary>
        /// <returns></returns>
        public FlightFetcher ClearFetcher()
        {
            _flightRepo.Clear();
            _filters.Clear();
            LastFetchRaw = "";
            return this;
        }
    }
}

