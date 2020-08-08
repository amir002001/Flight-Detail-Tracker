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
using Windows.UI.Xaml.Media.Imaging;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace FINAL_PROJECT_GROUP4
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class FlightStatusPage : Page
    {
        private string FlightNumberFilter { set; get; }
        private string AirlineIataFilter { get; set; }
        private string DepartureIataFilter { set; get; }
        private string ArrivalIataFilter { set; get; }
        private FlightFetcher _flightFetcher = new FlightFetcher();
        private AirportFetcher _airportFetcher = new AirportFetcher();
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
                .FilterByFlightNumber(FlightNumberFilter)
                .FetchRawFromApi().ProcessFetch();
        }

        private void ClearOnClick(object sender, RoutedEventArgs e)
        {
            TxtBoxArrIata.Text = "";
            TxtBoxDptIata.Text = "";
            TxtBoxFlightNo.Text = "";
            TxtBoxAirlineIata.Text = "";
        }

        private void NavigateToWeatherOnClick(object sender, RoutedEventArgs e)
        {
            _airportFetcher.Code = ((FlightDetails)LstFlights.SelectedItem).Arrival.Iata;
            _airportFetcher.FetchRawFromApi().ProcessFetch();
            var lon = _airportFetcher.FetchedAirport.Longitude;
            var lat = _airportFetcher.FetchedAirport.Latitude;
            Frame.Navigate(typeof(WeatherStatusPage), new double[] { lat, lon });
        }

        private void NavigateToCovidOnClick(object sender, RoutedEventArgs e)
        {
            _airportFetcher.Code = ((FlightDetails)LstFlights.SelectedItem).Departure.Iata;
            Frame.Navigate(typeof(CovidStatusPage), _airportFetcher.FetchedAirport.Country_Iso);
        }

        private async void SaveOnClick(object sender, RoutedEventArgs e)
        {
            FileSavePicker savePicker = new FileSavePicker();
            savePicker.SuggestedStartLocation = PickerLocationId.Desktop;
            savePicker.FileTypeChoices.Add("Json", new List<String> { ".json" });
            savePicker.SuggestedFileName = "SavedFlight.json";
            StorageFile storageFile = await savePicker.PickSaveFileAsync();
            
            JsonSaver.Save<FlightDetails>(await storageFile.OpenStreamForWriteAsync(), (FlightDetails)LstFlights.SelectedItem);
        }

        private void DisplayDetailsOnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (LstFlights.SelectedItem == null)
                return;
            _airportFetcher.Code = ((FlightDetails)LstFlights.SelectedItem).Departure.Iata;
            _airportFetcher.FetchRawFromApi().ProcessFetch();
            ImgDpt.Source = new BitmapImage(new Uri(GetFlagPath(_airportFetcher.FetchedAirport.Country_Iso)));
            _airportFetcher.Code = ((FlightDetails)LstFlights.SelectedItem).Arrival.Iata;
            _airportFetcher.FetchRawFromApi().ProcessFetch();
            ImgArr.Source = new BitmapImage(new Uri(GetFlagPath(_airportFetcher.FetchedAirport.Country_Iso)));
        }
        private string GetFlagPath(string countryIso) => $"ms-appx:///Assets/Flags/{countryIso}.png";
    }
}
