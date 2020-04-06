using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AsyncAwait1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// go to https://the-one-api.herokuapp.com/ to get the access token to be used for accessing the LOTR WebAPI.
        /// While accessing the API send this token in Authorization header as "Bearer [[access token]]" e.g. "Bearer qwerty1234_567"
        /// </summary>
        string accesstoken = "Bearer [your access token]";
        public MainWindow()
        {
            InitializeComponent();
            AppProgressBar.Visibility = Visibility.Hidden;
            ClearGridButton.Visibility = Visibility.Hidden;
            SearchButton.Width = 200;
        }


        private async void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            AppProgressBar.Visibility = Visibility.Visible;

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Authorization", accesstoken);
                var url = "https://the-one-api.herokuapp.com/v1/character";

                var response = await client.GetAsync(url);

                try
                {
                    response.EnsureSuccessStatusCode();

                    var content = await response.Content.ReadAsStringAsync();

                    var LOTRCharacters = JsonConvert.DeserializeObject<LOTRCharacter>(content);

                    LOTRCharacterGrid.ItemsSource = LOTRCharacters.Docs;

                    SearchButton.Width = 90;

                    ClearGridButton.Visibility = Visibility.Visible;

                }
                catch (Exception ex)
                {

                    AppNotesTextBlock.Text = ex.Message;
                }

            }

            AppProgressBar.Visibility = Visibility.Hidden;

        }

        private void ClearGridButton_Click(object sender, RoutedEventArgs e)
        {
            if (LOTRCharacterGrid.ItemsSource != null)
            {
                LOTRCharacterGrid.ItemsSource = null;

                SearchButton.Width = 200;

                ClearGridButton.Visibility = Visibility.Hidden;
            }
        }

        //private async void SearchButton_Click(object sender, RoutedEventArgs e)
        //{
        //    AppProgressBar.Visibility = Visibility.Visible;

        //    var region = RegionTextBox.Text;
        //    var rapidAPIKey = string.IsNullOrWhiteSpace(RapidAPIKey.Text) ? apiKey : RapidAPIKey.Text;


        //    using (var client = new HttpClient())
        //    {
        //        client.DefaultRequestHeaders.Add("x-rapidapi-host", "apidojo-yahoo-finance-v1.p.rapidapi.com");
        //        client.DefaultRequestHeaders.Add("x-rapidapi-key", rapidAPIKey);


        //        var url = $"https://apidojo-yahoo-finance-v1.p.rapidapi.com/market/get-summary?region={region}&lang=en";

        //        var response = await client.GetAsync(url);

        //        try
        //        {
        //            response.EnsureSuccessStatusCode();

        //            var content = await response.Content.ReadAsStringAsync();

        //        }
        //        catch (Exception ex)
        //        {

        //            AppNotesTextBlock.Text = ex.Message;
        //        }

        //    }

        //    AppProgressBar.Visibility = Visibility.Hidden;

        //}
    }

    internal class LOTRCharacter
    {
        public List<CharacterInfo> Docs { get; set; }
    }

    internal class CharacterInfo
    {
        public string Name { get; set; }
        public string Gender { get; set; }
        public string Race { get; set; }
        public string Birth { get; set; }
        public string Death { get; set; }
        public string Realm { get; set; }
        public string WikiUrL { get; set; }
    }
}
