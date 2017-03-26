using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyoSnake
{
    class Pickup
    {

        public int PosX { get; set; }
        public int PosY { get; set; }
        public string PlayerName { get; set; }

        public Pickup(string playerName)
        {
            this.PlayerName = playerName;
        }

    } // class
} //namespace
