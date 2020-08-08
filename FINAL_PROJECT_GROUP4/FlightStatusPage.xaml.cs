using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using ANCAviationLib.Flights;
using ANCAviationLib.Airports;
using System.Collections.ObjectModel;
using Windows.Storage.Pickers;
using Windows.Storage;
using ANCAviationLib.DataAccessLayer;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace FINAL_PROJECT_GROUP4
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class FlightStatusPage : Page
    {
        private string FlightNumberFilter { set; get; }
        private DateTime? FlightDateFilter { get; set; }
        private string AirlineIataFilter { get; set; }
        private string DepartureIataFilter { set; get; }
        private string ArrivalIataFilter { set; get; }
        private FlightFetcher _flightFetcher = new FlightFetcher();
        public FlightStatusPage()
        {
            this.InitializeComponent();
        }

        private void SearchOnClick(object sender, RoutedEventArgs e)
        {
            _flightFetcher.ClearFetcher()
                .FilterByAirlineIata(AirlineIataFilter)
                .FilterByEndpointIata(Endpoints.Arrival, ArrivalIataFilter)
                .FilterByEndpointIata(Endpoints.Departure, DepartureIataFilter)
                .FilterByFlightDate(FlightDateFilter)
                .FilterByFlightNumber(FlightNumberFilter)
                .FetchRawFromApi().ProcessFetch();
        }

        private void ClearOnClick(object sender, RoutedEventArgs e)
        {
            TxtBoxArrIata.Text = "";
            TxtBoxDptIata.Text = "";
            TxtBoxFlightNo.Text = "";
            TxtBoxAirlineIata.Text = "";
            DatePckrFlightDate.SelectedDate = null;
        }

        private void NavigateToWeatherOnClick(object sender, RoutedEventArgs e)
        {
            AirportFetcher fetcher = new AirportFetcher();
            fetcher.Code = ((FlightDetails)LstFlights.SelectedItem).Arrival.Iata;
            fetcher.FetchRawFromApi().ProcessFetch();
            var lon = fetcher.FetchedAirport.Longitude;
            var lat = fetcher.FetchedAirport.Latitude;
            Frame.Navigate(typeof(WeatherStatusPage), new double[] { lat, lon });
        }

        private void NavigateToCovidOnClick(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(CovidStatusPage), ((FlightDetails)LstFlights.SelectedItem).Departure.Iata);
        }

        private async void SaveOnClick(object sender, RoutedEventArgs e)
        {
            FileSavePicker savePicker = new FileSavePicker();
            savePicker.SuggestedStartLocation = PickerLocationId.Desktop;
            savePicker.FileTypeChoices.Add("Json", new List<String> { ".json" });
            savePicker.SuggestedFileName = "SavedFlight.json";
            StorageFile storageFile = await savePicker.PickSaveFileAsync();
            JsonSaver.Save<FlightDetails>(storageFile.Path, (FlightDetails)LstFlights.SelectedItem);
        }
    }
}
