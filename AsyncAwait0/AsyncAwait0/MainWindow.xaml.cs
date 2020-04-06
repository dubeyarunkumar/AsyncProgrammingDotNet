using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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

namespace AsyncAwait0
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //string apiKey = "c037fd79a0msh45bb7cba3853769p18302cjsnf5af9c082f02";
        string accesstoken = "Bearer azhjXEBXKSG-5Hg_snbg";
        public MainWindow()
        {
            InitializeComponent();
            AppProgressBar.Visibility = Visibility.Visible;
            ClearGridButton.Visibility = Visibility.Hidden;
            SearchButton.Width = 200;
        }


        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            AppProgressBar.Visibility = Visibility.Visible;

            var url = "https://the-one-api.herokuapp.com/v1/character";
            var client = new WebClient();
            client.Headers.Add("Authorization", accesstoken);

            var response = client.DownloadString(url);

            var LOTRCharacters = JsonConvert.DeserializeObject<LOTRCharacter>(response);

            LOTRCharacterGrid.ItemsSource = LOTRCharacters.Docs;

            SearchButton.Width = 90;

            ClearGridButton.Visibility = Visibility.Visible;

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
