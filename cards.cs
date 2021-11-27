using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Programming_Project
{
    static class cards
    {
        //PROPERTIES
        static Random rnd = new Random();
        public static bool GameInProgress = false;
        public static List<string> Player1Cards = new List<string>();
        public static List<string> Player2Cards = new List<string>();
        public static List<string> Deck = new List<string>();
        public static int PlayerTurn = 1;

        public static string player1card = "";
        public static string player2card = "";

        //METHODS
        //we have .reset .setDeck, .shuffleDeck, .calculateWinner, .drawCard, .findCardNumber, .findCardColour, .getFinalWinner, .giveCards

        static public void reset()
        {
            setDeck();
            shuffleDeck();
            Player1Cards.Clear();
            Player2Cards.Clear();
            player1card = "";
            player2card = "";
            PlayerTurn = 1;
        }

        static public void setDeck()
        {
            Deck.Clear();
            for (int i = 0; i <= 9; i++)
            {
                Deck.Add($"{i + 1} Red");
                Deck.Add($"{i + 1} Black");
                Deck.Add($"{i + 1} Yellow");
            }
            //Deck = ["1 Red", "1 Black", "1 Yellow","2 Red", "2 Black", "2 Yellow","3 Red", "3 Black", "3 Yellow","4 Red", "4 Black", "4 Yellow","5 Red", "5 Black", "5 Yellow","6 Red", "6 Black", "6 Yellow","7 Red", "7 Black", "7 Yellow","8 Red", "8 Black", "8 Yellow","9 Red", "9 Black", "9 Yellow","10 Red", "10 Black", "10 Yellow"]
        }

        static public void shuffleDeck()
        {
            for (int i = Deck.Count - 1; i > 0; i--)
            {
                int randomIndex = rnd.Next(0, i + 1);

                string temp = Deck[i];
                Deck[i] = Deck[randomIndex];
                Deck[randomIndex] = temp;
            }
        }

        static public List<string> getCards(int player)
        {
            if (player == 0)
            {
                return Player1Cards;
            } else
            {
                return Player2Cards;
            }
        }

        // red beats black
        // yellow beats red
        // black beats yellow

        static public int calculateWinner(string player1card, string player2card)
        {
            if (findCardNumber(player1card) > findCardNumber(player2card))
            {
                return 1;
            }
            else if (findCardNumber(player1card) < findCardNumber(player2card))
            {
                return 2;
            }
            else if ((findCardColour(player1card) == "Red") && (findCardColour(player2card) == "Black"))
            {
                return 1;
            }
            else if ((findCardColour(player1card) == "Yellow") && (findCardColour(player2card) == "Red"))
            {
                return 1;
            }
            else if ((findCardColour(player1card) == "Black") && (findCardColour(player2card) == "Yellow"))
            {
                return 1;
            }


            else if ((findCardColour(player2card) == "Red") && (findCardColour(player1card) == "Black"))
            {
                return 2;
            }
            else if ((findCardColour(player2card) == "Yellow") && (findCardColour(player1card) == "Red"))
            {
                return 2;
            }
            else if ((findCardColour(player2card) == "Black") && (findCardColour(player1card) == "Yellow"))
            {
                return 2;
            }
            else
            {
                return 1;
            }

        }

        static public string drawCard()
        {
            string drawnCard = Deck[0];
            Deck.RemoveAt(0);
            return drawnCard;
        }

        public static int findCardNumber(string card)
        {
            int spaceindex = card.IndexOf(" ");
            int returnn;
            if (int.TryParse(card.Substring(0, spaceindex), out returnn))
            {
                return returnn;
            }
            else
            {
                return 0;
            }
        }

        public static string findCardColour(string card)
        {
            int spaceindex = card.IndexOf(" ");
            return card.Substring(spaceindex + 1, card.Length - 1 - spaceindex);
        }

        public static int getFinalWinner()
        {
            if (Player1Cards.Count > Player2Cards.Count)
            {
                return 0;
            }
            else
            {
                return 1;
            }
        }

        public static void giveCards(int player, string card1, string card2)
        {
            if (player == 1)
            {
                Player1Cards.Add(card1);
                Player1Cards.Add(card2);
            }
            else if (player == 2)
            {
                Player2Cards.Add(card1);
                Player2Cards.Add(card2);
            }
        }
    }
}
