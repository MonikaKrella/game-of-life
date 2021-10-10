using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Forms;

namespace GameOfLife
{
    public class GameManager
    {
        private const int MILISECONDS_IN_SECOND = 1000;
        
        //=======PROPERTY & CLASS FIELDS===============================================================================
        private readonly Grid gridGameSpaceReference;
        private readonly List<List<Cell>> listOfCells = new List<List<Cell>>();
        private readonly Timer timer = new Timer();
        private readonly RandomCellsDistribution randomDistribution = new RandomCellsDistribution();

        private int _boardSize;

        public int BoardSize 
        {
            get
            {
                return _boardSize;
            }
            set
            {
                _boardSize = value;
                FitBoardToCurrentSize();
            }
        }

        private double _timeInterval;
        public double TimeInterval
        {
            get
            {
                return _timeInterval;
            }
            set
            {
                _timeInterval = value;
                timer.Interval = (int) (TimeInterval * MILISECONDS_IN_SECOND);
            }
        }
        
        //=====CONSTRUCTOR=============================================================================================
        public GameManager(Grid argGridGameSpace)
        {
            gridGameSpaceReference = argGridGameSpace;
            timer.Tick += (o, args) => {CreateNewGeneration(); };
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
            int newValue = BoardSize;
            int oldValue = listOfCells.Count;
            gridGameSpaceReference.ShowGridLines = true;
            int differenceOldVsNewValue = newValue - oldValue;

            if (differenceOldVsNewValue > 0)
            {
                for (int i = 0; i < differenceOldVsNewValue; i++)
                {
                    RowDefinition rowDefinition = new RowDefinition();
                    gridGameSpaceReference.RowDefinitions.Add(rowDefinition);

                    ColumnDefinition columnDefinition = new ColumnDefinition();
                    gridGameSpaceReference.ColumnDefinitions.Add(columnDefinition);
                }


                for (int i = 0; i < oldValue; i++)
                {
                    for (int j = oldValue; j < newValue; j++)
                    {
                        Cell cell = new Cell();
                        cell.SetupDefaultCoordinates(i, j);
                        listOfCells[i].Add(cell);
                        gridGameSpaceReference.Children.Add(cell);
                    }
                }

                for (int i = oldValue; i < newValue; i++)
                {
                    List<Cell> listOfRowCells = new List<Cell>();
                    for (int j = 0; j < newValue; j++)
                    {
                        Cell cell = new Cell();
                        cell.SetupDefaultCoordinates(i, j);
                        listOfRowCells.Add(cell);
                        gridGameSpaceReference.Children.Add(cell);
                    }

                    listOfCells.Add(listOfRowCells);
                }
            }
            else if (differenceOldVsNewValue < 0)
            {
                for (int i = oldValue - 1; i >= newValue; i--)
                {
                    for (int j = oldValue - 1; j >= 0; j--)
                    {
                        gridGameSpaceReference.Children.Remove(listOfCells[i][j]);
                    }

                    listOfCells.Remove(listOfCells[i]);
                }

                for (int i = 0; i < newValue; i++)
                {
                    for (int j = oldValue - 1; j >= newValue; j--)
                    {
                        gridGameSpaceReference.Children.Remove(listOfCells[i][j]);
                        listOfCells[i].Remove(listOfCells[i][j]);
                    }
                }

                for (int i = oldValue - 1; i >= newValue; i--)
                {
                    gridGameSpaceReference.RowDefinitions.Remove(gridGameSpaceReference.RowDefinitions[i]);
                }

                for (int i = oldValue - 1; i >= newValue; i--)
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
                    if (gameSettings.ListOfBoolCells[i][j] == true )
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
        
    }
}