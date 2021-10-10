using System.Windows;


namespace GameOfLife
{
    public partial class MainWindow
    {
        private readonly GameManager gameManager;
        private readonly SaveAndLoadManager saveAndLoadManager = new SaveAndLoadManager();

        public MainWindow()
        {
            InitializeComponent();
            gameManager = new GameManager(GridGameSpace);
            
            gameManager.TimeInterval = frequencyOfChangingGenerations.Minimum;
            gameManager.BoardSize = (int)sliderBoardSize.Minimum;
            DataContext = gameManager;
        }

        private void ClearButton_OnClick(object sender, RoutedEventArgs e)
        {
            gameManager.ClearAllCellsToBeDead();
        }

        private void NextGeneration_OnClick(object sender, RoutedEventArgs e)
        {
            gameManager.CreateNewGeneration();
        }

        private void Exit_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
        
        private void AutomaticGeneretions_OnClick(object sender, RoutedEventArgs e)
        {
            AutomaticGeneretionsButton.IsEnabled = false;
            gameManager.StartTimer();
        }

        private void StopButton_OnClick(object sender, RoutedEventArgs e)
        {
            gameManager.StopTimer();
            AutomaticGeneretionsButton.IsEnabled = true;
        }

        private void RandomButton_OnClick(object sender, RoutedEventArgs e)
        {
            gameManager.DistributeCellsRandomly();
        }

        private void Save_OnClick(object sender, RoutedEventArgs e)
        {
            saveAndLoadManager.Save<GameSettings>(gameManager.GetGameSettings());
        }

        private void Load_OnClick(object sender, RoutedEventArgs e)
        {
            GameSettings gameSettings = saveAndLoadManager.Load<GameSettings>();
            if (gameSettings != null)
            {
              gameManager.InitializeGameFromSettings(gameSettings);  
            }
        }
    }
}