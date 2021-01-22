using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using MaterialDesignColors;
using MaterialDesignThemes.Wpf;
using SnakeGame.UI.Entities;

namespace SnakeGame.UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
     
        
        private GameWorld _gameWorld;
        int apples, score, level;

        protected override void OnContentRendered(EventArgs e)
        {
            _gameWorld = new GameWorld(this);
            InitializeScore();
            base.OnContentRendered(e);
        }

        private void InitializeScore()
        {
            apples = 0; 
            score = level = 1;
        }

        private void Window_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (_gameWorld != null)
                switch (e.Key)
                {
                    case Key.W:
                        _gameWorld.UpdateMovementDirection(MovementDirection.Up);
                        break;
                    case Key.A:
                        _gameWorld.UpdateMovementDirection(MovementDirection.Left);
                        break;
                    case Key.S:
                        _gameWorld.UpdateMovementDirection(MovementDirection.Down);
                        break;
                    case Key.D:
                        _gameWorld.UpdateMovementDirection(MovementDirection.Right);
                        break;
                    case Key.Escape:
                        PauseGame();
                        break;
                }
        }

        internal void GameOver()
        {
            _gameWorld.StopGame();
            MessageBox.Show($"Game Over! Level Reached: {level}.You score {score}. And you eat:{apples} apples!!");
        }

        private void RestartClick(object sender, RoutedEventArgs e)
        {
            _gameWorld.StopGame();
            _gameWorld = new GameWorld(this);
            GameWorld.Children.Clear();
            if (!_gameWorld.IsRunning)
            {
                _gameWorld.InitializeGame((int)DifficultySlider.Value, (int)ElementSizeSlider.Value);
                StartBtn.IsEnabled = false;
            }
        }

        private void PauseGame()
        {
            _gameWorld.PauseGame();
            MessageBox.Show("Would you like to continue ?", "Game paused",MessageBoxButton.OKCancel);
       
            _gameWorld.ContinueGame();
        }

        private void StartClick(object sender, RoutedEventArgs e)
        {
            if (!_gameWorld.IsRunning)
            { 
                _gameWorld.InitializeGame((int)DifficultySlider.Value, (int)ElementSizeSlider.Value);
                StartBtn.IsEnabled = false;
            }
        }

        private void OptionsClick(object sender, RoutedEventArgs e)
        {
            StartBtn.IsEnabled = !StartBtn.IsEnabled;
            RestartBtn.IsEnabled = !RestartBtn.IsEnabled;
            this.DialogHost.IsOpen = !this.DialogHost.IsOpen;
        }
        internal void IncrementScore()
        {
            apples += 1;
            if (apples % 3 == 0)
                level += 1;
            score += (int)DifficultySlider.Value * level;
            UpdateScore();           
        }

        internal void UpdateScore()
        {
            ApplesLbl.Content = $"Apple: {apples}";
            ScoreLbl.Content = $"Score: {score}";
            LevelLbl.Content = $"Level: {level }";
        }
    }
}
