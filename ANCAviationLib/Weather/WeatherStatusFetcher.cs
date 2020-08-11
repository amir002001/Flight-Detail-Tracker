using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace ANCAviationLib.Weather
{
    public class WeatherStatusFetcher : Fetcher<WeatherStatusFetcher>
    {
 
        private string _today = DateTime.Today.ToString("yyyy-MM-dd");
        private string _code = "";
        private string _lastFetchRaw { get; private set; }
        private WeatherStatus _weatherStatus = new WeatherStatus();
        public WeatherStatus WeatherStatusRepository
        {
            get
            {
                return WeatherStatusRepository;
            }
            
        }

        WeatherStatus _lastFetchParsed;
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

        public WeatherStatusFetcher FetchRawFromApi()
        {
            HttpWebRequest request = HttpWebRequest.CreateHttp(Address);
            request.Headers.Add("x-rapidapi-host", "aerisweather1.p.rapidapi.com");
            request.Headers.Add("x-rapidapi-key", "b8c87a89b4mshc6c9f01386f13cap159265jsn9457679896c5");
            WebResponse response = request.GetResponse();
            Stream stream = response.GetResponseStream();
            StreamReader reader = new StreamReader(stream, Encoding.UTF8);
            _lastFetchRaw = reader.ReadToEnd();
            return this;
        }
     

        public WeatherStatusFetcher FetchRawFromApi()
        {
            HttpWebRequest request = HttpWebRequest.CreateHttp(Address);
            Stream responseStream = request.GetResponse().GetResponseStream();
            StreamReader reader = new StreamReader(responseStream, Encoding.UTF8);
            LastFetchRaw = reader.ReadToEnd();
            return this;
        }
        public WeatherStatusFetcher ProcessFetch()
        {
            string[] jsonStringSplit = LastFetchRaw.Split('[');
            jsonStringSplit = jsonStringSplit[1].Split(']');
            string jsonString = jsonStringSplit[0];
        }
        public WeatherStatusFetcher SaveFetch(Uri directory)
        {
            throw new NotImplementedException();
        }
        private MatchCollection MatchFinder(string jsonString)
        {
            const string pattern = @"[{].+?[{].+?[}].+?[{].+?[}].+?[{].+?[}].+?[{].+?[}].+?[}]";

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
}
