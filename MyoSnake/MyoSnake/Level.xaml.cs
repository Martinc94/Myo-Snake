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
using Windows.System;
using Windows.UI.Popups;
using MyoSharp.Device;
using MyoSharp.Poses;
using System.Threading.Tasks;


// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace MyoSnake
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Level : Page
    {
        const int EASY_GAME_SPEED = 300;
        const int NORMAL_GAME_SPEED = 250;
        const int HARD_GAME_SPEED = 200;
        const int PICKUP_SCORE = 10;
        const string PLAYER_ONE = "Player1";
        const string PLAYER_TWO = "Player2";

        MyoManager myoManager = null;
        Random ran = new Random();
        Boolean gameIsPlaying = true;
        Boolean isTwoPlayer = false;
        static int boardSize = 32;
        Grid grid = new Grid();
        Dictionary<string, StackPanel> gameBoard = new Dictionary<string, StackPanel>();
        Snake player1 = new Snake(PLAYER_ONE, boardSize, 10, 10);
        Snake player2 = new Snake(PLAYER_TWO, boardSize, 20, 10);

        SolidColorBrush backgroundColour = new SolidColorBrush(Colors.SeaGreen);
        SolidColorBrush player1BodyColour = new SolidColorBrush(Colors.LimeGreen);
        SolidColorBrush player1HeadColour = new SolidColorBrush(Colors.Lime);
        SolidColorBrush player2BodyColour = new SolidColorBrush(Colors.Orange);
        SolidColorBrush player2HeadColour = new SolidColorBrush(Colors.Yellow);

        Pickup p1Pickup = new Pickup(PLAYER_ONE);
        Pickup p2Pickup = new Pickup(PLAYER_TWO);
        Boolean p1PickupPlaced = false;
        Boolean p2PickupPlaced = false;

        DispatcherTimer timer = new DispatcherTimer();
        gameSettings settings = null;

        public Level()
        {
            // setup event handler for window size changing to update the grid
            Window.Current.SizeChanged += Current_SizeChanged;

            // get instance of MyoManager
            myoManager = MyoManager.getInstance();

            if (myoManager.UseMyo) {
                myoManager.connect();
            }

            this.InitializeComponent();

            // setup timer

            // set tick handler
            timer.Tick += Timer_Tick;

        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            // get the settings passed by the previous page
            settings = (gameSettings)e.Parameter;

           // Debug.WriteLine(settings.Diff + " - " + settings.Players);

            // initialise level
            Init(settings);
        }

        // handler for tick event
        private async void Timer_Tick(object sender, object e)
        {
            // controls if the game keeps playing
            if (gameIsPlaying)
            {
                // draw the player
                drawPlayer(player1);

                if (p1PickupPlaced == false)
                {
                    // place the pickup
                    placePickup(p1Pickup);

                    // flag as placed
                    p1PickupPlaced = true;

                } // if

                if (isTwoPlayer)
                {
                    // draw second player
                    drawPlayer(player2);

                    if (p2PickupPlaced == false)
                    {
                        // place the pickup
                        placePickup(p2Pickup);

                        // flag as placed
                        p2PickupPlaced = true;

                    } // if
                } // if

                // reset player move count
                player1.Moved = false;
                player2.Moved = false;

                // update the score on the screen
                player1ScoreTB.Text = player1.Score.ToString();

            } else
            {

                // stop the game timer
                timer.Stop();

                // show game over screen
                // Create the message dialog and set its content
                var messageDialog = new MessageDialog("Game Over! Your Score is: " + player1.Score);

                messageDialog.Commands.Add(new UICommand(
                    "OK",
                    new UICommandInvokedHandler(this.CommandInvokedHandler)));

                // Set the command to be invoked when escape is pressed
                messageDialog.CancelCommandIndex = 0;

                // Show the message dialog
                await messageDialog.ShowAsync();

            } // if

        } // Timer_Tick()

        // handler for message dialog
        private void CommandInvokedHandler(IUICommand command)
        {

            switch (command.Label)
            {
                case "OK":

                    // navigate to the next page with the current game settings
                    Frame.Navigate(typeof(PostGame), settings);
                    break;
            } // switch

        } // CommandInvokedHandler()

        private void Init(gameSettings settings)
        {
            // flag weather the game is single player or two player
            if(settings.Players == 2)
            {
                isTwoPlayer = true;
            } else
            {
                isTwoPlayer = false;
            } // if

            // set the speed that the game goes at based on difficulty
            switch (settings.Diff)
            {
                case 0: // easy
                        // set interval time
                    timer.Interval = TimeSpan.FromMilliseconds(EASY_GAME_SPEED);

                    break;
                case 1: // normal
                        // set interval time
                    timer.Interval = TimeSpan.FromMilliseconds(NORMAL_GAME_SPEED);
                    break;
                case 2: // hard
                        // set interval time
                    timer.Interval = TimeSpan.FromMilliseconds(HARD_GAME_SPEED);
                    break;
            } // switch

            // only setup the myo if it's being used
            if (myoManager.UseMyo == true)
            {
                var consumer = Task.Factory.StartNew(() =>
                {
                    // wait for the selected number of player myos to connect
                    while (myoManager.MyoEventArgsList.Count != settings.Players) { }

                    // for each connected myo
                    foreach (var myo in myoManager.MyoEventArgsList)
                    {
                        // set the post changed event handler
                        myo.Myo.PoseChanged += Myo_PoseChanged;
                    }

                    // players myos are loaded
                    // start the game loop
                    timer.Start();

                    Debug.WriteLine("Closing Thread");

                });

            } // if
            
            int rowCount = boardSize;
            int colCount = boardSize;

            // set the width and height of the grid using screen size
            if (Window.Current.Bounds.Height < Window.Current.Bounds.Width)
            {
                grid.Width = Window.Current.Bounds.Height;
                grid.Height = Window.Current.Bounds.Height - 126;

            }
            else
            {
                grid.Height = Window.Current.Bounds.Width - 126;
                grid.Width = Window.Current.Bounds.Width;

            } // if

            // Fix clipping issues by using the smaller number for width and height
            if (grid.Height > grid.Width)
            {
                grid.Height = grid.Width;
            }
            else
            {
                grid.Width = grid.Height;
            } // if
            
            grid.Margin = new Thickness(20, 100, 20, 20);
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

            // if myos are not being used
            if (myoManager.UseMyo == false)
            {
                // start the game
                timer.Start();
            } // if

        } // Init()

        // handle key presses
        protected override void OnKeyDown(KeyRoutedEventArgs e)
        {
            e.Handled = true;

            switch (e.Key)
            {
                // To handle player One controls
                case VirtualKey.Left:

                    // move player 1 left
                    movePlayerLeft(PLAYER_ONE);
                    break;
                case VirtualKey.Right:

                    // move player 1 right
                    movePlayerRight(PLAYER_ONE);
                    break;

                // to handle player 2 controls
                case VirtualKey.A:

                    // move player 2 left
                    movePlayerLeft(PLAYER_TWO);
                    break;
                case VirtualKey.D:

                    // move player 2 right
                    movePlayerRight(PLAYER_TWO);
                    break;

            } // switch
        } // OnKeyDown()

        // draws the player on the screen
        private void drawPlayer(Snake player)
        {
            StackPanel sp = null;
            Boolean increasePlayerSize = false;
            SolidColorBrush bodyColour = null;
            SolidColorBrush headColour = null;

            // set the colours, based on the player
            if(player.PlayerName == PLAYER_ONE)
            {
                bodyColour = player1BodyColour;
                headColour = player1HeadColour;
            }
            else
            {
                bodyColour = player2BodyColour;
                headColour = player2HeadColour;
            } // if

            // reset old last player body parts
            // loop through each body part
            foreach (var bodyPart in player.Body)
            {
                sp = null;

                // try and get the stackpanel at the position the body part is at
                gameBoard.TryGetValue(bodyPart.PosY + "." + bodyPart.PosX, out sp);

                // if a panel is there
                if (sp != null)
                {
                    // draw the background to replace player
                    sp.Background = backgroundColour;

                } // if
            } // for

            // move the player
            player.Move();

            // loop through each body part
            foreach (var bodyPart in player.Body)
            {
                sp = null;

                // try and get the stackpanel at the position the body part is at
                gameBoard.TryGetValue(bodyPart.PosY + "." + bodyPart.PosX, out sp);

                // if a panel is there
                if(sp != null)
                {
                    // check of pickup is placed
                    if(sp.Background == headColour)
                    {
                        // increase score
                        player.Score += PICKUP_SCORE;

                        // flag player 1 for body size increase
                        increasePlayerSize = true;

                        // remove pickup
                        removePickup(player.PlayerName);

                    }
                    else if (sp.Background == bodyColour || sp.Background == headColour) // if the player is eating itself
                    {
                        // stop the game
                        gameIsPlaying = false;

                    } // if

                    // draw the players body
                    sp.Background = bodyColour;

                } // if
            } // for

            sp = null;

            // try and get the stackpanel at the position the body part is at
            gameBoard.TryGetValue(player.Head.PosY + "." + player.Head.PosX, out sp);

            // if a panel is there
            if (sp != null)
            {
                // draw the players head
                sp.Background = headColour;

            } // if

            // check if player needs to get bigger
            if (increasePlayerSize)
            {
                // increase player 1 size
                player.IncreaseBodySize();

                // reset flag
                increasePlayerSize = false;

            } // if

        } // drawPlayer()

        // removes pickup
        private void removePickup(string playerName)
        {
            StackPanel sp = null;

            // remove old pickup
            // try and get the stackpanel at the position
            if (playerName == PLAYER_ONE)
            {

                gameBoard.TryGetValue(p1Pickup.PosY + "." + p1Pickup.PosX, out sp);
            }
            else
            {
                gameBoard.TryGetValue(p2Pickup.PosY + "." + p2Pickup.PosX, out sp);
            } // if
          
            // if a panel is there
            if (sp != null)
            {
                // draw the part
                sp.Background = backgroundColour;

                // flag pickup as removed
                if(playerName == PLAYER_ONE)
                {
                    p1PickupPlaced = false;
                } else
                {
                    p2PickupPlaced = false;
                }
            } // if
        } // removePickup()

        // places the pickup for the player
        private void placePickup(Pickup pickup)
        {
            StackPanel sp = null;
            Boolean isFree = false;
            SolidColorBrush pickupColour = null;
            int posX;
            int posY;

            if(pickup.PlayerName == PLAYER_ONE)
            {
                pickupColour = player1HeadColour;
            }
            else
            {
                pickupColour = player2HeadColour;
            }

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
                      
                    } // if
                } // if

            } while (isFree == false); // do while

            // place the pickup
            if(sp != null)
            {
                // draw the pickup
                sp.Background = pickupColour;

            } // if

        } // placePickup()

        private int generateCoord()
        {
            int coord = 0;

            // generate a random number
            coord = ran.Next(boardSize);

            return coord;

        } // generateCoord()

        private void movePlayerRight(string playerName)
        {
            if(playerName == PLAYER_ONE)    // if player one
            {

                if (player1.Moved == false)
                {
                    // move the player
                    player1.MoveRight();

                    // decrease move count
                    player1.Moved = true;

                } // if
            }
            else // if player two
            {
                if (player2.Moved == false)
                {
                    // move the player
                    player2.MoveRight();

                    // decrease move count
                    player2.Moved = true;

                } // if
            } // f

        } // movePlayerRight()

        private void movePlayerLeft(string playerName)
        {
            if (playerName == PLAYER_ONE)    // if player one
            {

                if (player1.Moved == false)
                {
                    // move the player
                    player1.MoveLeft();

                    // decrease move count
                    player1.Moved = true;

                } // if
            }
            else // if player two
            {
                if (player2.Moved == false)
                {
                    // move the player
                    player2.MoveLeft();

                    // decrease move count
                    player2.Moved = true;

                } // if
            } // f

        } // movePlayerLeft()

        private void leftBtn_Click(object sender, RoutedEventArgs e)
        {
            // move player 1 left
            movePlayerLeft(PLAYER_ONE);

        }

        private void rightBtn_Click(object sender, RoutedEventArgs e)
        {
            // move player 1 right
            movePlayerRight(PLAYER_ONE);

        }

        private async void Myo_PoseChanged(object sender, PoseEventArgs e)
        {
            Pose curr = e.Pose;

            string pName = "";

            pName = myoManager.getPlayerName(e.Myo.Handle.ToString());

            switch (curr)
            {
                case Pose.Rest:
                    
                    break;
                case Pose.Fist:
                                       
                    break;
                case Pose.WaveIn:

                    movePlayerLeft(pName);
                    break;
                case Pose.WaveOut:

                    movePlayerRight(pName);
               
                    break;
                case Pose.FingersSpread:
                    break;
                case Pose.DoubleTap:
                    break;
                case Pose.Unknown:
                    break;
                default:
                    break;
            }
           
        }

        // gets called when the window size changes
        private void Current_SizeChanged(object sender, Windows.UI.Core.WindowSizeChangedEventArgs e)
        {
            // update the width and height of the grid
            if (e.Size.Height == e.Size.Width)
            {
                // do nothing
                return;
            }
            else if (e.Size.Height  < e.Size.Width)
            {
                grid.Width = e.Size.Height;
                grid.Height = e.Size.Height - 126;
                
            }
            else
            {
                grid.Height = e.Size.Width - 126;
                grid.Width = e.Size.Width;
                
            } // if

            // Fix clipping issues by using the smaller number for width and height
            if(grid.Height > grid.Width)
            {
                grid.Height = grid.Width;
            }
            else
            {
                grid.Width = grid.Height;
            } // if
        } // Current_SizeChanged()

    }
}
