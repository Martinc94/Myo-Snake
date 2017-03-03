using MyoSnake.Classes;
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

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace MyoSnake
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Selection : Page
    {
        public Selection()
        {
            this.InitializeComponent();
        }

        private void btnOnePlayer_Click(object sender, RoutedEventArgs e)
        {
            //pass one player argument
            var settings = new gameSettings();
            settings.Players = 1;

            this.Frame.Navigate(typeof(MyoSnake.DifficultySelection),settings);
        }

        private void btnTwoPlayer_Click(object sender, RoutedEventArgs e)
        {
            //pass two player argument
            var settings = new gameSettings();
            settings.Players = 2;

            this.Frame.Navigate(typeof(MyoSnake.DifficultySelection),settings);
        }
    }
}
