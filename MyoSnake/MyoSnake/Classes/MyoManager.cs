using MyoSharp.Communication;
using MyoSharp.Device;
using MyoSharp.Exceptions;
using MyoSharp.Poses;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace MyoSnake.Classes
{
    // A singleton class for managing the myo connections
    class MyoManager
    {
        private static MyoManager myoManager = new MyoManager();

        IChannel _myoChannel;
        IChannel _myoChannel1;
        IHub _myoHub;
        IHub _myoHub1;

        Pose _currentPose;
        double _currentRoll;

        List<string> playerId = new List<string>();
        Dictionary<string, string> playerName = new Dictionary<string, string>();

        public PoseEventArgs _currentPoseP1;
        public PoseEventArgs _currentPoseP2;

        // private constructor to make object a singleton
        private MyoManager()
        {

        }

        // returns the instance of the singleton object
        public static MyoManager getInstance()
        {
            return myoManager;
        } // getInstance()

        #region Myo Setup Methods
        public void connect()
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
            //await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            //{
            //    //tblUpdates.Text = tblUpdates.Text + System.Environment.NewLine +"Myo disconnected";
            //});
            _myoHub.MyoConnected -= _myoHub_MyoConnected;
            _myoHub.MyoDisconnected -= _myoHub_MyoDisconnected;
        }

        private async void _myoHub_MyoConnected(object sender, MyoEventArgs e)
        {
            e.Myo.Vibrate(VibrationType.Long);
            playerId.Add(e.Myo.Handle.ToString());

            

            if (playerId.Count == 2)
            {
                for (int i = 0; i < 2; i++)
                {
                    playerName.Add(playerId[i], "Player" + (i + 1));
                }

            }
            // add the pose changed event here
            e.Myo.PoseChanged += Myo_PoseChanged;
            

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


        #region Pose related methods

        private async void Sequence_PoseSequenceCompleted(object sender, PoseSequenceEventArgs e)
        {
            //await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            //{
            //    //tblUpdates.Text = "Pose Sequence completed";
            //});
        }

        private async void Pose_Triggered(object sender, PoseEventArgs e)
        {
            //await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            //{
            //    // tblUpdates.Text = "Pose Held: " + e.Pose.ToString();
            //});

        }

        private async void Myo_PoseChanged(object sender, PoseEventArgs e)
        {
            Pose curr = e.Pose;
            
            
                string pName = "";

                if (curr.ToString() != "Rest")
                {
                    playerName.TryGetValue(e.Myo.Handle.ToString(), out pName);

                    if (pName == "Player1")
                    {
                        //tblposeP1.Text = curr.ToString();
                    }
                    else
                    {
                        //tblposeP2.Text = curr.ToString();
                    }

                    //playerName.TryGetValue(e.Myo.Handle.ToString(), out str);
                }

            playerName.TryGetValue(e.Myo.Handle.ToString(), out pName);

            if (pName == "Player1")
            {
                PoseEventArgs p1Args = new PoseEventArgs(e.Myo, new DateTime(),e.Pose);
                
                _currentPoseP1 = p1Args;
            }
            else
            {
                //_currentPoseP2 = curr;
                PoseEventArgs p1Args = new PoseEventArgs(e.Myo, new DateTime(), e.Pose);

                _currentPoseP1 = p1Args;
            }

            //Debug.WriteLine("Pose Changed to "+e.Pose.ToString());

            /*switch (curr)
                {
                    case Pose.Rest:
                        // eMyo.Fill = new SolidColorBrush(Colors.Blue);
                        break;
                    case Pose.Fist:
                    //eMyo.Fill = new SolidColorBrush(Colors.Red);
                        Debug.WriteLine("Fist");                
                        break;
                    case Pose.WaveIn:
                        Debug.WriteLine("Wave right");
                        break;
                    case Pose.WaveOut:
                        Debug.WriteLine("Wave Left");
                        break;
                    case Pose.FingersSpread:
                        Debug.WriteLine("Fingers Spread");
                        break;
                    case Pose.DoubleTap:
                        Debug.WriteLine("Double Tap");
                        break; 
                    case Pose.Unknown:
                        break;
                    default:
                        break;
                }
            //});*/
        }
        #endregion


    } // class
} // namespace
