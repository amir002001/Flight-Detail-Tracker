﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace ANCAviationLib.COVID
{
    
    public class LiveCovidStatusFetcher : IFetcher<LiveCovidStatusFetcher>
    {
        private string _today = DateTime.Today.ToString("yyyy-MM-dd");
        private string _code = "";
        private string _lastFetchRaw;
        private LiveCountryCovidStatus _liveCountryCovidStatusRepository = new LiveCountryCovidStatus();
        public LiveCountryCovidStatus LiveCountryCovidStatusRepository
        {
            get
            {
                return _liveCountryCovidStatusRepository;
            }
            
        }

        public string Code {
            get
            {
                return _code;
            }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new InvalidOperationException("Iata code cannot be null or white space");
                if (value.Trim().Length != 2)
                    throw new InvalidOperationException("Iata code has to be only two characters");
                _code = value;
            }
        }

        LiveCountryCovidStatus _lastFetchParsed ;
        LiveCountryCovidStatus Fetch
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
                return $"https://covid-19-data.p.rapidapi.com/report/country/code?format=json&date-format=YYYY-MM-DD&date={_today}&code={Code}"; }
            } 

        public LiveCovidStatusFetcher FetchRawFromApi()
        {
            HttpWebRequest request = HttpWebRequest.CreateHttp(Address);
            request.Headers.Add("x-rapidapi-host", "covid-19-data.p.rapidapi.com");
            request.Headers.Add("x-rapidapi-key", "83f3091260msh62a2c88c3615566p1bbdb8jsn29647eb037fe");
            WebResponse response = request.GetResponse();
            Stream stream = response.GetResponseStream();
            StreamReader reader = new StreamReader(stream, Encoding.UTF8);
            _lastFetchRaw = reader.ReadToEnd();
            return this;
        }
        public string CountryName
        {
            get
            {
                string[] splitByQuote = _lastFetchRaw.Split('"');
                string countryName = splitByQuote[3];
                return countryName;
            }
        }

        public LiveCovidStatusFetcher ProcessFetch()
        {
            if (_lastFetchRaw.Contains("Country not found"))
                throw new WebException("Country Not Found");
            string[] open = _lastFetchRaw.Split('[');
            string[] close = open[2].Split(']');
            string provincies = close[0];
            string pattern = @"[{].+?[}]";
            MatchCollection matches = Regex.Matches(provincies, pattern);
            Fetch = new LiveCountryCovidStatus();
            foreach (Match match in matches)
            {
                Fetch.AddRegionToList(match.Value);
                
            }
            return this;

        }
        public LiveCovidStatusFetcher ClearRepository()
        {
            _liveCountryCovidStatusRepository.Clear();
            return this;
        }
    }
}
