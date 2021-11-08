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

            cards.setDeck();
            cards.shuffleDeck();
            updateStats();

            P1DrawCard.IsEnabled = true;
            P2DrawCard.IsEnabled = false;

            if (!login.PlayersLoggedIn)
            {
                DisableContent.Visibility = Visibility.Visible;
                P1.Visibility = Visibility.Collapsed;
                P2.Visibility = Visibility.Collapsed;
                General.Visibility = Visibility.Collapsed;
            }
            else if (login.PlayersLoggedIn)
            {
                DisableContent.Visibility = Visibility.Collapsed;
                P1.Visibility = Visibility.Visible;
                P2.Visibility = Visibility.Visible;
                General.Visibility = Visibility.Visible;

                P1Title.Text = login.displayNames[0];
                P2Title.Text = login.displayNames[1];

                P1DrawnHeader.Text = $"{login.displayNames[0]}:";
                P2DrawnHeader.Text = $"{login.displayNames[1]}:";
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

        string player1card;
        string player2card;

        private void P1DrawCard_Click(object sender, RoutedEventArgs e)
        {
            P1Drawn.Text = "";
            P2Drawn.Text = "";

            P1DrawCard.IsEnabled = false;
            P2DrawCard.IsEnabled = true;

            player1card = cards.drawCard();

            P1Drawn.Text = player1card;

            updateStats();
        }

        private void P2DrawCard_Click(object sender, RoutedEventArgs e)
        {
            if (cards.Deck.Count <= 1)
            {
                P1DrawCard.IsEnabled = false;
                P2DrawCard.IsEnabled = false;
            }
            else
            {
                P2DrawCard.IsEnabled = false;
                P1DrawCard.IsEnabled = true;
            }

            player2card = cards.drawCard();

            P2Drawn.Text = player2card;

            int winner = cards.calculateWinner(player1card, player2card);

            RoundWinner.Text = $"{login.displayNames[winner - 1]}";
            cards.giveCards(winner, player1card, player2card);

            

            updateStats();
        }

        void updateStats()
        {
            P1CardsCount.Text = $"Has {cards.Player1Cards.Count}/30 cards";
            P1RoundsCount.Text = $"Has won {cards.Player1Cards.Count / 2}/15 rounds";

            P2CardsCount.Text = $"Has {cards.Player2Cards.Count}/30 cards";
            P2RoundsCount.Text = $"Has won {cards.Player2Cards.Count / 2}/15 rounds";

            GeneralCardsCount.Text = $"There are {cards.Deck.Count} cards left";
            GeneralRoundsCount.Text = $"There are {cards.Deck.Count / 2} rounds left";

            //if (cards.Player1Cards.Count > 0)
            //{
            //    P1Drawn.Text = $"The {cards.findCardColour(cards.Player1Cards[cards.Player1Cards.Count - 1])} {cards.findCardNumber(cards.Player1Cards[cards.Player1Cards.Count - 1])}";
            //}
            //if (cards.Player2Cards.Count > 0)
            //{
            //    P1Drawn.Text = $"The {cards.findCardColour(cards.Player2Cards[cards.Player2Cards.Count - 1])} {cards.findCardNumber(cards.Player2Cards[cards.Player2Cards.Count - 1])}";
            //}
        }
    }
}
