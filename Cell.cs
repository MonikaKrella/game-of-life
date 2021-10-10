using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Media;

namespace GameOfLife
{
    public class Cell : Button
    {
        private const int THREE_NEIGHBOURS_FROM_RULES = 3;
        private const int TWO_NEIGHBOURS_FROM_RULES = 2;
        private readonly SolidColorBrush COLOR_OF_DEAD_CELL = Brushes.White;
        private readonly SolidColorBrush COLOR_OF_ALIVE_CELL = Brushes.Black;

        private int X { get; set; }
        private int Y { get; set; }
        public bool IsAlive { get; private set; }
        private bool ShoulBecomeAlive { get; set; }


        public Cell()
        {
            Click += (sender, args) =>
            {
                if (IsAlive == false)
                {
                    ChangeToAlive();
                }
                else if (IsAlive == true)
                {
                    ChangeToDead();
                }
            };
        }

        //======METHODS==============================================================================================

        public void SetupDefaultCoordinates(int i, int j)
        {
            X = j;
            Y = i;
            ChangeToDead();
            Grid.SetColumn(this, j);
            Grid.SetRow(this, i);
        }

        public void CheckStatusForNextGeneration(IReadOnlyList<IReadOnlyList<Cell>> listOfCells)
        {
            List<Cell> listOfAliveNeighbours = new List<Cell>();

            if (Y - 1 >= 0)
            {
                if (X - 1 >= 0 && listOfCells[Y - 1][X - 1].IsAlive == true)
                {
                    listOfAliveNeighbours.Add(listOfCells[Y - 1][X - 1]);
                }

                if (listOfCells[Y - 1][X].IsAlive == true)
                {
                    listOfAliveNeighbours.Add(listOfCells[Y - 1][X]);
                }

                if (X + 1 <= listOfCells.Count - 1 && listOfCells[Y - 1][X + 1].IsAlive == true)
                {
                    listOfAliveNeighbours.Add(listOfCells[Y - 1][X + 1]);
                }
            }

            if (X - 1 >= 0 && listOfCells[Y][X - 1].IsAlive == true)
            {
                listOfAliveNeighbours.Add(listOfCells[Y][X - 1]);
            }

            if (X + 1 <= listOfCells.Count - 1 && listOfCells[Y][X + 1].IsAlive == true)
            {
                listOfAliveNeighbours.Add(listOfCells[Y][X + 1]);
            }

            if (Y + 1 <= listOfCells.Count - 1)
            {
                if (X - 1 >= 0 && listOfCells[Y + 1][X - 1].IsAlive == true)
                {
                    listOfAliveNeighbours.Add(listOfCells[Y + 1][X - 1]);
                }

                if (listOfCells[Y + 1][X].IsAlive == true)
                {
                    listOfAliveNeighbours.Add(listOfCells[Y + 1][X]);
                }

                if (X + 1 <= listOfCells.Count - 1 && listOfCells[Y + 1][X + 1].IsAlive == true)
                {
                    listOfAliveNeighbours.Add(listOfCells[Y + 1][X + 1]);
                }
            }

            if (IsAlive == false && listOfAliveNeighbours.Count == THREE_NEIGHBOURS_FROM_RULES)
            {
                ShoulBecomeAlive = true;
            }
            else if (IsAlive == true && (listOfAliveNeighbours.Count == THREE_NEIGHBOURS_FROM_RULES ||
                                         listOfAliveNeighbours.Count == TWO_NEIGHBOURS_FROM_RULES))
            {
                ShoulBecomeAlive = true;
            }
            else
            {
                ShoulBecomeAlive = false;
            }
        }

        public void ChangeCellStatusAtNextGeneration()
        {
            if (ShoulBecomeAlive == true)
            {
                IsAlive = true;
                Background = COLOR_OF_ALIVE_CELL;
            }

            if (ShoulBecomeAlive == false)
            {
                IsAlive = false;
                Background = COLOR_OF_DEAD_CELL;
            }
        }

        public void ChangeToAlive()
        {
            IsAlive = true;
            Background = COLOR_OF_ALIVE_CELL;
        }

        public void ChangeToDead()
        {
            IsAlive = false;
            Background = COLOR_OF_DEAD_CELL;
        }
    }
}