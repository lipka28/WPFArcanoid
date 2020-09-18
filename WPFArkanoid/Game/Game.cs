using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Text;
using System.Timers;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace WPFArkanoid
{
    public class Game : INotifyPropertyChanged
    {
        private const int DPI = 96;
        private char[] LEVEL = { '5', '5', '5', '5', '5', '5', '5', '5', '5', '5',
                                 '4', '4', '4', '4', '4', '4', '4', '4', '4', '4',
                                 '3', '3', '3', '3', '3', '3', '3', '3', '3', '3',
                                 '2', '2', '2', '2', '2', '2', '2', '2', '2', '2',
                                 '1', '0', '0', '1', '1', '1', '1', '0', '0', '1',};

        private static DispatcherTimer timer;
        private Drawer Renderer;

        private IColidableObject[] objectList;
        private Ball ball;

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
            checkOutOfBounds();
            processObjectList();
            PropertyChanged(this, new PropertyChangedEventArgs("RenderTarget"));
        }

        private void processObjectList() 
        {
            Renderer.Clear();
            Renderer.Draw(ball);
            foreach (var item in objectList)
            {
                if (item.IsActive) 
                {
                    var temp = item.Color;
                    Renderer.Draw(item); 
                }
            }
        }

        private void checkOutOfBounds() 
        {
            if (ball.Position.X <= 0) { ball.Position.X = 0; ball.bounce(BallColisionSide.LEFT); }
            if (ball.Position.X + ball.Size.Width*4 >= GameArea.Width) { ball.Position.X = GameArea.Width - ball.Size.Width*4; ball.bounce(BallColisionSide.RIGHT); }
            if (ball.Position.Y <= 0) { ball.Position.Y = 0; ball.bounce(BallColisionSide.TOP); }
            if (ball.Position.Y >= GameArea.Height) { ball.Position.Y = GameArea.Height; resetBallPostion(); }
        }

        private void resetBallPostion() 
        {
            ball.Position.X = (int)(RenderTarget.Width / 2);
            ball.Position.Y = (int)(RenderTarget.Height / 2);
            Lives -= 1;
            PropertyChanged(this, new PropertyChangedEventArgs("Lives"));

        }

    }
}
