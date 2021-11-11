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
            string more = "";
            if (files.FirstTime)
            {
                files.InitiateFile();

                string File = files.GetFileValue();
                files.FirstTime = false;
            }
            else
            {
                string File = files.GetFileValue();

                List<string> names = new List<string>();
                List<int> scores = new List<int>();
                var lines = File.Split("\n");
                foreach (var x in lines)
                {
                    if (x.IndexOf(",") != -1)
                    {
                        int value = 0;
                        var midline = x.Split(",");
                        int.TryParse(midline[1], out value);
                        names.Add(midline[0]);
                        scores.Add(value);
                    }
                }

                files.SortList(ref scores, ref names);

                
                for (int i = 0; i < 5; i++)
                {
                    more += $"{i + 1}) {names[i]} with a score of {scores[i]}.\n";
                }
                Test.Text = more;
            }

            

            //using (var reader = new StreamWriter(@"winners.csv"))
            //{
            //    reader.WriteLine("Simon Cowell,30");
            //    reader.WriteLine("Dev,26");
            //    reader.WriteLine("Dr Who,16");
            //    reader.WriteLine("Mr Bean,20");
            //    reader.WriteLine("Dev's friend,16");
            //}


            //using (var reader = File.AppendText(@"winners.csv"))
            //{
            //    reader.WriteLine($"{players[finalWinner - 1]},{WinnerNoOfCards}");
            //}


            //using (var reader = new StreamReader(@"winners.csv"))
            //{
            //    while (!reader.EndOfStream)
            //    {
            //        var line = reader.ReadLine();
            //        var values = line.Split(',');
            //        int value;
            //        int.TryParse(values[1], out value);
            //        names.Add(values[0]);
            //        scores.Add(value);
            //    }
            //}

            // Create sample file; replace if exists.

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
