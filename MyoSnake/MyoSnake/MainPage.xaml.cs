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
using MyoSharp.Communication;
using MyoSharp.Device;
using MyoSharp.Exceptions;
using MyoSharp.Poses;
using Windows.UI;
using System.Diagnostics;
using MyoSnake.Classes;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace MyoUWP
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private MyoManager myoManager = MyoManager.getInstance();

        public MainPage()
        {
            this.InitializeComponent();
        }


        private void Start_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MyoSnake.Selection));
        }

        private void HighScore_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MyoSnake.HighScores));
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);

            // save if using myo
            if (dontUseMyoCB.IsChecked == true) // don't use the myo
            {
                myoManager.UseMyo = false;

            }
            else // use the myo
            {
                myoManager.UseMyo = true;
            } // if

            if (myoManager.UseMyo)
            {
                myoManager.connect();
            }

        }
    }
}
