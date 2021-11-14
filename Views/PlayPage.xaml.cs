using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media.Imaging;


namespace Programming_Project.Views
{
    public sealed partial class PlayPage : Page, INotifyPropertyChanged
    {
        public PlayPage()
        {
            InitializeComponent();
            Arrow.Opacity = 0;

            if (!login.PlayersLoggedIn)
            {
                DisableContent.Visibility = Visibility.Visible;

                P1.Visibility = Visibility.Collapsed;
                P2.Visibility = Visibility.Collapsed;
                General.Visibility = Visibility.Collapsed;

                P1CardGraphics.Visibility = Visibility.Collapsed;
                P2CardGraphics.Visibility = Visibility.Collapsed;
            }
            else if (login.PlayersLoggedIn)
            {
                DisableContent.Visibility = Visibility.Collapsed;
                P1.Visibility = Visibility.Visible;
                P2.Visibility = Visibility.Visible;
                General.Visibility = Visibility.Visible;

                P1CardGraphics.Visibility = Visibility.Visible;
                P2CardGraphics.Visibility = Visibility.Visible;

                P1Title.Text = login.displayNames[0];
                P2Title.Text = login.displayNames[1];

                P1DrawnHeader.Text = $"{login.displayNames[0]}:";
                P2DrawnHeader.Text = $"{login.displayNames[1]}:";

                UpdateStats();
                if (!cards.GameInProgress)
                {
                    cards.reset();
                    UpdateStats();
                    GameOverMenu.Visibility = Visibility.Collapsed;
                    cards.GameInProgress = true;
                    P1DrawCard.IsEnabled = true;
                    P2DrawCard.IsEnabled = false;

                    SetImg1();
                    SetImg2();
                } else if (cards.GameInProgress)
                {
                    GameOverMenu.Visibility = Visibility.Collapsed;
                    if (cards.PlayerTurn == 1)
                    {
                        P1DrawCard.IsEnabled = true;
                        P2DrawCard.IsEnabled = false;
                        P1Drawn.Text = "";
                        P2Drawn.Text = "";
                        SetImg1();
                        SetImg2();
                    } else if (cards.PlayerTurn == 2)
                    {
                        P1DrawCard.IsEnabled = false;
                        P2DrawCard.IsEnabled = true;
                        P2Drawn.Text = "";
                        SetImg1(cards.findCardNumber(cards.player1card), cards.findCardColour(cards.player1card));
                        SetImg2();
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
            DisappearImg1();
            DisappearImg2();

            P1Drawn.Text = "";
            P2Drawn.Text = "";
            RoundWinner.Text = "";

            P1DrawCard.IsEnabled = false;
            P2DrawCard.IsEnabled = true;

            cards.player1card = cards.drawCard();
            AppearImg1(cards.findCardNumber(cards.player1card), cards.findCardColour(cards.player1card));

            cards.PlayerTurn = 2;
            UpdateStats();

            P2Drawn.Text = "";
        }

        private void P2DrawCard_Click(object sender, RoutedEventArgs e)
        {
            cards.player2card = cards.drawCard();
            AppearImg2(cards.findCardNumber(cards.player2card), cards.findCardColour(cards.player2card));

            int winner = cards.calculateWinner(cards.player1card, cards.player2card);

            RoundWinner.Text = $"{login.displayNames[winner - 1]}";
            cards.giveCards(winner, cards.player1card, cards.player2card);

            if (winner == 1)
            {
                DoArrowToLeft();
            } else if (winner == 2)
            {
                DoArrowToRight();
            }

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
            UpdateStats();
            if (cards.Deck.Count == 0)
            {
                cards.GameInProgress = false;
                GameOverMenu.Visibility = Visibility.Visible;
                int FinalWinner = cards.getFinalWinner();
                GameWinner.Text = $"Winner: {login.displayNames[FinalWinner]}";
                files.AddToFile(FinalWinner, cards.getCards(FinalWinner).Count);
                GameWinnerCardsHeader.Text = $"{login.displayNames[FinalWinner]}'s Cards:\t";
                GameWinnerCards.Text = "";
                foreach (var x in cards.getCards(FinalWinner))
                {
                    GameWinnerCards.Text += $"{x}\t";
                }
                    
            }
        }

        void UpdateStats()
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
            UpdateStats();

            cards.GameInProgress = true;
            P1DrawCard.IsEnabled = true;
            P2DrawCard.IsEnabled = false;
            RoundWinner.Text = "";

            DisappearImg1();
            DisappearImg2();

            GameOverMenu.Visibility = Visibility.Collapsed;
        }

        

        //string NullImgSource = "ms-appx:///Images/null.png";
        //BitmapImage bitmapImage = new BitmapImage();
        //Uri uri = new Uri(NullImgSource);
        //bitmapImage.UriSource = uri;
        //P1CardImage.Source = bitmapImage;

        void SetImg1(int number = 0, string colour = null)
        {
            if (colour == null)
            {
                P1CardImage.Source = new BitmapImage(new Uri("ms-appx:///Images/null.png"));
                P1CardImageNumber.Text = "";
                return;
            } else if (colour == "Black")
            {
                P1CardImage.Source = new BitmapImage(new Uri("ms-appx:///Images/Grey-Background.png"));
            } else if (colour == "Yellow")
            {
                P1CardImage.Source = new BitmapImage(new Uri("ms-appx:///Images/Yellow-Background.png"));
            } else if (colour == "Red")
            {
                P1CardImage.Source = new BitmapImage(new Uri("ms-appx:///Images/Red-Background.png"));
            }
            P1CardImageNumber.Text = number.ToString();
        }

        void SetImg2(int number = 0, string colour = null)
        {
            if (colour == null)
            {
                P2CardImage.Source = new BitmapImage(new Uri("ms-appx:///Images/null.png"));
                P2CardImageNumber.Text = "";
                return;
            } else if (colour == "Black")
            {
                P2CardImage.Source = new BitmapImage(new Uri("ms-appx:///Images/Grey-Background.png"));
            }
            else if (colour == "Yellow")
            {
                P2CardImage.Source = new BitmapImage(new Uri("ms-appx:///Images/Yellow-Background.png"));
            }
            else if (colour == "Red")
            {
                P2CardImage.Source = new BitmapImage(new Uri("ms-appx:///Images/Red-Background.png"));
            }
            P2CardImageNumber.Text = number.ToString();
        }

        void AppearImg1(int number, string colour)
        {
            P1CardGraphics.Visibility = Visibility.Visible;
            SetImg1(number, colour);
            P1CardArriveAnimation.Begin();

            P1CardGraphics.Opacity = 1;
        }
        void DisappearImg1()
        {
            P1CardLeaveAnimation.Begin();

            //P1CardGraphics.Visibility = Visibility.Collapsed; //does not give enough time to complete animation before collapsing
            //P1CardGraphics.Opacity = 1;
        }

        void AppearImg2(int number, string colour)
        {
            P2CardGraphics.Visibility = Visibility.Visible;
            SetImg2(number, colour);
            P2CardArriveAnimation.Begin();
            
            P2CardGraphics.Opacity = 1;
        }
        void DisappearImg2()
        {
            P2CardLeaveAnimation.Begin();

            //P2CardGraphics.Visibility = Visibility.Collapsed; //does not give enough time to complete animation before collapsing
            //P2CardGraphics.Opacity = 1;
        }

        

        void DoArrowToRight()
        {
            Arrow.Source = new BitmapImage(new Uri("ms-appx:///Images/RightArrow.png"));
            RightArrow.Begin();
        }
        void DoArrowToLeft()
        {
            Arrow.Source = new BitmapImage(new Uri("ms-appx:///Images/LeftArrow.png"));
            LeftArrow.Begin();
        }
    }
}
