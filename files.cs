using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Programming_Project
{
    static class files
    {
        //static bool DoesFileExist;

        //FIELDS
        public static string FileValue = "";
        public static bool FirstTime = true;


        //METHODS

        // we have .AddToFile .ReadFile .InitiateFile
        public static async void AddToFile(int player, int score)
        {
            Windows.Storage.StorageFolder storageFolder = Windows.Storage.ApplicationData.Current.LocalFolder;

            try
            {
                Windows.Storage.StorageFile winners = await storageFolder.CreateFileAsync("winners.csv", Windows.Storage.CreationCollisionOption.FailIfExists); //fail if exists

                await Windows.Storage.FileIO.WriteTextAsync(winners, $"Simon Cowell,30\nDeveloper,26\nDr Who,16\nMr Benn,20\nMr Jenkins,16\n{login.displayNames[player - 1]},{score}");

                //var text = await Windows.Storage.FileIO.ReadTextAsync(winners);
                //var lines = text.Split("\n");]
            }
            catch (Exception)
            {
                Windows.Storage.StorageFile winners = await storageFolder.CreateFileAsync("winners.csv", Windows.Storage.CreationCollisionOption.OpenIfExists);

                await Windows.Storage.FileIO.AppendTextAsync(winners, $"{login.displayNames[player]},{score}\n");
            }
        }

        static public async void InitiateFile()
        {
            Windows.Storage.StorageFolder storageFolder = Windows.Storage.ApplicationData.Current.LocalFolder;

            try
            {
                Windows.Storage.StorageFile winners = await storageFolder.CreateFileAsync("winners.csv", Windows.Storage.CreationCollisionOption.FailIfExists); //fail if exists

                await Windows.Storage.FileIO.WriteTextAsync(winners, "Simon Cowell,30\nDeveloper,26\nDr Who,16\nMr Benn,20\nMr Jenkins,16\n");
            }
            catch (Exception)
            {
                
            }
        }


        static public async void ReadFile()
        {
            Windows.Storage.StorageFolder storageFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
            try
            {
                Windows.Storage.StorageFile winners = await storageFolder.GetFileAsync("winners.csv");
                FileValue = await Windows.Storage.FileIO.ReadTextAsync(winners);
            } catch (Exception e)
            {
                FileValue = $"Unable to recover file: {e}";
            }
        }

        static public string GetFileValue()
        {
            ReadFile();
            return FileValue;
        }

        static public void SortList(ref List<int> score, ref List<string> name)
        {
            int swaps = -1;
            while (swaps != 0)
            {
                swaps = 0;
                for (int i = 0; i <= score.Count - 2; i++)
                {
                    if (score[i + 1] > score[i])
                    {
                        int t = score[i];
                        score[i] = score[i + 1];
                        score[i + 1] = t;

                        string s = name[i];
                        name[i] = name[i + 1];
                        name[i + 1] = s;
                        swaps++;

                    }
                }
            }
        }

    }
}
