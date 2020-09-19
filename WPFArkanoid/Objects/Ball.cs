using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO.Pipes;
using System.Text;

namespace WPFArkanoid
{
    public class Ball : IColidableObject
    {
        const int BALL_SIZE = 5;
        public Ball(Position pos) 
        {
            Position = pos;
            Size = new Size(BALL_SIZE, BALL_SIZE);
            Speed = getInitialBallSpeed();

            IsColidable = true;
            IsDestroyable = false;
            IsActive = true;

            Color = Colors.WHITE;
            Shape = Shape.ELLIPSE;
        }

        public void move() 
        {
            Position.X += Speed.XSpeed;
            Position.Y += Speed.YSpeed;
        }

        public bool bounce(BallColisionSide col) 
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
        public Speed Speed { get; set; }

        public Position Position { get; set; }
        public Size Size { get; set; }
        public int PointValue { get; set; }

        public Colors Color { get; set; }
        public Shape Shape { get; set; }

        private Speed getInitialBallSpeed() 
        {
            int speed = 3;
            int[] choice = { -speed, speed };
            int xSpeed = choice[new Random().Next(choice.Length)];
            int ySpeed = -speed;

            return new Speed(xSpeed, ySpeed);
        }

    }
}
