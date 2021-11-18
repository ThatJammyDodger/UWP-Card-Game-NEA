using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml;
using System.Threading.Tasks;
using System.Threading;

namespace Programming_Project.Views
{
    public sealed partial class SigninPage : Page, INotifyPropertyChanged
    {
        string check = "";
        public SigninPage()
        {
            InitializeComponent();


            //Task t1 = Task.Run(() =>
            //{
            //    login.StayLoggedInChecked("a", "azerty");
            //});

            if ((!login.PlayersLoggedIn)&&(login.ProgrammeIsNew))
            {
                check = login.CheckStayLoggedIn();
            } else { login.ProgrammeIsNew = false; }

            //foreach (var x in login.displayNames)
            //{
            //    P1Username.Text += $"{x}\t";
            //}

            if (check == "true")
            {
                login.PlayersLoggedIn = true;
            }

            P1Error.Visibility = Visibility.Collapsed;
            P2Error.Visibility = Visibility.Collapsed;
            DisplayError.Visibility = Visibility.Collapsed;

            if (!login.PlayersLoggedIn)
            {
                Player1.Visibility = Visibility.Visible;
                Player2.Visibility = Visibility.Collapsed;
                Display.Visibility = Visibility.Collapsed;
                SuccessMsgGrid.Visibility = Visibility.Collapsed;

                Logout.Visibility = Visibility.Collapsed;

                LoggedInInfo.Visibility = Visibility.Collapsed;
                
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

                RememberButton.IsEnabled = false;

                SuccessMsgGrid.Visibility = Visibility.Visible;
                Logout.Visibility = Visibility.Visible;

                SetLoggedInInfo();

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
            RememberButton.IsEnabled = false;

            if (P1Display.Text == "" || P2Display.Text == "" || P1Display.Text == P2Display.Text || P1Display.Text.Length > 8 || P2Display.Text.Length > 8)
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

                SuccessMsgGrid.Visibility = Visibility.Visible;
                Logout.Visibility = Visibility.Visible;

                if((bool)RememberButton.IsChecked) { login.StayLoggedInChecked(P1Display.Text, P2Display.Text); } else { login.StayLoggedInUnchecked(); }

                SetLoggedInInfo();
            }
        }

        private void GoToPlayPage_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(PlayPage));
        }

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            login.PlayersLoggedIn = false;
            Task t1 = Task.Run(() =>
            {
                login.StayLoggedInUnchecked();
            });
            t1.Wait();
            //Thread.Sleep(100);
            check = "false";
            login.PlayersLoggedIn = false;
            LoggedInInfo.Visibility = Visibility.Collapsed;
            this.Frame.Navigate(typeof(SigninPage));

        }

        private void SetLoggedInInfo()
        {
            LoggedInInfo.Visibility = Visibility.Visible;
            LoggedInInfoP1.Text = $"Player 1: {login.displayNames[0]}";
            LoggedInInfoP2.Text = $"Player 2: {login.displayNames[1]}";
        }

        private void InfoButton_Click(object sender, RoutedEventArgs e)
        {
            ShowInfoDialogue();
        }

        async void ShowInfoDialogue()
        {
            ContentDialog InfoDialogue = new ContentDialog()
            {
                Title = "How To Play?",
                Content = "Now that you have logged in, you can go to the 'Play' tab to play the game proper.\n\nAfter that, Player 1 starts by clicking 'Draw Card' underneath their name. Their randomly assigned card should then popout underneath.\n\nThe same then happens for Player 2.\n\nEach round, the blue arrow points towards the player who has won the round. All info is displayed as text just above too.\n\nThis then repeats until the end of the game, when the overall winner (with the most cards) will be indicated.\n\nThe results tab displays the players who have won their game with the most cards of all-time (on this device). Try and get yourself onto this leaderboard!\n\n I hope you really like the game and have fun!",
                CloseButtonText = "Ok"
            };

            await InfoDialogue.ShowAsync();
        }
    }
}
