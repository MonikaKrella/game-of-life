using System.IO;
using Microsoft.Win32;
using Newtonsoft.Json;

namespace GameOfLife
{
    public class SaveAndLoadManager
    {
        public void Save<T>(T dataToSave) where T : class 
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "GOL file (*.gol)|*.gol";
            if (saveFileDialog.ShowDialog() == true)
            {
                string settingsJsonContent = JsonConvert.SerializeObject(dataToSave);
                File.WriteAllText(saveFileDialog.FileName, settingsJsonContent);
            }
        }

        public T Load<T>() where T : class
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "GOL file (*.gol)|*.gol";
            if (openFileDialog.ShowDialog() == true)
            {
                string settingsJsonContent = File.ReadAllText(openFileDialog.FileName);
                T loadedData = JsonConvert.DeserializeObject<T>(settingsJsonContent);
                return loadedData ;
            }

            return null;
        }
    }
}