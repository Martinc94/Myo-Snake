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
        public Level()
        {
            this.InitializeComponent();

            // initialise level
            Init();

        }

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

                    // add stackpanel to grid
                    grid.Children.Add(sp);
                } // for
            } // for

            // add grid to page
            mainGrid.Children.Add(grid);
            
        } // Init()
    }
}
