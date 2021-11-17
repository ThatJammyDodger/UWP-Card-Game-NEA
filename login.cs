using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace Programming_Project
{
    static class login
    {
        //FIELDS
        static public bool PlayersLoggedIn = false;
        static string[] usernames = new string[] { "player1", "player2", "player3" };
        static string[] passwords = new string[] { "pass1", "pass2", "pass3" };
        static public string[] displayNames = new string[2];
        static public string FileValue = "";
        static public bool ProgrammeIsNew = true;

        //METHODS
        static public bool auth(string enteredUsername, string enteredPassword)
        {
            int usernamepos = Array.IndexOf(usernames, enteredUsername);
            if (usernamepos == -1)
            {
                return false;
            }
            else
            {
                if (enteredPassword == passwords[usernamepos])
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        static public async void StayLoggedInChecked(string username1, string username2)
        {
            Windows.Storage.StorageFolder storageFolder = Windows.Storage.ApplicationData.Current.LocalFolder;

            Windows.Storage.StorageFile LoggedIn = await storageFolder.CreateFileAsync("LoggedIn.txt", Windows.Storage.CreationCollisionOption.ReplaceExisting);
            Task t1 = Task.Run(async() =>
            {
                LoggedIn = await storageFolder.CreateFileAsync("LoggedIn.txt", Windows.Storage.CreationCollisionOption.ReplaceExisting);
            });
            t1.Wait();
            Task t2 = Task.Run(async () =>
            {
                await Windows.Storage.FileIO.WriteTextAsync(LoggedIn, $"true\n{username1}\n{username2}");
            });
            t2.Wait();
        }
        static public async void StayLoggedInUnchecked()
        {
            Windows.Storage.StorageFolder storageFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
            Windows.Storage.StorageFile LoggedIn = await storageFolder.CreateFileAsync("LoggedIn.txt", Windows.Storage.CreationCollisionOption.ReplaceExisting);
            Task t1 = Task.Run(async () =>
            {
                LoggedIn = await storageFolder.CreateFileAsync("LoggedIn.txt", Windows.Storage.CreationCollisionOption.ReplaceExisting);
            });
            t1.Wait();
            Task t2 = Task.Run(async () =>
            {
                await Windows.Storage.FileIO.WriteTextAsync(LoggedIn, $"false");
            });
            t2.Wait();
        }
        static public string CheckStayLoggedIn()
        {
            FileValue = "";
            GetFileValue();
            do
            {
                GetFileValue();
            } while (FileValue == "");
            string[] temp = FileValue.Split('\n');
            if (temp[0] == "true")
            {
                PlayersLoggedIn = true;
                displayNames[0] = temp[1];
                displayNames[1] = temp[2];
            } else if (temp[0] == "false")
            {
                PlayersLoggedIn = false;
            }
            return temp[0];
        }
        static public async void GetFileValue()
        {
            Windows.Storage.StorageFolder storageFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
            
            try
            {
                Windows.Storage.StorageFile LoggedIn = await storageFolder.GetFileAsync("LoggedIn.txt");
                Task t = Task.Run(async () =>
                {
                    LoggedIn = await storageFolder.GetFileAsync("LoggedIn.txt");
                });
                t.Wait();
                Task t2 = Task.Run(async () =>
                {
                    FileValue = await Windows.Storage.FileIO.ReadTextAsync(LoggedIn);
                });
                t2.Wait();
                FileValue.Replace("\r", "");
                if (FileValue == "")
                {
                    Task t3 = Task.Run(async () =>
                    {
                        await Windows.Storage.FileIO.WriteTextAsync(LoggedIn, "false");
                    });
                    t3.Wait();
                    FileValue = "false";
                } else { return; }
            } catch (Exception)
            {
                Windows.Storage.StorageFile LoggedIn = await storageFolder.CreateFileAsync("LoggedIn.txt", Windows.Storage.CreationCollisionOption.ReplaceExisting);
                Task t5 = Task.Run(async () =>
                {
                     LoggedIn = await storageFolder.CreateFileAsync("LoggedIn.txt", Windows.Storage.CreationCollisionOption.ReplaceExisting);
                });
                t5.Wait();
                Task t4 = Task.Run(async () =>
                {
                    await Windows.Storage.FileIO.WriteTextAsync(LoggedIn, "false");
                });
                t4.Wait();
                return;
            }
            
        }
    }
}
