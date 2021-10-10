using System.Collections.Generic;
using System.Windows.Forms;
using System.Windows.Input;
using Newtonsoft.Json;

namespace GameOfLife
{
    public class GameSettings
    {
        public int Boardsize { get; set; }
        public List<List<bool>> ListOfBoolCells { get; set; }
        public double TimeInterval { get; set; }

        public GameSettings(int boardSize, List<List<Cell>> listOfCells ,double timeInterval)
        {
            Boardsize = boardSize;
            TimeInterval = timeInterval;
            ChangeListOfCellsToListOfBools(listOfCells);
            
        }
        
        [JsonConstructor]
        public GameSettings(int boardSize, List<List<bool>> listOfBoolCells ,double timeInterval)
        {
            Boardsize = boardSize;
            TimeInterval = timeInterval;
            ListOfBoolCells = listOfBoolCells;
        }
        
        

        private void ChangeListOfCellsToListOfBools(List<List<Cell>> listOfCells)
        {
            ListOfBoolCells = new List<List<bool>>();
            
            foreach (var list in listOfCells)
            {
                List<bool> singleListOfBoolsFromCells = new List<bool>();
                foreach (var cell in list)
                {
                    singleListOfBoolsFromCells.Add(cell.IsAlive);
                }
                ListOfBoolCells.Add(singleListOfBoolsFromCells);
            }
        }

    }

    /*public static class StringExtensions
    {
        public static bool IsThisTextPretty(this string text)
        {
            return true;
        }
    }*/

    /*public static class CellsExtensions
    {
        public static List<List<bool>> ChangeListOfCellsToListOfBools(this List<List<Cell>> listOfCells)
        {
            List<List<bool>> listOfBoolCells = new List<List<bool>>();
            
            foreach (var list in listOfCells)
            {
                List<bool> singleListOfBoolsFromCells = new List<bool>();
                foreach (var cell in list)
                {
                    singleListOfBoolsFromCells.Add(cell.IsAlive);
                }
                listOfBoolCells.Add(singleListOfBoolsFromCells);
            }

            return listOfBoolCells;
        }
    }*/
    
}