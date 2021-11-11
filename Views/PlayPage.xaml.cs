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

                updateStats();
                if (!cards.GameInProgress)
                {
                    cards.reset();
                    updateStats();
                    GameOverMenu.Visibility = Visibility.Collapsed;
                    cards.GameInProgress = true;
                    P1DrawCard.IsEnabled = true;
                    P2DrawCard.IsEnabled = false;
                } else if (cards.GameInProgress)
                {
                    GameOverMenu.Visibility = Visibility.Collapsed;
                    if (cards.PlayerTurn == 1)
                    {
                        P1DrawCard.IsEnabled = true;
                        P2DrawCard.IsEnabled = false;
                        P1Drawn.Text = "";
                        P2Drawn.Text = "";
                    } else if (cards.PlayerTurn == 2)
                    {
                        P1DrawCard.IsEnabled = false;
                        P2DrawCard.IsEnabled = true;
                        P2Drawn.Text = "";
                    }
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

        

        private void P1DrawCard_Click(object sender, RoutedEventArgs e)
        {
            P1Drawn.Text = "";
            P2Drawn.Text = "";
            RoundWinner.Text = "";

            P1DrawCard.IsEnabled = false;
            P2DrawCard.IsEnabled = true;

            cards.player1card = cards.drawCard();

            cards.PlayerTurn = 2;
            updateStats();

            P2Drawn.Text = "";
        }

        private void P2DrawCard_Click(object sender, RoutedEventArgs e)
        {
            cards.player2card = cards.drawCard();

            int winner = cards.calculateWinner(cards.player1card, cards.player2card);

            RoundWinner.Text = $"{login.displayNames[winner - 1]}";
            cards.giveCards(winner, cards.player1card, cards.player2card);

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
            cards.PlayerTurn = 1;
            updateStats();
            if (cards.Deck.Count == 0)
            {
                cards.GameInProgress = false;
                GameOverMenu.Visibility = Visibility.Visible;
                int FinalWinner = cards.getFinalWinner();
                GameWinner.Text = $"Winner: {login.displayNames[FinalWinner]}";
                files.AddToFile(FinalWinner, cards.getCards(FinalWinner).Count);
                GameWinnerCards.Text = $"{login.displayNames[FinalWinner]}'s Cards:\t";
                foreach (var x in cards.getCards(FinalWinner))
                {
                    GameWinnerCards.Text += $"{x}\t";
                }
                    
            }
        }

        void updateStats()
        {
            P1CardsCount.Text = $"Has {cards.Player1Cards.Count}/30 cards";
            P1RoundsCount.Text = $"Has won {cards.Player1Cards.Count / 2}/15 rounds";

            P2CardsCount.Text = $"Has {cards.Player2Cards.Count}/30 cards";
            P2RoundsCount.Text = $"Has won {cards.Player2Cards.Count / 2}/15 rounds";

            GeneralCardsCount.Text = $"There are {cards.Deck.Count} cards left";
            GeneralRoundsCount.Text = $"There are {cards.Deck.Count / 2} rounds left";

            P1Drawn.Text = cards.player1card;
            P2Drawn.Text = cards.player2card;

            //if (cards.cards.player1cards.Count > 0)
            //{
            //    P1Drawn.Text = $"The {cards.findCardColour(cards.cards.player1cards[cards.cards.player1cards.Count - 1])} {cards.findCardNumber(cards.cards.player1cards[cards.cards.player1cards.Count - 1])}";
            //}
            //if (cards.cards.player2cards.Count > 0)
            //{
            //    P1Drawn.Text = $"The {cards.findCardColour(cards.cards.player2cards[cards.cards.player2cards.Count - 1])} {cards.findCardNumber(cards.cards.player2cards[cards.cards.player2cards.Count - 1])}";
            //}
        }

        private void PlayAgain_Click(object sender, RoutedEventArgs e)
        {
            cards.reset();
            cards.setDeck();
            cards.shuffleDeck();
            updateStats();

            cards.GameInProgress = true;
            P1DrawCard.IsEnabled = true;
            P2DrawCard.IsEnabled = false;
            RoundWinner.Text = "";

            GameOverMenu.Visibility = Visibility.Collapsed;
        }
    }
}
