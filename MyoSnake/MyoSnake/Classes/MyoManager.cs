﻿using MyoSharp.Communication;
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

        public bool UseMyo = true;
        public bool isConnected = true;

        IChannel _myoChannel;
        IHub _myoHub;
       // IHub _myoHub1;

        //Pose _currentPose;
        //double _currentRoll;

        List<string> playerId = new List<string>();
        Dictionary<string, string> playerName = new Dictionary<string, string>();
        public List<MyoEventArgs> MyoEventArgsList = new List<MyoEventArgs>();

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

            IChannel _myoChannel=null;
            IHub _myoHub=null;
            try
            {
                playerName.Clear();
                playerId.Clear();
                MyoEventArgsList.Clear();

                // create the channel
                _myoChannel = Channel.Create(ChannelDriver.Create(ChannelBridge.Create(), MyoErrorHandlerDriver.Create(MyoErrorHandlerBridge.Create())));

                // create the hub with the channel
                _myoHub = MyoSharp.Device.Hub.Create(_myoChannel);
            
                // create the event handlers for connect and disconnect
                _myoHub.MyoConnected += _myoHub_MyoConnected;
                _myoHub.MyoDisconnected += _myoHub_MyoDisconnected;

                // start listening 
                _myoChannel.StartListening();

            }
            catch (System.InvalidOperationException e)
            {
                Debug.WriteLine(e.StackTrace);

                return;
            }

        }

        private async void _myoHub_MyoDisconnected(object sender, MyoEventArgs e)
        {
            //_myoHub.MyoConnected -= _myoHub_MyoConnected;
            //_myoHub.MyoDisconnected -= _myoHub_MyoDisconnected;
 
            playerName.Remove(e.Myo.Handle.ToString());

        }

        private async void _myoHub_MyoConnected(object sender, MyoEventArgs e)
        {
            e.Myo.Vibrate(VibrationType.Long);
            playerId.Add(e.Myo.Handle.ToString());

            if (playerId.Count == 1)
            {
                for (int i = 0; i < 1; i++)
                {
                    playerName.Add(playerId[i], "Player" + (i + 1));
                }

            }

            if (playerId.Count == 2)
            {
                for (int i = 1; i < 2; i++)
                {
                    playerName.Add(playerId[i], "Player" + (i + 1));
                }

            }

            //save args here
            MyoEventArgsList.Add(e);

            // unlock the Myo so that it doesn't keep locking between our poses
            e.Myo.Unlock(UnlockType.Hold);
        }
        #endregion

        public string getPlayerName(string handle) {

            string pName = "";
            playerName.TryGetValue(handle, out pName);
            return pName;
        }

        public void Vibrate(string name)
        {      
            foreach (var myo in MyoEventArgsList)
            {

                string pName = "";
                playerName.TryGetValue(myo.Myo.Handle.ToString(), out pName);

                if (pName==name) {
                    myo.Myo.Vibrate(VibrationType.Medium);
                }
            }
    
        }



    } // class
} // namespace
