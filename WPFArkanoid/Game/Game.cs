using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Timers;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace WPFArkanoid
{
    public enum KeyPressed
    {
        NONE,
        LEFT,
        RIGHT,
        SPACE
    }
    public class Game : INotifyPropertyChanged
    {
        private const int ARENA_RIGHT_OFFSET = 16;
        private char[] LEVEL = { '5', '5', '5', '5', '1', '1', '5', '5', '5', '5',
                                 '4', '4', '4', '4', '1', '1', '4', '4', '4', '4',
                                 '3', '3', '3', '3', '1', '1', '3', '3', '3', '3',
                                 '2', '2', '2', '2', '1', '1', '2', '2', '2', '2',
                                 '1', '0', '0', '1', '1', '1', '1', '0', '0', '1',};

        private static DispatcherTimer timer;
        private Drawer Renderer;

        private IColidableObject[] objectList;
        private Ball ball;
        private PlayerPaddle player;

        public event PropertyChangedEventHandler PropertyChanged = (sender, e) => { };

        public Game(Size s) 
        {
            GameArea = s;

            Renderer = new Drawer(GameArea);
            objectList = LevelGenerator.GenerateLevel(LEVEL);

            Score = 0;
            Lives = 5;

            RenderTarget = Renderer.Render;
            ball = new Ball(new Position((int)(RenderTarget.Width / 2), (int)(RenderTarget.Height / 2)));
            player = new PlayerPaddle();

            timer = new DispatcherTimer();
            timer.Tick += new EventHandler(gameLoop);
            timer.Interval = new TimeSpan(0, 0, 0, 0, 16);
            timer.Start();

        }
        public WriteableBitmap RenderTarget { get; set; }
        public int Score { get; set; }
        public int Lives { get; set; }
        public Size GameArea { get; set; }

        private void gameLoop(object sender, EventArgs e) 
        {
            ball.move();
            HandleKeyboardInput();
            checkOutOfBounds();
            checkPlayerOutOfBounds();
            processObjectList();
            PropertyChanged(this, new PropertyChangedEventArgs("RenderTarget"));
        }

        private void processObjectList() 
        {
            CheckBallColisionWith(player);

            Renderer.Clear();
            Renderer.Draw(ball);
            Renderer.Draw(player);

            foreach (var item in objectList)
            {
                if (item.IsActive) 
                {
                    CheckBallColisionWith(item);
                    Renderer.Draw(item);
                }
            }
        }

        private void checkOutOfBounds() 
        {
            if (ball.Position.X <= 0) { ball.Position.X = 0; ball.bounce(BallColisionSide.LEFT); }
            if (ball.Position.X + ball.Size.Width + ARENA_RIGHT_OFFSET >= GameArea.Width) { ball.Position.X = GameArea.Width - ball.Size.Width - ARENA_RIGHT_OFFSET; ball.bounce(BallColisionSide.RIGHT); }
            if (ball.Position.Y <= 0) { ball.Position.Y = 0; ball.bounce(BallColisionSide.TOP); }
            if (ball.Position.Y >= GameArea.Height) { ball.Position.Y = GameArea.Height; resetBallPostion(); }
        }
        private void checkPlayerOutOfBounds() 
        {
            if (player.Position.X <= 0) { player.Position.X = 0; }
            if (player.Position.X + player.Size.Width + ARENA_RIGHT_OFFSET >= GameArea.Width) { player.Position.X = GameArea.Width - player.Size.Width - ARENA_RIGHT_OFFSET; }
            if (player.Position.Y <= 0) { player.Position.Y = 0; }
            if (player.Position.Y >= GameArea.Height) { player.Position.Y = GameArea.Height; }
        }

        private void CheckBallColisionWith(IColidableObject obj) 
        {
            int ballBotSide = ball.Position.Y + ball.Size.Height;
            int ballTopSide = ball.Position.Y;
            int ballLeftSide = ball.Position.X;
            int ballRightSide = ball.Position.X + ball.Size.Width;

            int objectBotSide = obj.Position.Y + obj.Size.Height; ;
            int objectTopSide = obj.Position.Y;
            int objectLeftSide = obj.Position.X;
            int objectRightSide = obj.Position.X + obj.Size.Width;

            int leftSideDist = GetDistance(objectLeftSide, ballRightSide);
            int rightSideDist = GetDistance(objectRightSide, ballLeftSide);
            int topSideDist = GetDistance(objectTopSide, ballBotSide);
            int botSideDist = GetDistance(objectBotSide, ballTopSide);

            if (ballBotSide >= objectTopSide && ballTopSide <= objectBotSide &&
                ballRightSide >= objectLeftSide && ballLeftSide <= objectRightSide)
            {
                var ballBounceSide = DecideColisonSide(leftSideDist, rightSideDist, topSideDist, botSideDist);
                ball.bounce(ballBounceSide);

            }

        }

        private BallColisionSide DecideColisonSide(int leftDist, int rightDist, int topDist, int botDist) 
        {
            int[] nums = { leftDist, rightDist, topDist, botDist };
            if (nums.Min() == botDist) return BallColisionSide.TOP;
            else if (nums.Min() == rightDist) return BallColisionSide.LEFT;
            else if (nums.Min() == leftDist) return BallColisionSide.RIGHT;
            return BallColisionSide.BOTTOM;
        }

        private int GetDistance(int x, int y) 
        {
            return (int)Math.Abs(x - y);
        }

        private void resetBallPostion() 
        {
            ball.Position.X = (int)(RenderTarget.Width / 2);
            ball.Position.Y = (int)(RenderTarget.Height / 2);
            Lives -= 1;
            PropertyChanged(this, new PropertyChangedEventArgs("Lives"));

        }

        private void HandleKeyboardInput() 
        {
            switch (KeyPressed)
            {
                case KeyPressed.LEFT:
                    player.MoveLeft();
                    break;
                case KeyPressed.RIGHT:
                    player.MoveRight();
                    break;
                case KeyPressed.SPACE:
                    break;
                case KeyPressed.NONE:
                default:
                    break;
            }
        }

        public KeyPressed KeyPressed { get; set; }

    }
}
