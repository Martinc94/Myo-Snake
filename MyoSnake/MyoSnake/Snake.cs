using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyoSnake
{
    class Snake
    {
        public string PlayerName { get; set; }
        public List<SnakeBody> Body { get; set; }

        public int StartSize { get; set; }
        public int StartPosX { get; set; }
        public int StartPosY { get; set; }
        

        public Snake()
        {
            // initialise body list
            Body = new List<SnakeBody>();

            // add body parts
            for (int i = 0; i < StartSize; i++)
            {
                IncreaseBodySize();
            } // for

        } // constructor

        // increases the size of the body by one
        public void IncreaseBodySize()
        {
            // create body part
            SnakeBody body = new SnakeBody();

            // if there are body parts
            if (Body.Count > 0)
            {
                SnakeBody lastPart = Body[Body.Count - 1];

                // set the body part direction
                body.CurrentDirection = lastPart.CurrentDirection;

                // set the body part position
                switch (lastPart.CurrentDirection)
                {
                    case "N":

                        body.PosX = lastPart.PosX - 1;
                        body.PosY = lastPart.PosY;

                        break;
                    case "W":

                        body.PosX = lastPart.PosX;
                        body.PosY = lastPart.PosY + 1;

                        break;
                    case "E":

                        body.PosX = lastPart.PosX;
                        body.PosY = lastPart.PosY - 1;

                        break;
                    case "S":
                        body.PosX = lastPart.PosX + 1;
                        body.PosY = lastPart.PosY;
                        break;

                } // switch
            }
            else // if there are no body parts (first part)
            {
                // set starting values
                body.PosX = StartPosX;
                body.PosY = StartPosY;
                body.CurrentDirection = "N";

            } // if

        } // IncreaseBodySize()

    } // class
} // namespace
