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
using System.Diagnostics;
using Windows.Storage;
using MyoSnake.Classes;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace MyoSnake
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Level : Page
    {
        MyoManager myoManager = null;
        Random ran = new Random();
        static int boardSize = 32;
        Grid grid = new Grid();
        Dictionary<string, StackPanel> gameBoard = new Dictionary<string, StackPanel>();
        Snake player1 = new Snake(boardSize);

        SolidColorBrush backgroundColour = new SolidColorBrush(Colors.SeaGreen);
       // SolidColorBrush backgroundColour = new SolidColorBrush(Colors.OliveDrab);
        SolidColorBrush pickUpColour = new SolidColorBrush(Colors.DarkOrange);

        SolidColorBrush player1BodyColour = new SolidColorBrush(Colors.LimeGreen);
        SolidColorBrush player1HeadColour = new SolidColorBrush(Colors.Lime);
        SolidColorBrush player2BodyColour = new SolidColorBrush(Colors.Green);
        SolidColorBrush player2HeadColour = new SolidColorBrush(Colors.Yellow);

        Pickup pickup = new Pickup();
        Boolean player1Moved = false;
        Boolean pickupPlaced = false;
       
        DispatcherTimer timer = new DispatcherTimer();
        gameSettings settings = null;

        public Level()
        {
            // get instance of MyoManager
            myoManager = MyoManager.getInstance();

            this.InitializeComponent();

            // setup timer

            // set interval time
            timer.Interval = TimeSpan.FromMilliseconds(400);

            // set tick handler
            timer.Tick += Timer_Tick;

            // initialise level
            Init();

        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            settings = (gameSettings)e.Parameter;
        }

        // handler for tick event
        private void Timer_Tick(object sender, object e)
        {

            // draw the player
            drawPlayer();

            if (pickupPlaced == false)
            {
                // place the pickup
                placePickup();

                // flag as placed
                pickupPlaced = true;

            } // if

            // reset player move count
            player1Moved = false;

        } // Timer_Tick()

        private void Init()
        {

            int rowCount = boardSize;
            int colCount = boardSize;

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
                    sp.BorderBrush = backgroundColour;
                    sp.Background = backgroundColour;

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
            Boolean increasePlayer1Size = false;

            // reset old last player body parts

            // loop through each body part
            foreach (var bodyPart in player1.Body)
            {

                // check if head of snake as hit a wall

                sp = null;

                // try and get the stackpanel at the position the body part is at
                gameBoard.TryGetValue(bodyPart.PosY + "." + bodyPart.PosX, out sp);

                // if a panel is there
                if (sp != null)
                {
                    // draw the part
                    sp.Background = backgroundColour;

                } // if

            } // for

            // move the player
            player1.Move();

            // loop through each body part
            foreach (var bodyPart in player1.Body)
            {
           
                sp = null;

                // try and get the stackpanel at the position the body part is at
                gameBoard.TryGetValue(bodyPart.PosY + "." + bodyPart.PosX, out sp);

                // if a panel is there
                if(sp != null)
                {

                    // check of pickup is placed
                    if(sp.Background == pickUpColour)
                    {
                        // increase score

                        // flag player 1 for body size increase
                        increasePlayer1Size = true;

                        // place pickup
                        placePickup();
                    }

                    // draw the players body
                    sp.Background = player1BodyColour;

                } // if

            } // for

            sp = null;

            // try and get the stackpanel at the position the body part is at
            gameBoard.TryGetValue(player1.Head.PosY + "." + player1.Head.PosX, out sp);

            // if a panel is there
            if (sp != null)
            {
                // draw the players head
                sp.Background = player1HeadColour;

            } // if

            // check if player needs to get bigger
            if (increasePlayer1Size)
            {
                // increase player 1 size
                player1.IncreaseBodySize();

                // reset flag
                increasePlayer1Size = false;

            } // if

        } // drawPlayer()

        // places the pickup for the player
        private void placePickup()
        {

            StackPanel sp = null;
            Boolean isFree = false;
            int posX;
            int posY;

            // remove old pickup
           
            // try and get the stackpanel at the position
            gameBoard.TryGetValue(pickup.PosY + "." + pickup.PosX, out sp);

            // if a panel is there
            if (sp != null)
            {
                // draw the part
                sp.Background = backgroundColour;

            } // if

            // generate pickup position until a free spot is generated
            do
            {
                // generate X coord
                posX = generateCoord();

                // generate Y coord
                posY = generateCoord();

                // try and get the stackpanel at the position
                gameBoard.TryGetValue(posY + "." + posX, out sp);

                // if a panel is there
                if (sp != null)
                {
                    // check if stackpanel is the background
                    if (sp.Background == backgroundColour)
                    {
                        // set the coords
                        pickup.PosX = posX;
                        pickup.PosY = posY;

                        // flag as free
                        isFree = true;
                    }
                    else
                    {
                        // otherwise, position not free
                        isFree = false;

                        //Debug.WriteLine("Position not free!");
       
                    } // if

                } // if

            } while (isFree == false); // do while

            // place the pickup
            if(sp != null)
            {

                // draw the pickup
                sp.Background = pickUpColour;

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
            if (player1Moved == false)
            {
                // move the player
                player1.MoveLeft();

                // decrease move count
                player1Moved = true;

            } // if

        }

        private void rightBtn_Click(object sender, RoutedEventArgs e)
        {

            if (player1Moved == false)
            {
                // move the player
                player1.MoveRight();

                // decrease move count
                player1Moved = true;

            } // if
        }
    }
}
