using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml;

namespace Programming_Project.Views
{
    public sealed partial class SigninPage : Page, INotifyPropertyChanged
    {
        public SigninPage()
        {
            InitializeComponent();

            P1Error.Visibility = Visibility.Collapsed;
            P2Error.Visibility = Visibility.Collapsed;
            DisplayError.Visibility = Visibility.Collapsed;

            if (!login.PlayersLoggedIn)
            {
                Player1.Visibility = Visibility.Visible;
                Player2.Visibility = Visibility.Collapsed;
                Display.Visibility = Visibility.Collapsed;
                SuccessMsg.Visibility = Visibility.Collapsed;
                

            } else if (login.PlayersLoggedIn)
            {
                P1Submit.IsEnabled = false;
                P1Username.IsEnabled = false;
                P1Password.IsEnabled = false;

                P2Submit.IsEnabled = false;
                P2Username.IsEnabled = false;
                P2Password.IsEnabled = false;

                DisplaySubmit.IsEnabled = false;
                P1Display.IsEnabled = false;
                P2Display.IsEnabled = false;

                SuccessMsg.Visibility = Visibility.Visible;
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

        private void P1Submit_Click(object sender, RoutedEventArgs e)
        {
            P1Error.Visibility = Visibility.Collapsed;
            P1Submit.IsEnabled = false;
            P1Username.IsEnabled = false;
            P1Password.IsEnabled = false;

            if (login.auth(P1Username.Text,P1Password.Password))
            {
                Player2.Visibility = Visibility.Visible;
            } else
            {
                P1Error.Visibility = Visibility.Visible;
                P1Submit.IsEnabled = true;
                P1Username.IsEnabled = true;
                P1Password.IsEnabled = true;
            }
        }

        private void P2Submit_Click(object sender, RoutedEventArgs e)
        {
            P2Error.Visibility = Visibility.Collapsed;
            P2Submit.IsEnabled = false;
            P2Username.IsEnabled = false;
            P2Password.IsEnabled = false;

            if (login.auth(P2Username.Text, P2Password.Password)&&(P1Username.Text!=P2Username.Text))
            {
                Display.Visibility = Visibility.Visible;
            }
            else
            {
                P2Error.Visibility = Visibility.Visible;
                P2Submit.IsEnabled = true;
                P2Username.IsEnabled = true;
                P2Password.IsEnabled = true;
            }
        }

        private void DisplaySubmit_Click(object sender, RoutedEventArgs e)
        {
            DisplayError.Visibility = Visibility.Collapsed;
            DisplaySubmit.IsEnabled = false;
            P1Display.IsEnabled = false;
            P2Display.IsEnabled = false;

            if (P1Display.Text == "" || P2Display.Text == ""|| P1Display.Text== P2Display.Text)
            {
                DisplayError.Visibility = Visibility.Visible;
                DisplaySubmit.IsEnabled = true;
                P1Display.IsEnabled = true;
                P2Display.IsEnabled = true;
            }
            else
            {
                login.PlayersLoggedIn = true;
                login.displayNames[0] = P1Display.Text;
                login.displayNames[1] = P2Display.Text;

                SuccessMsg.Visibility = Visibility.Visible;
            }
        }
    }
}
