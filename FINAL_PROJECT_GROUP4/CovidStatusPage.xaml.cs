using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage.Pickers;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using ANCAviationLib.COVID;
using Windows.Storage;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace FINAL_PROJECT_GROUP4
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class CovidStatusPage : Page
    {
        LiveCountryCovidStatus _fetchedCovidStatus;
        LiveCovidStatusFetcher _covidFetcher = new LiveCovidStatusFetcher() ;
        public CovidStatusPage()
        {
            this.InitializeComponent();
            _covidFetcher.Code = "IT";
 
        }
        private void SetDetails()
        {

        }

        private async void SaveOnClick(object sender, RoutedEventArgs e)
        {
            if (_fetchedCovidStatus == null)
            {
                return;
            }
            FileSavePicker savePicker = new FileSavePicker();
            savePicker.SuggestedStartLocation = PickerLocationId.Desktop;
            savePicker.FileTypeChoices.Add("Json", new List<String> { ".json" });
            savePicker.SuggestedFileName = "SavedData.json";
            StorageFile storageFile = await savePicker.PickSaveFileAsync();

            JsonSaver.Save<LiveCountryCovidStatus>(await storageFile.OpenStreamForWriteAsync(), _fetchedCovidStatus);

        }

    }

}
