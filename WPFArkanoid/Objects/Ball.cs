﻿using System;

namespace WPFArkanoid
{
    /// <summary>
    /// Ball implementation
    /// </summary>
    public class Ball : IColidableObject
    {
        private const int BALL_SIZE = 10;
        private const int BALL_SPEED = 5;
        public Ball(Position pos) 
        {
            Position = pos;
            Size = new Size(BALL_SIZE, BALL_SIZE);
            Speed = GetInitialBallSpeed();

            IsColidable = true;
            IsDestroyable = false;
            IsActive = true;
            IsBoundToPaddle = true;

            Color = Colors.WHITE;
            Shape = Shape.ELLIPSE;
        }

        public void Move() 
        {
            Position.X += Speed.XSpeed;
            Position.Y += Speed.YSpeed;
        }

        public bool Bounce(BallColisionSide col) 
        {
            switch (col)
            {  
                case BallColisionSide.TOP:
                case BallColisionSide.BOTTOM:
                    Speed.YSpeed *= -1;
                    return true;
                case BallColisionSide.LEFT:
                case BallColisionSide.RIGHT:
                    Speed.XSpeed *= -1;
                    return true;
                case BallColisionSide.NONE:
                default:
                    return false;
            }
        }

        public bool IsColidable { get; set; }
        public bool IsDestroyable { get; set; }
        public bool IsActive { get; set; }
        public bool IsBoundToPaddle { get; set; }

        public Speed Speed { get; set; }
        public Position Position { get; set; }
        public Size Size { get; set; }
        public int PointValue { get; set; }

        public Colors Color { get; set; }
        public Shape Shape { get; set; }

        private Speed GetInitialBallSpeed() 
        {
            int speed = BALL_SPEED;
            int[] choice = { -speed, speed };
            int xSpeed = choice[new Random().Next(choice.Length)];
            int ySpeed = -speed;

            return new Speed(xSpeed, ySpeed);
        }

    }
}
