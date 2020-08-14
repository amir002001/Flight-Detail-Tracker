using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Text.Json;

namespace ANCAviationLib.Flights
{
    /// <summary>
    /// A repository holding flights with the ability to add based on a json string.
    /// </summary>
    public class FlightDetailRepository
    {
        
        internal ObservableCollection<FlightDetails> _flightDetailRepo = new ObservableCollection<FlightDetails>();
        /// <summary>
        /// Adds a flight to the list based on a json string.
        /// </summary>
        /// <param name="flightJson"></param>
        internal void Add(string flightJson)
        {
            JsonSerializerOptions options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            FlightDetails flightDetails = JsonSerializer.Deserialize<FlightDetails>(flightJson, options);
            _flightDetailRepo.Add(flightDetails);
        }
        /// <summary>
        /// clears the repository.
        /// </summary>
        public void Clear()
        {
            _flightDetailRepo.Clear();
        }
    }
}
