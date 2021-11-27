using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.IO;
using Windows.UI.Xaml.Controls;
using System.Collections.Generic;

namespace Programming_Project.Views
{
    public sealed partial class ResultsPage : Page, INotifyPropertyChanged
    {
        public ResultsPage()
        {
            InitializeComponent();
            string output = "";
            if (files.FirstTime)
            {
                files.InitiateFile();
                _ = files.GetFileValue();
                files.FirstTime = false;
            }
            else
            {
                string File = files.GetFileValue();

                List<string> names = new List<string>();
                List<int> scores = new List<int>();
                string[] lines = File.Split("\n");
                foreach (var x in lines)
                {
                    if (x.IndexOf(",") != -1)
                    {
                        var midline = x.Split(",");
                        int.TryParse(midline[1], out int value);
                        names.Add(midline[0]);
                        scores.Add(value);
                    }
                }

                files.SortList(ref scores, ref names);

                if (names.Count > 0)
                {
                    for (int i = 0; i < 5; i++)
                    {
                        output += $"\n{i + 1}) {names[i]} with a score of {scores[i]}.\n";
                    }
                    Test.Text = output;
                }
            }
        }

        

        public event PropertyChangedEventHandler PropertyChanged;

        private void Set<T>(ref T storage, T value, [CallerMemberName]string propertyName = null)
        {
            if (Equals(storage, value))
            {
                return;
            }

            storage = value;
            OnPropertyChanged(propertyName);
        }

        private void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
