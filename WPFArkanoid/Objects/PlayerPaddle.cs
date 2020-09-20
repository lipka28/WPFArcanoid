
namespace WPFArkanoid
{
    /// <summary>
    /// Class implementing player controlled paddle 
    /// </summary>
    public class PlayerPaddle : IColidableObject
    {
        private const int PADDLE_WIDTH = 80;
        private const int PADDLE_HEIGHT = 20;

        private const int INIT_X_POS = 400 - PADDLE_WIDTH / 2;
        private const int INIT_Y_POS = 550;

        private const int START_SPEED = 10;

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
        public Position CenteredPostion { get; set; }
        public Size Size { get; set; }
        public int PointValue { get; set; }

        public Colors Color { get; set; }
        public Shape Shape { get; set; }
    }
}
