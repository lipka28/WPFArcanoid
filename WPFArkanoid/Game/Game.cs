﻿using System;
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
        private Drawer painter;

        private List<IColidableObject> objectList;
        private Ball ball;

        public event PropertyChangedEventHandler PropertyChanged = (sender, e) => { };

        public Game(Size s) 
        {
            GameArea = s;
            painter = new Drawer(GameArea);
            objectList = new List<IColidableObject>();

            Score = 0;
            Lives = 5;

            RenderTarget = new RenderTargetBitmap((int)GameArea.Width, (int)GameArea.Height, DPI, DPI, PixelFormats.Pbgra32);
            RenderTarget.Render(painter.Drawing);
            ball = new Ball(new Position((int)(RenderTarget.Width / 2), (int)(RenderTarget.Height / 2)));

            timer = new DispatcherTimer();
            timer.Tick += new EventHandler(gameLoop);
            timer.Interval = new TimeSpan(0, 0, 0, 0, 16);
            timer.Start();

        }
        public RenderTargetBitmap RenderTarget { get; set; }
        public int Score { get; set; }
        public int Lives { get; set; }
        public Size GameArea { get; set; }

        private void gameLoop(object sender, EventArgs e) 
        {
            //RenderTarget.Clear();
            ball.move();
            checkOutOfBounds();
            processObjectList();
            //RenderTarget.Render(painter.Drawing);
            PropertyChanged(this, new PropertyChangedEventArgs("RenderTarget"));
        }

        private void processObjectList() 
        {
            painter.Clear();
            painter.Draw(ball);
        }

        private void checkOutOfBounds() 
        {
            if (ball.Position.X <= 0) { ball.Position.X = 0; ball.bounce(BallColisionSide.LEFT); }
            if (ball.Position.X >= GameArea.Width) { ball.Position.X = GameArea.Width; ball.bounce(BallColisionSide.RIGHT); }
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