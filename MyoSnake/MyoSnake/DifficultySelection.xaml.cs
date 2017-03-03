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
    public sealed partial class DifficultySelection : Page
    {
        private gameSettings settings;

        public DifficultySelection()
        {
            this.InitializeComponent();
        }

        private void btnEasy_Click(object sender, RoutedEventArgs e)
        {
            //pass player argument and difficulty argument
            settings.Diff = 0;
            //this.Frame.Navigate(typeof(MyoSnake.Level,settings));
        }

        private void btnNormal_Click(object sender, RoutedEventArgs e)
        {
            //pass player argument and difficulty argument
            settings.Diff = 1;
            //this.Frame.Navigate(typeof(MyoSnake.Level));
        }

        private void btnHard_Click(object sender, RoutedEventArgs e)
        {
            //pass player argument and difficulty argument
            settings.Diff = 2;
            //this.Frame.Navigate(typeof(MyoSnake.Level));
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            settings = (gameSettings)e.Parameter;
        }
    }
}
