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
using ANCAviationLib.Flights;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace FINAL_PROJECT_GROUP4
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class WeatherStatusPage : Page
    {
        private WeatherStatusFetcher _weatherFeature = new WeatherStatusFetcher();
        public int TempC { get; set; }
        public int TemoF { get; set; }
        public int Humidity { get; set; }
        public int Windspeed { get; set; }
        public int Weather { get; set; }
        public int Sunrise { get; set; }
        public int Sunset { get; set; }
        public int Pressure { get; set; }

        public WeatherStatusPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            object FlightFetcher = base.OnNavigatedFrom(e);
        }

        private void Clear_Button(object sender, RoutedEventArgs e)
        {
            TempC.Text = "";
            TempF.Text = "";
            Humidity.Text = "";
            Windspeed.Text = "";
            Weather.Text = "";
            Sunrise.Text = "";
            Sunset.Text = "";
            Pressure.Text = "";
        }
        private void Back_Button(object sender, RoutedEventArgs e)
        {

        }
    }
}
