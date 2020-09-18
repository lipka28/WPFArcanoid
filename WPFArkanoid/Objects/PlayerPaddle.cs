using System;
using System.Collections.Generic;
using System.DirectoryServices.ActiveDirectory;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace WPFArkanoid
{
    public class PlayerPaddle : IColidableObject
    {
        public const int PADDLE_WIDTH = 80;
        public const int PADDLE_HEIGHT = 20;

        public const int INIT_X_POS = 400 - PADDLE_WIDTH / 2;
        public const int INIT_Y_POS = 550;

        public const int START_SPEED = 10;

        public PlayerPaddle() 
        {
            Position = new Position(INIT_X_POS, INIT_Y_POS);
            Size = new Size(PADDLE_WIDTH, PADDLE_HEIGHT);
            Speed = new Speed(START_SPEED, 0);

            IsColidable = true;
            IsDestroyable = false;
            IsActive = true;

            Color = Colors.WHITE;
            Shape = Shape.RECT;
        
        }

        public void MoveLeft() 
        {
            Position.X -= Speed.XSpeed;
        }

        public void MoveRight()
        {
            Position.X += Speed.XSpeed;
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
    }
}
