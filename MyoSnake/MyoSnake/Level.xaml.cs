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
using Windows.UI;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace MyoSnake
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Level : Page
    {
        Grid grid = new Grid();
        Snake player1 = new Snake();

        DispatcherTimer timer = new DispatcherTimer();

        public Level()
        {
            this.InitializeComponent();

            // setup timer

            // set interval time
            timer.Interval = TimeSpan.FromMilliseconds(800);

            // set tick handler
            timer.Tick += Timer_Tick;

            // initialise level
            Init();

        }

        // handler for tick event
        private void Timer_Tick(object sender, object e)
        {

            // draw the player
            drawPlayer();

        } // Timer_Tick()

        private void Init()
        {

            int rowCount = 32;
            int colCount = 32;

            // grid.Width = 400;
            // grid.Height = 400;
            grid.Margin = new Thickness(20);
            grid.HorizontalAlignment = HorizontalAlignment.Stretch;
            grid.VerticalAlignment = VerticalAlignment.Stretch;
            grid.BorderThickness = new Thickness(4);
            grid.BorderBrush = new SolidColorBrush(Colors.Black);
            
            StackPanel sp;
            double spWidth = grid.Width / colCount;
            double spHeight = grid.Height / rowCount;

            // create the rows
            for (int i = 0; i < rowCount; i++)
            {
                grid.RowDefinitions.Insert(i, new RowDefinition());
            } // for

            // create columns
            for (int j = 0; j < colCount; j++)
            {
                grid.ColumnDefinitions.Insert(j, new ColumnDefinition());
            } // for

           
            // build the grid
            for (int i = 0; i < grid.RowDefinitions.Count; i++)
            {
                for (int j = 0; j < grid.ColumnDefinitions.Count; j++)
                {
                    
                    sp = new StackPanel();
                    //sp.Width = spWidth;
                    //sp.Height = spHeight;
                    sp.BorderThickness = new Thickness(2);
                    sp.BorderBrush = new SolidColorBrush(Windows.UI.Colors.Black);
                    sp.Background = new SolidColorBrush(Windows.UI.Colors.Blue);

                    // set stackpanels row and column index
                    sp.SetValue(Grid.RowProperty, i);
                    sp.SetValue(Grid.ColumnProperty, j);

                    // add tag to find it later
                    sp.Tag = i + "." + j;

                    // add stackpanel to grid
                    grid.Children.Add(sp);
                } // for
            } // for

            // add grid to page
            mainGrid.Children.Add(grid);

            // start the timer to draw the player
            timer.Start();

        } // Init()

        // draws the player on the screen
        private void drawPlayer()
        {
           
            foreach(var item in grid.Children)
            {
                var sp = item as StackPanel;
                string[] index = sp.Tag.ToString().Split('.');
                int row = Convert.ToInt32(index[0]);
                int col = Convert.ToInt32(index[1]);

               // System.Diagnostics.Debug.WriteLine("Panels X: " + row + " and Y: " + col);

               // sp.Background = new SolidColorBrush(Colors.Red);

                foreach (var bodyPart in player1.Body)
                {

                    System.Diagnostics.Debug.WriteLine("Body X: " + bodyPart.PosX + " and Y: " + bodyPart.PosY);
                    if (bodyPart.PosX == row && bodyPart.PosY == col)
                    {
                        // draw the part
                        sp.Background = new SolidColorBrush(Colors.Red);

                        // break out of loop
                        break;

                    } // if

                } // for

                // System.Diagnostics.Debug.WriteLine(sp.Tag);


            } // for

        } // drawPlayer()
    }
}
