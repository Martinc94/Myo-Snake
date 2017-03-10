﻿using System;
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

        public SnakeBody Head { get; set; }
        public SnakeBody Tail { get; set; }

        public int StartSize { get; set; }
        public int StartPosX { get; set; }
        public int StartPosY { get; set; }
        private int boardSize;

        public Snake(int boardSize)
        {

            // save the board size
            this.boardSize = boardSize;

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

                        bodyPart.PosY = lastPart.PosY + 1;
                        bodyPart.PosX = lastPart.PosX;

                        break;
                    case "W":

                        bodyPart.PosY = lastPart.PosY;
                        bodyPart.PosX = lastPart.PosX + 1;

                        break;
                    case "E":

                        bodyPart.PosY = lastPart.PosY;
                        bodyPart.PosX = lastPart.PosX - 1;

                        break;
                    case "S":
                        bodyPart.PosY = lastPart.PosY - 1;
                        bodyPart.PosX = lastPart.PosX;
                        break;

                } // switch

            }
            else // if there are no body parts (first part)
            {
                // set starting values
                bodyPart.PosX = StartPosX;
                bodyPart.PosY = StartPosY;
                bodyPart.CurrentDirection = "N";

                // set head
                Head = bodyPart;

            } // if

            // add body part to the list
            Body.Add(bodyPart);

            // Flag as tail
            Tail = bodyPart;

        } // IncreaseBodySize()

        // moves the snake
        public void Move()
        {
           // SnakeBody last = Head;

            for (int i = Body.Count -1; i >= 0; i--)
            {

                // set the body part position
                switch (Body[i].CurrentDirection)
                {
                    case "N":

                        Body[i].PosY -= 1;
                        
                        break;
                    case "W":

                        Body[i].PosX -= 1;
                        
                        break;
                    case "E":

                        Body[i].PosX += 1;
                        
                        break;
                    case "S":
                        Body[i].PosY += 1;

                        break;

                } // switch

                // account for walls, move snake to other size of the board
                if (Body[i].PosX >= boardSize)
                    Body[i].PosX = 0;
                else if (Body[i].PosX < 0)
                    Body[i].PosX = boardSize - 1;

                if (Body[i].PosY >= boardSize)
                    Body[i].PosY = 0;
                else if (Body[i].PosY < 0)
                    Body[i].PosY = boardSize - 1;

                if (i > 0)
                {
                    // tell body part where to move next, by getting the part ahead of its direction
                    Body[i].CurrentDirection = Body[i - 1].CurrentDirection;

                } // if
            } // for

        } // Move()

        // updates the direction of the snakes head, to make it move left of its current position
        public void MoveLeft()
        {
            switch (Head.CurrentDirection)
            {
                case "N":

                    Head.CurrentDirection = "W";

                    break;
                case "W":

                    Head.CurrentDirection = "S";

                    break;
                case "E":

                    Head.CurrentDirection = "N";

                    break;
                case "S":
                    Head.CurrentDirection = "E";

                    break;

            } // switch

        } // MoveLeft()

        // updates the direction of the snakes head, to make it move right of its current position
        public void MoveRight()
        {
            switch (Head.CurrentDirection)
            {
                case "N":

                    Head.CurrentDirection = "E";

                    break;
                case "W":

                    Head.CurrentDirection = "N";

                    break;
                case "E":

                    Head.CurrentDirection = "S";

                    break;
                case "S":
                    Head.CurrentDirection = "W";

                    break;

            } // switch

        } // MoveRight()

    } // class
} // namespace
