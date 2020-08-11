using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Text.RegularExpressions;

namespace ANCAviationLib.Weather
{
    public class WeatherStatusFetcher : Fetcher<WeatherStatusFetcher>
    {
 
        private string _today = DateTime.Today.ToString("yyyy-MM-dd");
        private string _code = "";
        private object lastFetchRaw;

        private string GetLastFetchRaw()
        {
            return lastFetchRaw;
        }

        private void SetLastFetchRaw(string value)
        {
            lastFetchRaw = value;
        }

        private WeatherStatus _weatherStatus = new WeatherStatus();
        public WeatherStatus WeatherStatusRepository
        {
            get
            {
                WeatherStatus weatherStatus = DataContractJsonSerializer<WeatherStatus>(myString);
                return WeatherStatusRepository;
            }
            
        }

        private T DataContractJsonSerializer<T>(object myString)
        {
            throw new NotImplementedException();
        }

        WeatherStatus _lastFetchParsed;
        private object myString;

        WeatherStatus Fetch
        {
            get
            {
                return _lastFetchParsed ;
            }
            set
            {
                _lastFetchParsed = value;
            }

        }


        public string Address
        {
            get
            {
                //get returns the value based on today and IATA code.
                return $"https://aerisweather1.p.rapidapi.com/report/country/code?format=json&date-format=YYYY-MM-DD&date={_today}"; }
            }


        public WeatherStatusFetcher ProcessFetch
        {
            get
            {
                string[] jsonStringSplit = GetLastFetchRaw().Split('[');
                jsonStringSplit = jsonStringSplit[1].Split(']');
                string jsonString = jsonStringSplit[0];
            }
        }

        public object LastFetchRaw { get; private set; }

        public WeatherStatusFetcher FetchRawFromApi => throw new NotImplementedException();

        public WeatherStatusFetcher SaveFetch(Uri directory)
        {
            throw new NotImplementedException();
        }
        private MatchCollection MatchFinder(string jsonString)
        {
            const string pattern = "\".ob+.]?";

            MatchCollection matches = Regex.Matches(jsonString, pattern);
            return matches;
        }


        public WeatherStatusFetcher ClearRepository()
        {
            WeatherStatusRepository.Clear();
            return this;
        }

        public WeatherStatusFetcher SaveFetch(Uri Directory)
        {
            throw new NotImplementedException();
        }
    }
}
