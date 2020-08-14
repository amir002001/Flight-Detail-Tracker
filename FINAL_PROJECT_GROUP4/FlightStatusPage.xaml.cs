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
using System.ComponentModel;
using System.Net;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace FINAL_PROJECT_GROUP4
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class FlightStatusPage : Page
    {
        private FlightFetcher _flightFetcher = new FlightFetcher();
        private AirportFetcher _airportFetcher = new AirportFetcher();
        private string FlightNumberFilter { set; get; }
        private string AirlineIataFilter { get; set; }
        private string DepartureIataFilter { set; get; }
        private string ArrivalIataFilter { set; get; }
        private FlightDetails _selectedFlight;

        public FlightStatusPage()
        {
            this.InitializeComponent();
        }
        /// <summary>
        /// gets the results from flight fetcher.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SearchOnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                _flightFetcher.ClearFetcher()
                    .FilterByAirlineIata(AirlineIataFilter)
                    .FilterByEndpointIata(Endpoints.Arrival, ArrivalIataFilter)
                    .FilterByEndpointIata(Endpoints.Departure, DepartureIataFilter)
                    .FilterByFlightNumber(FlightNumberFilter)
                    .FetchRawFromApi().ProcessFetch();
                TxtErr.Text = "";
            }
            catch (System.Net.WebException ex)
            {
                TxtErr.Text = $"{ex.Message}{Environment.NewLine}{ex.InnerException.Message}";
            }
            catch (InvalidOperationException ex)
            {
                TxtErr.Text = "Invalid Operation!" + ex.Message;
            }
            catch (Exception ex)
            {
                TxtErr.Text = ex.Message;
            }
        }
        /// <summary>
        /// clears boxes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ClearOnClick(object sender, RoutedEventArgs e)
        {
            TxtBoxArrIata.Text = "";
            TxtBoxDptIata.Text = "";
            TxtBoxFlightNo.Text = "";
            TxtBoxAirlineIata.Text = "";
        }
        /// <summary>
        /// Navigates to WeatherStatusPage with a lat and long double array.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NavigateToWeatherOnClick(object sender, RoutedEventArgs e)
        {
            if (_selectedFlight == null)
                return;
            try
            {
                _airportFetcher.Code = ((bool)DepartureButton.IsChecked) ? _selectedFlight.Departure.Iata : _selectedFlight.Arrival.Iata;
                _airportFetcher.FetchRawFromApi().ProcessFetch();
                var lon = _airportFetcher.FetchedAirport.Longitude;
                var lat = _airportFetcher.FetchedAirport.Latitude;
                Frame.Navigate(typeof(WeatherStatusPage), new double[] { lat, lon });
                TxtErr.Text = "";
            }
            catch (WebException ex)
            {
                TxtErr.Text = $"{ex.Message}{Environment.NewLine}{ex.InnerException?.Message}";
            }
            catch (Exception ex) { TxtErr.Text = ex.Message; }
        }
        /// <summary>
        /// Navigates to Covid page with a country iso code.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NavigateToCovidOnClick(object sender, RoutedEventArgs e)
        {
            if (_selectedFlight == null)
                return;
            try
            {
                _airportFetcher.Code = ((bool)DepartureButton.IsChecked) ? _selectedFlight.Departure.Iata : _selectedFlight.Arrival.Iata;
                _airportFetcher.FetchRawFromApi().ProcessFetch();
                Frame.Navigate(typeof(CovidStatusPage), _airportFetcher.FetchedAirport.Country_Iso);
                TxtErr.Text = "";
            }
            catch (WebException ex)
            {
                TxtErr.Text = $"{ex.Message}{Environment.NewLine}{ex.InnerException?.Message}";
            }
            catch (Exception ex) { TxtErr.Text = ex.Message; }

        }
        /// <summary>
        /// Saves the selected flight using Data Contract Serializier.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void SaveOnClick(object sender, RoutedEventArgs e)
        {
            if (_selectedFlight == null)
                return;
            try
            {
                FileSavePicker savePicker = new FileSavePicker();
                savePicker.SuggestedStartLocation = PickerLocationId.Desktop;
                savePicker.FileTypeChoices.Add("Json", new List<String> { ".json" });
                savePicker.SuggestedFileName = "SavedFlight.json";
                StorageFile storageFile = await savePicker.PickSaveFileAsync();

                JsonSaver.Save<FlightDetails>(await storageFile.OpenStreamForWriteAsync(), _selectedFlight);
                TxtErr.Text = "";
            }
            catch (Exception ex)
            {
                TxtErr.Text = "There was an error saving the file" + ex.Message;
            }
        }
        /// <summary>
        /// Displays flights details including country flags when selection changes.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DisplayDetailsOnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_selectedFlight == null)
            {
                ImgArr.Source = ImgDpt.Source = null;
                ClearDetails();
                return;
            }
            try
            {
                _airportFetcher.Code = _selectedFlight.Departure.Iata;
                _airportFetcher.FetchRawFromApi().ProcessFetch();
                ImgDpt.Source = new BitmapImage(new Uri(GetFlagPath(_airportFetcher.FetchedAirport.Country_Iso)));
                _airportFetcher.Code = _selectedFlight.Arrival.Iata;
                _airportFetcher.FetchRawFromApi().ProcessFetch();
                ImgArr.Source = new BitmapImage(new Uri(GetFlagPath(_airportFetcher.FetchedAirport.Country_Iso)));
                SetDetails();
            }
            catch (WebException ex)
            {
                TxtErr.Text = $"{ex.Message}{Environment.NewLine}{ex.InnerException?.Message}";
            }
            catch (Exception ex) { TxtErr.Text = ex.Message; }
        }
        /// <summary>
        /// Sets detail blocks to flight's value
        /// </summary>
        private void SetDetails()
        {

            try { TxtBlockDptTimeDetails.Text = DateTime.Parse(_selectedFlight.Departure.Scheduled ?? "").ToString("f"); }
            catch { TxtBlockDptTimeDetails.Text = ""; }

            try { TxtBlockArrTimeDetails.Text = DateTime.Parse(_selectedFlight.Arrival.Scheduled ?? "").ToString("f"); }
            catch { TxtBlockDptTimeDetails.Text = ""; }

            TxtBlockFNoDetails.Text = _selectedFlight.Flight.Number;
            TxtBlockAirlnNameDetails.Text = _selectedFlight.Airline.Name ?? "";
            TxtBlockAirlnIataDetails.Text = _selectedFlight.Airline.Iata ?? "";
            TxtBlockDptAirportDetails.Text = _selectedFlight.Departure.Airport ?? "";
            TxtBlockDptIataDetails.Text = _selectedFlight.Departure.Iata ?? "";
            TxtBlockArrAirportDetails.Text = _selectedFlight.Arrival.Airport ?? "";
            TxtBlockArrIataDetails.Text = _selectedFlight.Arrival.Iata ?? "";
        }
        /// <summary>
        /// Clears details
        /// </summary>
        private void ClearDetails()
        {
            TxtBlockFNoDetails.Text = "";
            TxtBlockAirlnNameDetails.Text = "";
            TxtBlockAirlnIataDetails.Text = "";
            TxtBlockDptAirportDetails.Text = "";
            TxtBlockDptIataDetails.Text = "";
            TxtBlockDptTimeDetails.Text = "";
            TxtBlockArrAirportDetails.Text = "";
            TxtBlockArrIataDetails.Text = "";
            TxtBlockArrTimeDetails.Text = "";
        }
        private string GetFlagPath(string countryIso) => $"ms-appx:///Assets/Flags/{countryIso}.png";
    }
}
