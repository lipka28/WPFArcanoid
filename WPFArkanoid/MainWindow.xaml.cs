using System;
using System.Windows;
using System.Windows.Input;


namespace WPFArkanoid
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            this.DataContext = new Game(new Size((int)ImageTarget.Width, (int)ImageTarget.Height));
            this.SetupEvents();
        }

        /// <summary>
        /// Setup all required event handelrs.
        /// </summary>
        private void SetupEvents() 
        {
            var game = DataContext as Game;

            this.KeyDown += (obj, args) => 
            {
                switch (args.Key)
                {
                    case Key.Right: game.KeyPressed = KeyPressed.RIGHT; break;
                    case Key.Left: game.KeyPressed = KeyPressed.LEFT; break;
                    case Key.Escape: this.Close(); break;
                    case Key.Space: game.KeyPressed = KeyPressed.SPACE; break;
                    default: game.KeyPressed = KeyPressed.NONE; break;
                }
            };

            this.KeyUp += (obj, args) =>
            {
                game.KeyPressed = KeyPressed.NONE;
            };

            game.VictoryReached += VictoryReached;
            game.GameOverReached += GameOverReached;
        }

        private void VictoryReached(object sender, EventArgs e) 
        {
            MessageBox.Show("Congratulations, You won!",
                            "Victory",
                            MessageBoxButton.OK,
                            MessageBoxImage.Information);
        }

        private void GameOverReached(object sender, EventArgs e) 
        {
            MessageBox.Show("You lost. Maybe try again?",
                            "Defeat",
                            MessageBoxButton.OK,
                            MessageBoxImage.Information);
        }
    }
}
