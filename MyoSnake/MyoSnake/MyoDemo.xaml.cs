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

using MyoUWP.Classes;

using MyoSharp.Communication;
using MyoSharp.Device;
using MyoSharp.Exceptions;
using MyoSharp.Poses;
using Windows.UI;





// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace MyoUWP
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MyoDemo : Page
    {
        /*
            1. Add the myosharp.dll from bin folder in MyoSharp to the bin folder.
            2. add a reference to this in the project references
            3. add two folders - x86 x64 to your project
            4. add "existing" dll to these two folders
                find these in MyoSharp git repo.
                change the "Content" to "Copy if newer" in properties for these
                

            5.  add the using commands to mainpage.xaml.cs
                            using MyoSharp.Communication;
                            using MyoSharp.Device;
                            using MyoSharp.Exceptions;
                            using MyoSharp.Poses;
            6.  code your responses



         */
        IChannel _myoChannel;
        IChannel _myoChannel1;
        IHub _myoHub;
        IHub _myoHub1;

        Pose _currentPose;
        double _currentRoll;

        List<string> playerId = new List<string>();
        Dictionary<string, string> playerName = new Dictionary<string, string>();

        DispatcherTimer _orientationTimer;

        public MyoDemo()
        {
            this.InitializeComponent();
            setupTimers();
            _orientationTimer.Start();

        }

        #region timers methods
        private void setupTimers()
        {
            if( _orientationTimer == null)
            {
                _orientationTimer = new DispatcherTimer();
                _orientationTimer.Interval = TimeSpan.FromMilliseconds(200);
                _orientationTimer.Tick += _orientationTimer_Tick;
            }
        }

        private void _orientationTimer_Tick(object sender, object e)
        {
            if (_currentRoll >= 0)
            {   // move to the left
                //eMyo.SetValue(Canvas.LeftProperty, (double)eMyo.GetValue(Canvas.LeftProperty) - 1);
            }
            else
            {   // movet to the right
               // eMyo.SetValue(Canvas.LeftProperty, (double)eMyo.GetValue(Canvas.LeftProperty) + 1);
            }

        }
        #endregion

        #region Myo Setup Methods
        private void btnMyo_Click(object sender, RoutedEventArgs e)
        { // communication, device, exceptions, poses
            // create the channel
            _myoChannel = Channel.Create(ChannelDriver.Create(ChannelBridge.Create(),
                                    MyoErrorHandlerDriver.Create(MyoErrorHandlerBridge.Create())));

            // create the hub with the channel
            _myoHub = MyoSharp.Device.Hub.Create(_myoChannel);
            // create the event handlers for connect and disconnect
            _myoHub.MyoConnected += _myoHub_MyoConnected;
            _myoHub.MyoDisconnected += _myoHub_MyoDisconnected;

            // start listening 
            _myoChannel.StartListening();


            // create the channel
            _myoChannel1 = Channel.Create(ChannelDriver.Create(ChannelBridge.Create(),
                                    MyoErrorHandlerDriver.Create(MyoErrorHandlerBridge.Create())));

            /*// create the hub with the channel
            _myoHub1 = MyoSharp.Device.Hub.Create(_myoChannel1);
            // create the event handlers for connect and disconnect
            _myoHub1.MyoConnected += _myoHub_MyoConnected;
            _myoHub1.MyoDisconnected += _myoHub_MyoDisconnected;

            // start listening 
            _myoChannel1.StartListening();*/

        }

        private async void _myoHub_MyoDisconnected(object sender, MyoEventArgs e)
        {
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                //tblUpdates.Text = tblUpdates.Text + System.Environment.NewLine +"Myo disconnected";
            });
            _myoHub.MyoConnected -= _myoHub_MyoConnected;
            _myoHub.MyoDisconnected -= _myoHub_MyoDisconnected;
            _orientationTimer.Stop();
        }

        private async void _myoHub_MyoConnected(object sender, MyoEventArgs e)
        {
            e.Myo.Vibrate(VibrationType.Long);
            playerId.Add(e.Myo.Handle.ToString());
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                //tblUpdates.Text +=  " - Myo Connected: " + e.Myo.Handle.ToString();
                //tblUpdates.Text = playerId.Count.ToString();

            });

            if (playerId.Count==2) {
                for (int i = 0; i < 2; i++)
                {
                    playerName.Add(playerId[i],"Player"+(i+1));
                }

            }
            // add the pose changed event here
            e.Myo.PoseChanged += Myo_PoseChanged;
            e.Myo.OrientationDataAcquired += Myo_OrientationDataAcquired;
            e.Myo.GyroscopeDataAcquired += Myo_GyroscopeDataAcquired;
            
           
            // unlock the Myo so that it doesn't keep locking between our poses
            e.Myo.Unlock(UnlockType.Hold);

            try
            {
                var sequence = PoseSequence.Create(e.Myo, Pose.FingersSpread, Pose.WaveIn);
                sequence.PoseSequenceCompleted += Sequence_PoseSequenceCompleted;

            }
            catch (Exception myoErr)
            {
                string strMsg = myoErr.Message;
            }

        }
        #endregion

        #region Gryoscope data
        private async void Myo_GyroscopeDataAcquired(object sender, GyroscopeDataEventArgs e)
        {
            
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                showGryoscopeData(e.Gyroscope.X, e.Gyroscope.Y, e.Gyroscope.Z);
            });

        }

        private void showGryoscopeData(float x, float y, float z)
        {
            var pitchDegree = (x * 180.0) / System.Math.PI;
            var yawDegree = (y * 180.0) / System.Math.PI;
            var rollDegree = (z * 180.0) / System.Math.PI;

            //tblXGyro.Text = "Gyro X: " + (pitchDegree).ToString("0.00");
           // tblYGyro.Text = "Gyro Y: " + (yawDegree).ToString("0.00");
           // tblZGyro.Text = "Gyro R: " + (rollDegree).ToString("0.00");

        }
        #endregion

        #region Accelerometer Orientation Data
        private async void Myo_OrientationDataAcquired(object sender, OrientationDataEventArgs e)
        {
            _currentRoll = e.Roll;
            
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                showOrientationData(e.Pitch, e.Yaw, e.Roll);
            });
        }

        private void showOrientationData(double pitch, double yaw, double roll)
        {

            var pitchDegree = (pitch * 180.0) / System.Math.PI;
            var yawDegree = (yaw * 180.0) / System.Math.PI;
            var rollDegree = (roll * 180.0) / System.Math.PI;

            //tblXValue.Text = "Pitch: " + (pitchDegree).ToString("0.00");
            //tblYValue.Text = "Yaw: " + (yawDegree).ToString("0.00");
           // tblZValue.Text = "Roll: " + (rollDegree).ToString("0.00");

            //pitchLine.X2 = pitchLine.X1 + pitchDegree;
            //yawLine.Y2 = yawLine.Y1 - yawDegree;
            //rollLine.X2 = rollLine.X1 - rollDegree;
            //rollLine.Y2 = rollLine.Y1 + rollDegree;
        }
        #endregion

        #region Pose related methods

        private async void Sequence_PoseSequenceCompleted(object sender, PoseSequenceEventArgs e)
        {
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                //tblUpdates.Text = "Pose Sequence completed";
            });
        }

        private async void Pose_Triggered(object sender, PoseEventArgs e)
        {
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
               // tblUpdates.Text = "Pose Held: " + e.Pose.ToString();
            });

        }


        private async void Myo_PoseChanged(object sender, PoseEventArgs e)
        {
            Pose curr = e.Pose;
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                string pName = "";

                if (curr.ToString()!="Rest")
                {
                    playerName.TryGetValue(e.Myo.Handle.ToString(), out pName);

                    if (pName == "Player1")
                    {
                        tblposeP1.Text = curr.ToString();
                    }
                    else {
                        tblposeP2.Text = curr.ToString();
                    }




                    //playerName.TryGetValue(e.Myo.Handle.ToString(), out str);

                   
                }
                
                switch (curr)
                {
                    case Pose.Rest:
                       // eMyo.Fill = new SolidColorBrush(Colors.Blue);
                        break;
                    case Pose.Fist:
//eMyo.Fill = new SolidColorBrush(Colors.Red);                    
                        break;
                    case Pose.WaveIn:
                        break;
                    case Pose.WaveOut:
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
            });
        }
        #endregion

        private void startTimer()
        {
            

        }
    }
}
