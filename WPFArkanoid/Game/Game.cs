using System;
using System.ComponentModel;
using System.Linq;
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

    /// <summary>
    /// GameView and the main GameLoop
    /// </summary>
    public class Game : INotifyPropertyChanged
    {
        private const int DEFAULT_SCORE = 0;
        private const int DEFAULT_LIVES = 3;
        private const int ARENA_RIGHT_OFFSET = 16; // Padding to compensate window and canvas size mismatch
        private const int COLISION_PADDING = 5; // Dirty fix for funky collision detection
        
        /// <summary>
        /// Level which get loaded.
        /// Level should always have 5 rows and 10 collumns
        /// The numbers represent point value and endurance of a brick.
        /// The numbers span from 0 to 5, 0 = empty space.
        /// </summary>
        private char[] LEVEL = { '5', '5', '5', '5', '1', '1', '5', '5', '5', '5',
                                 '4', '4', '4', '4', '1', '1', '4', '4', '4', '4',
                                 '3', '3', '3', '3', '1', '1', '3', '3', '3', '3',
                                 '2', '2', '2', '2', '1', '1', '2', '2', '2', '2',
                                 '1', '0', '0', '1', '1', '1', '1', '0', '0', '1',};

        private static DispatcherTimer timer;

        private IColidableObject[] brickList;
        private readonly Drawer Renderer;
        private readonly Ball ball;
        private readonly PlayerPaddle player;

        public event PropertyChangedEventHandler PropertyChanged = (sender, e) => { };
        public event EventHandler GameOverReached;
        public event EventHandler VictoryReached;

        public KeyPressed KeyPressed { get; set; }
        public WriteableBitmap RenderTarget { get; set; }
        public int NumberOfActiveBricks { get; set; }
        public int Score { get; set; }
        public int Lives { get; set; }
        public Size GameArea { get; set; }

        public Game(Size s) 
        {
            GameArea = s;

            Renderer = new Drawer(GameArea);
            RenderTarget = Renderer.Render;

            ball = new Ball(new Position((int)(RenderTarget.Width / 2), (int)(RenderTarget.Height / 2)));
            player = new PlayerPaddle();

            timer = new DispatcherTimer();
            timer.Tick += new EventHandler(GameLoop);
            timer.Interval = new TimeSpan(0, 0, 0, 0, 16);

            ResetTheGame();
            timer.Start();

        }

        /// <summary>
        /// One tick of the game loop
        /// </summary>
        /// <param name="sender"> Object sender</param>
        /// <param name="e">Event arguments</param>
        private void GameLoop(object sender, EventArgs e) 
        {
            if (ball.IsBoundToPaddle)
            {
                ball.Position.X = player.Position.X + player.Size.Width / 2 - ball.Size.Width / 2;
                ball.Position.Y = player.Position.Y - (ball.Size.Height + COLISION_PADDING);
            }
            else ball.Move();
            HandleKeyboardInput();
            CheckOutOfBounds();
            CheckPlayerOutOfBounds();
            ProcessBricks();
            PropertyChanged(this, new PropertyChangedEventArgs("RenderTarget"));

            CheckWinLoseConditions();
        }

        /// <summary>
        /// Function which renders all bricks and checks their collisions with ball
        /// </summary>
        private void ProcessBricks() 
        {
            CheckBallColisionWith(player);

            Renderer.Clear();
            Renderer.Draw(ball);
            Renderer.Draw(player);

            foreach (var item in brickList)
            {
                if (item.IsActive) 
                {
                    bool colided = CheckBallColisionWith(item);
                    Renderer.Draw(item);

                    if (colided && item.IsDestroyable) 
                    {
                        var brick = item as Brick;
                        Score += brick.PointValue;
                        NumberOfActiveBricks -= 1;
                        brick.Break();

                        PropertyChanged(this, new PropertyChangedEventArgs("Score"));
                    }
                }
            }
        }

        /// <summary>
        /// Bauncing ball of the canvas walls.
        /// </summary>
        private void CheckOutOfBounds() 
        {
            if (ball.Position.X <= 0) 
            { 
                ball.Position.X = 0; 
                ball.Bounce(BallColisionSide.LEFT); 
            }

            if (ball.Position.X + ball.Size.Width + ARENA_RIGHT_OFFSET >= GameArea.Width) 
            { 
                ball.Position.X = GameArea.Width - ball.Size.Width - ARENA_RIGHT_OFFSET; 
                ball.Bounce(BallColisionSide.RIGHT); 
            }

            if (ball.Position.Y <= 0) 
            { 
                ball.Position.Y = 0; 
                ball.Bounce(BallColisionSide.TOP); 
            }

            if (ball.Position.Y >= GameArea.Height) 
            { 
                ball.Position.Y = GameArea.Height; 
                ResetBallPostion(); 
            }
        }

        /// <summary>
        /// Not letting player to get out of the rendering canvas.
        /// </summary>
        private void CheckPlayerOutOfBounds() 
        {
            if (player.Position.X <= 0) 
            { 
                player.Position.X = 0; 
            }

            if (player.Position.X + player.Size.Width + ARENA_RIGHT_OFFSET >= GameArea.Width) 
            { 
                player.Position.X = GameArea.Width - player.Size.Width - ARENA_RIGHT_OFFSET;
            }

            if (player.Position.Y <= 0) 
            {
                player.Position.Y = 0; 
            }

            if (player.Position.Y >= GameArea.Height) 
            {
                player.Position.Y = GameArea.Height; 
            }
        }

        /// <summary>
        /// Checking ball colision with specified colidable object
        /// </summary>
        /// <param name="obj">Colidable object to check collisions against.</param>
        /// <returns></returns>
        private bool CheckBallColisionWith(IColidableObject obj) 
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
                ball.Bounce(ballBounceSide);
                BallColisionCorrection(ballBounceSide);
                return true;
            }

            return false;

        }

        /// <summary>
        /// Calculate whitch side of the object did ball bounce off.
        /// </summary>
        /// <param name="leftDist"> Distance between object left side and ball right side.</param>
        /// <param name="rightDist"> Distance between object right side and ball left side.</param>
        /// <param name="topDist"> Distance between object top side and ball bottom side.</param>
        /// <param name="botDist"> Distance between object bottom side and ball top side.</param>
        /// <returns></returns>
        private BallColisionSide DecideColisonSide(int leftDist, int rightDist, int topDist, int botDist) 
        {
            int[] nums = { leftDist, rightDist, topDist, botDist };
            if (nums.Min() == botDist) return BallColisionSide.TOP;
            else if (nums.Min() == rightDist) return BallColisionSide.LEFT;
            else if (nums.Min() == leftDist) return BallColisionSide.RIGHT;
            return BallColisionSide.BOTTOM;
        }

        /// <summary>
        /// Ball position correction to not get stuck inside of a brick.
        /// </summary>
        /// <param name="col"> Ball collision side</param>
        private void BallColisionCorrection(BallColisionSide col) 
        {
            switch (col)
            {
                case BallColisionSide.TOP:
                    ball.Position.Y += COLISION_PADDING;
                    break;
                case BallColisionSide.BOTTOM:
                    ball.Position.Y -= COLISION_PADDING;
                    break;
                case BallColisionSide.LEFT:
                    ball.Position.X += COLISION_PADDING;
                    break;
                case BallColisionSide.RIGHT:
                    ball.Position.X -= COLISION_PADDING;
                    break;
                case BallColisionSide.NONE:
                default:
                    break;
            }
        }

        /// <summary>
        /// Regenerate level and reset Score and HP.
        /// </summary>
        private void ResetTheGame() 
        {
            brickList = LevelGenerator.GenerateLevel(LEVEL);
            NumberOfActiveBricks = brickList.Length;

            Score = DEFAULT_SCORE;
            Lives = DEFAULT_LIVES;
        }

        /// <summary>
        /// Get distance betwwen 2 points in 1D space.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="x2"></param>
        /// <returns></returns>
        private int GetDistance(int x, int x2) 
        {
            return (int)Math.Abs(x - x2);
        }

        /// <summary>
        /// Reset ball postition and bind it to paddle.
        /// </summary>
        private void ResetBallPostion() 
        {
            ball.IsBoundToPaddle = true;
            Lives -= 1;
            PropertyChanged(this, new PropertyChangedEventArgs("Lives"));
        }

        /// <summary>
        /// Handeling input events from View.
        /// </summary>
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
                    ball.IsBoundToPaddle = false;
                    break;
                case KeyPressed.NONE:
                default:
                    break;
            }
        }

        /// <summary>
        /// Check Victory or Losing conditions and reset the game if aplicable.
        /// </summary>
        private void CheckWinLoseConditions() 
        {
            if (Lives == 0)
            {
                GameOverReached(this, null);
                ResetTheGame();
                PropertyChanged(this, new PropertyChangedEventArgs("Lives"));
                PropertyChanged(this, new PropertyChangedEventArgs("Score"));
            }
            else if (NumberOfActiveBricks == 0) 
            {
                VictoryReached(this, null);
                ResetTheGame();
                PropertyChanged(this, new PropertyChangedEventArgs("Lives"));
                PropertyChanged(this, new PropertyChangedEventArgs("Score"));
            }
        }

        protected virtual void OnGameOverReached(EventArgs e) 
        {
            EventHandler handler = GameOverReached;
            handler?.Invoke(this, e);
        }

        protected virtual void OnVictoryReached(EventArgs e)
        {
            EventHandler handler = VictoryReached;
            handler?.Invoke(this, e);
        }

    }
}
