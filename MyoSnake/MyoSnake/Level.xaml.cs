﻿using System;
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
using System.Diagnostics;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace MyoSnake
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Level : Page
    {
        Random ran = new Random();
        Grid grid = new Grid();
        Dictionary<string, StackPanel> gameBoard = new Dictionary<string, StackPanel>();
        Snake player1 = new Snake();
        Pickup pickup = new Pickup();
        int boardSize = 32;

        DispatcherTimer timer = new DispatcherTimer();

        public Level()
        {
            this.InitializeComponent();

            // setup timer

            // set interval time
            timer.Interval = TimeSpan.FromMilliseconds(400);

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

            placePickup();

        } // Timer_Tick()

        private void Init()
        {

            int rowCount = boardSize;
            int colCount = boardSize;

            // grid.Width = 400;
            // grid.Height = 400;
            grid.Margin = new Thickness(20, 100, 20, 20);
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

                    // add stackpanel to gameBoard dictionary
                    gameBoard.Add(sp.Tag.ToString(), sp);

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

            StackPanel sp = null;

            //// reset old last player body part

            //// try and get the stackpanel at the position the body part is at
            //gameBoard.TryGetValue(player1.Tail.PosX + "." + player1.Tail.PosY, out sp);

            //// if a panel is there
            //if (sp != null)
            //{
            //    // draw the part
            //    sp.Background = new SolidColorBrush(Colors.Blue);

            //} // if

            // loop through each body part
            foreach (var bodyPart in player1.Body)
            {

                // check if head of snake as hit a wall

                sp = null;

                // try and get the stackpanel at the position the body part is at
                gameBoard.TryGetValue(bodyPart.PosX + "." + bodyPart.PosY, out sp);

                // if a panel is there
                if (sp != null)
                {
                    // draw the part
                    sp.Background = new SolidColorBrush(Colors.Blue);

                } // if

            } // for

            // move the player
            player1.Move();

            // loop through each body part
            foreach (var bodyPart in player1.Body)
            {

                // check if head of snake as hit a wall
           
                sp = null;

                // try and get the stackpanel at the position the body part is at
                gameBoard.TryGetValue(bodyPart.PosX + "." + bodyPart.PosY, out sp);

                // if a panel is there
                if(sp != null)
                {
                    // draw the part
                    sp.Background = new SolidColorBrush(Colors.Red);

                } // if

            } // for

            sp = null;

            // try and get the stackpanel at the position the body part is at
            gameBoard.TryGetValue(player1.Head.PosX + "." + player1.Head.PosY, out sp);

            // if a panel is there
            if (sp != null)
            {
                // draw the part
                sp.Background = new SolidColorBrush(Colors.Orange);

            } // if

        } // drawPlayer()

        // places the pickup for the player
        private void placePickup()
        {

            StackPanel sp = null;

            // remove old pickup
           
            // try and get the stackpanel at the position
            gameBoard.TryGetValue(pickup.PosX + "." + pickup.PosY, out sp);

            // if a panel is there
            if (sp != null)
            {
                // draw the part
                sp.Background = new SolidColorBrush(Colors.Blue);

            } // if
          
            // generate X coord
            pickup.PosX = generateCoord();

            // generate Y coord
            pickup.PosY = generateCoord();

            // check that spot is not taken

            // place the pickup

            // try and get the stackpanel at the position
            gameBoard.TryGetValue(pickup.PosX + "." + pickup.PosY, out sp);

            // if a panel is there
            if (sp != null)
            {
                // draw the part
                sp.Background = new SolidColorBrush(Colors.Orange);

            } // if

        } // placePickup()

        private int generateCoord()
        {
            int coord = 0;

            // generate a random number
            coord = ran.Next(boardSize);

            return coord;

        } // generateCoord()

        private void leftBtn_Click(object sender, RoutedEventArgs e)
        {
            player1.MoveLeft();

        }

        private void rightBtn_Click(object sender, RoutedEventArgs e)
        {
            player1.MoveRight();
        }
    }
}
