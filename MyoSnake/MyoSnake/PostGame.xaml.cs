using MyoSnake.Classes;
using MyoUWP;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace MyoSnake
{
    public sealed partial class PostGame : Page
    {
        private gameSettings settings;

        public PostGame()
        {
            this.InitializeComponent();

        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            // get the settings passed
            settings = (gameSettings)e.Parameter;
            lblMessage.Text = settings.Message;

            showPopupPlayer1();
        }

        private async void showPopupPlayer1()
        {
            var dialog1 = new ContentDialog1();
            dialog1.Title = "Enter Player One Name";
            var result = await dialog1.ShowAsync();
            if (result == ContentDialogResult.Primary)
            {
                var text = dialog1.Text;
                if (text!=null)
                {
                    postToServer(text, settings.Player1Score.ToString());
                }
               
                
            }

            if (settings.Players == 2)
            {
                showPopupPlayer2();
            }

        }

        private async void showPopupPlayer2()
        {
            var dialog2 = new ContentDialog1();
            dialog2.Title = "Enter Player Two Name";
            var result = await dialog2.ShowAsync();
            if (result == ContentDialogResult.Primary)
            {
                var text = dialog2.Text;
                if (text != null)
                {
                    postToServer(text, settings.Player2Score.ToString());
                }
            }
        }

        private void btnReplay_Click(object sender, RoutedEventArgs e)
        {
            // navigate back to the level game, passing the same settings
            Frame.Navigate(typeof(Level), settings);
        }

        private void btnMenu_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(MainPage));
        }

        private async void postToServer(string name, string score)
        {
            var client = new HttpClient();

            var pairs = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("name", name),
                new KeyValuePair<string, string>("score", score)
            };

            var content = new FormUrlEncodedContent(pairs);

            var response = client.PostAsync("http://52.169.18.184:3000/api/postScore", content).Result;

            if (response.IsSuccessStatusCode)
            {
                //Debug.WriteLine("Success");
                var messageDialog = new MessageDialog("Score submitted to server.");

                await messageDialog.ShowAsync();
            }
            else
            {
                Debug.WriteLine("Error");
            }

        }
    }
}
