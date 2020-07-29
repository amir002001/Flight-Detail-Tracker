using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace ANCAviationLib.Flights
{
    public class FlightDetailRepository
    {
        List<FlightDetails> _flightDetailRepo = new List<FlightDetails>();
        internal void Add(string flightJson)
        {
            JsonSerializerOptions options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            FlightDetails flightDetails = JsonSerializer.Deserialize<FlightDetails>(flightJson, options);
            _flightDetailRepo.Add(flightDetails);
        }

        internal void Clear()
        {
            _flightDetailRepo.Clear();
        }
    }
}
