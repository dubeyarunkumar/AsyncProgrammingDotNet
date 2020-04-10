﻿using Newtonsoft.Json;
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

namespace AsyncAwait2
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
        /// It is blocking as even though we have used getAsync() and ReadAsStringAsyn() but we are accessing the "Result" to get the data.
        /// calling result (or wait) on an async operation will block and potentially deadlock your application  
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void BlockingSearchButton_Click(object sender, RoutedEventArgs e)
        {
            AppProgressBar.Visibility = Visibility.Visible;

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Authorization", accesstoken);

                var url = "https://the-one-api.herokuapp.com/v1/character";

                var response = client.GetAsync(url);

                try
                {
                    var result = response.Result;

                    var content = result.Content.ReadAsStringAsync();

                    var contentResult = content.Result;

                    var LOTRCharacters = JsonConvert.DeserializeObject<LOTRCharacter>(contentResult);

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

        /// <summary>
        /// It is not blocking as we are using Await keyword.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
