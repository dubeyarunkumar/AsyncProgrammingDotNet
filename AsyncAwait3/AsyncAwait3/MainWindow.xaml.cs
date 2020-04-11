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

namespace AsyncAwait3
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

        /// <summary>
        /// It is not blocking as we are using Await keyword.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            AppProgressBar.Visibility = Visibility.Visible;

            await GetLOTRCharacters();

            SearchButton.Width = 90;

            ClearGridButton.Visibility = Visibility.Visible;

            AppProgressBar.Visibility = Visibility.Hidden;
        }

        private async Task GetLOTRCharacters()
        {
            using (var client = new HttpClient())
            {
                var url = "https://the-one-api.herokuapp.com/v1/character";
                client.DefaultRequestHeaders.Add("Authorization", accesstoken);

                var response = await client.GetAsync(url);

                var content = await response.Content.ReadAsStringAsync();

                var LOTRCharacters = JsonConvert.DeserializeObject<LOTRCharacter>(content);

                LOTRCharacterGrid.ItemsSource = LOTRCharacters.Docs;
            }
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
