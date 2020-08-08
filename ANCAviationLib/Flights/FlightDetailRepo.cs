using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Text.Json;

namespace ANCAviationLib.Flights
{
    public class FlightDetailRepository
    {
        
        internal ObservableCollection<FlightDetails> _flightDetailRepo = new ObservableCollection<FlightDetails>();
        
        internal void Add(string flightJson)
        {
            JsonSerializerOptions options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            FlightDetails flightDetails = JsonSerializer.Deserialize<FlightDetails>(flightJson, options);
            _flightDetailRepo.Add(flightDetails);
        }
        public void Clear()
        {
            _flightDetailRepo.Clear();
        }
    }
}
