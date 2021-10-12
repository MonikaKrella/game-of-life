using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Controls;
using System.Windows.Forms;
using GameOfLife.Annotations;

namespace GameOfLife
{
    public class GameManager : INotifyPropertyChanged
    {
        private const int MILISECONDS_IN_SECOND = 1000;
        private const int MIN_VALUE_OF_BOARDSIZE = 5;
        private const int MAX_VALUE_OF_BOARDSIZE = 50;
        private const float MIN_TIME_INTERVAL = 0.05f;
        private const float MAX_TIME_INTERVAL = 1;

        //=======PROPERTY & CLASS FIELDS===============================================================================
        private readonly Grid gridGameSpaceReference;
        private readonly List<List<Cell>> listOfCells = new List<List<Cell>>();
        private readonly Timer timer = new Timer();
        private readonly RandomCellsDistribution randomDistribution = new RandomCellsDistribution();

        private int _boardSize;

        public int BoardSize
        {
            get { return _boardSize; }
            set
            {
                if (value < MIN_VALUE_OF_BOARDSIZE)
                {
                    value = MIN_VALUE_OF_BOARDSIZE;
                }
                else if (value > MAX_VALUE_OF_BOARDSIZE)
                {
                    value = MAX_VALUE_OF_BOARDSIZE;
                }
                
                _boardSize = value;
                OnPropertyChanged();
                FitBoardToCurrentSize();
            }
        }

        private double _timeInterval;

        public double TimeInterval
        {
            get { return _timeInterval; }
            set
            {
                if (value < MIN_TIME_INTERVAL)
                {
                    value = MIN_TIME_INTERVAL;
                }
                else if (value > MAX_TIME_INTERVAL)
                {
                    value = MAX_TIME_INTERVAL;
                }
                
                _timeInterval = value;
                OnPropertyChanged();
                timer.Interval = (int) (TimeInterval * MILISECONDS_IN_SECOND);
            }
        }

        //=====CONSTRUCTOR=============================================================================================
        public GameManager(Grid argGridGameSpace)
        {
            gridGameSpaceReference = argGridGameSpace;
            timer.Tick += (o, args) => { CreateNewGeneration(); };
        }

        //======METHODS==============================================================================================

        public void StartTimer()
        {
            timer.Start();
        }

        public void StopTimer()
        {
            timer.Stop();
        }

        public void DistributeCellsRandomly()
        {
            randomDistribution.MakeRandomDistributionOfAliveCells(listOfCells);
        }

        public void CreateNewGeneration()
        {
            foreach (Cell cell in gridGameSpaceReference.Children.OfType<Cell>())
            {
                cell.CheckStatusForNextGeneration(listOfCells);
            }

            foreach (Cell cell in gridGameSpaceReference.Children.OfType<Cell>())
            {
                cell.ChangeCellStatusAtNextGeneration();
            }
        }

        private void FitBoardToCurrentSize()
        {
            int newBoardSize = BoardSize;
            int oldBoardSize = listOfCells.Count;
            gridGameSpaceReference.ShowGridLines = true;
            int differenceOfBoardSize = newBoardSize - oldBoardSize;

            if (differenceOfBoardSize > 0)
            {
                for (int i = 0; i < differenceOfBoardSize; i++)
                {
                    RowDefinition rowDefinition = new RowDefinition();
                    gridGameSpaceReference.RowDefinitions.Add(rowDefinition);

                    ColumnDefinition columnDefinition = new ColumnDefinition();
                    gridGameSpaceReference.ColumnDefinitions.Add(columnDefinition);
                }


                for (int i = 0; i < oldBoardSize; i++)
                {
                    for (int j = oldBoardSize; j < newBoardSize; j++)
                    {
                        Cell cell = new Cell();
                        cell.SetupDefaultCoordinates(i, j);
                        listOfCells[i].Add(cell);
                        gridGameSpaceReference.Children.Add(cell);
                    }
                }

                for (int i = oldBoardSize; i < newBoardSize; i++)
                {
                    List<Cell> listOfRowCells = new List<Cell>();
                    for (int j = 0; j < newBoardSize; j++)
                    {
                        Cell cell = new Cell();
                        cell.SetupDefaultCoordinates(i, j);
                        listOfRowCells.Add(cell);
                        gridGameSpaceReference.Children.Add(cell);
                    }

                    listOfCells.Add(listOfRowCells);
                }
            }
            else if (differenceOfBoardSize < 0)
            {
                for (int i = oldBoardSize - 1; i >= newBoardSize; i--)
                {
                    for (int j = oldBoardSize - 1; j >= 0; j--)
                    {
                        gridGameSpaceReference.Children.Remove(listOfCells[i][j]);
                    }

                    listOfCells.Remove(listOfCells[i]);
                }

                for (int i = 0; i < newBoardSize; i++)
                {
                    for (int j = oldBoardSize - 1; j >= newBoardSize; j--)
                    {
                        gridGameSpaceReference.Children.Remove(listOfCells[i][j]);
                        listOfCells[i].Remove(listOfCells[i][j]);
                    }
                }

                for (int i = oldBoardSize - 1; i >= newBoardSize; i--)
                {
                    gridGameSpaceReference.RowDefinitions.Remove(gridGameSpaceReference.RowDefinitions[i]);
                }

                for (int i = oldBoardSize - 1; i >= newBoardSize; i--)
                {
                    gridGameSpaceReference.ColumnDefinitions.Remove(gridGameSpaceReference.ColumnDefinitions[i]);
                }
            }
        }

        public void ClearAllCellsToBeDead()
        {
            foreach (Cell cell in gridGameSpaceReference.Children.OfType<Cell>())
            {
                cell.ChangeToDead();
            }
        }

        public GameSettings GetGameSettings()
        {
            GameSettings gameSettings = new GameSettings(BoardSize, listOfCells, TimeInterval);
            return gameSettings;
        }

        public void InitializeGameFromSettings(GameSettings gameSettings)
        {
            ClearAllCellsToBeDead();
            BoardSize = gameSettings.Boardsize;
            TimeInterval = gameSettings.TimeInterval;
            FitBoardToCurrentSize();

            for (int i = 0; i < gameSettings.ListOfBoolCells.Count; i++)
            {
                for (int j = 0; j < gameSettings.ListOfBoolCells[i].Count; j++)
                {
                    if (gameSettings.ListOfBoolCells[i][j] == true)
                    {
                        listOfCells[i][j].ChangeToAlive();
                    }
                    else
                    {
                        listOfCells[i][j].ChangeToDead();
                    }
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}