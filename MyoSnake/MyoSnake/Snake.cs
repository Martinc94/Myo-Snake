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

            // set start size
            StartSize = 4;

            // set start PosX and PosY
            StartPosX = 10;
            StartPosY = 10;

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
            SnakeBody bodyPart = new SnakeBody();

            // if there are body parts
            if (Body.Count > 0)
            {
                SnakeBody lastPart = Body[Body.Count - 1];

                // set the body part direction
                bodyPart.CurrentDirection = lastPart.CurrentDirection;

                // set the body part position
                switch (lastPart.CurrentDirection)
                {
                    case "N":

                        bodyPart.PosX = lastPart.PosX - 1;
                        bodyPart.PosY = lastPart.PosY;

                        break;
                    case "W":

                        bodyPart.PosX = lastPart.PosX;
                        bodyPart.PosY = lastPart.PosY + 1;

                        break;
                    case "E":

                        bodyPart.PosX = lastPart.PosX;
                        bodyPart.PosY = lastPart.PosY - 1;

                        break;
                    case "S":
                        bodyPart.PosX = lastPart.PosX + 1;
                        bodyPart.PosY = lastPart.PosY;
                        break;

                } // switch
            }
            else // if there are no body parts (first part)
            {
                // set starting values
                bodyPart.PosX = StartPosX;
                bodyPart.PosY = StartPosY;
                bodyPart.CurrentDirection = "N";

            } // if

            // add body part to the list
            Body.Add(bodyPart);

        } // IncreaseBodySize()

    } // class
} // namespace
