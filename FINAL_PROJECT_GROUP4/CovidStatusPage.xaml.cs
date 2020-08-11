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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace FINAL_PROJECT_GROUP4
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class CovidStatusPage : Page
    {
        public CovidStatusPage()
        {
            this.InitializeComponent();
        }
    }
   // private async void SaveOnClick(object sender, RoutedEventArgs e)
   //    {
           // FileSavePicker savePicker = new FileSavePicker();
//           savePicker.SuggestedStartLocation = PickerLocationId.Desktop;
//           savePicker.FileTypeChoices.Add("Json", new List<String> { ".json" });
//           savePicker.SuggestedFileName = "SavedFlight.json";
//           StorageFile storageFile = await savePicker.PickSaveFileAsync();
//
//         JsonSaver.Save<FlightDetails>(await storageFile.OpenStreamForWriteAsync(), _selectedFlight);
     //  }
}
