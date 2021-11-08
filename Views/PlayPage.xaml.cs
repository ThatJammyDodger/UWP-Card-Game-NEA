using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml;

namespace Programming_Project.Views
{
    public sealed partial class PlayPage : Page, INotifyPropertyChanged
    {
        public PlayPage()
        {
            InitializeComponent();

            

            if (!login.PlayersLoggedIn)
            {
                DisableContent.Visibility = Visibility.Visible;
                P1.Visibility = Visibility.Collapsed;
                P2.Visibility = Visibility.Collapsed;
            } else if (login.PlayersLoggedIn)
            {
                DisableContent.Visibility = Visibility.Collapsed;
                P1.Visibility = Visibility.Visible;
                P2.Visibility = Visibility.Visible;

                P1Title.Text = login.displayNames[0];
                P2Title.Text = login.displayNames[1];
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

        private void P1DrawCard_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void P2DrawCard_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
