using System;
using System.Collections.Generic;

namespace GameOfLife
{
    public class RandomCellsDistribution
    {
        private const float PERCENTAGE_OF_CELLS_TO_MAKE_ALIVE = 0.2f;

        private readonly Random random = new Random();

        public void MakeRandomDistributionOfAliveCells(IReadOnlyList<IReadOnlyList<Cell>> listOfCells)
        {
            int totalNumberOfCells = (listOfCells.Count * listOfCells[0].Count);
            int numberOfCellsToMakeAlive = (int) (totalNumberOfCells * PERCENTAGE_OF_CELLS_TO_MAKE_ALIVE);
            int lenghtOfBoardSide = listOfCells.Count;

            ChangeStatusInRandomCells(numberOfCellsToMakeAlive, lenghtOfBoardSide, listOfCells);
        }

        private void ChangeStatusInRandomCells(int numberOfCellsToMakeAlive, int lenghtOfBoardSide, IReadOnlyList<IReadOnlyList<Cell>> listOfCells)
        {
            for (int i = 0; i < numberOfCellsToMakeAlive; i++)
            {
                int x, y;
                do
                {
                    x = CreateRandomCoordinate(lenghtOfBoardSide);
                    y = CreateRandomCoordinate(lenghtOfBoardSide);
                } while (listOfCells[y][x].IsAlive == true);

                listOfCells[y][x].ChangeToAlive();
            }
        }

        private int CreateRandomCoordinate(int lenghtOfBoardSide)
        {
            int coordianteForAliveCell = random.Next(0, lenghtOfBoardSide);
            return coordianteForAliveCell;
        }
    }
}