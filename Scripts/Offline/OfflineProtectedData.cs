using System;
using System.Collections.Generic;
using UnityEngine;

namespace QGAMES
{
    /// <summary>
    /// Stores the important data of the game
    /// We will encypt the fields in a multiplayer game.
    /// </summary>
    [Serializable]
    public class OfflineProtectedData
    {
        [SerializeField]
        List<byte> poolOfCards = new List<byte>();
        [SerializeField]
        List<byte> player1Cards = new List<byte>();
        [SerializeField]
        List<byte> player2Cards = new List<byte>();
        [SerializeField]
        List<byte> droppedCards = new List<byte>();
        [SerializeField]
        int numberOfBooksForPlayer1;
        [SerializeField]
        int numberOfBooksForPlayer2;
        [SerializeField]
        string player1Id;
        [SerializeField]
        string player2Id;
        [SerializeField]
        string roomId;


        public OfflineProtectedData(string p1Id, string p2Id)
        {
            player1Id = p1Id;
            player2Id = p2Id;
            //add this Later
            //roomId = rId;
        }

        public void SetDroppedCards()
        {

        }

        public void SetPoolOfCards(List<byte> cardValues)
        {
            poolOfCards = cardValues;
        }

        public List<byte> GetPoolOfCards()
        {
            return poolOfCards;
        }

        public List<byte> PlayerCards(MyPlayer player)
        {
            if (player.PlayerId.Equals(player1Id))
            {
                return player1Cards;
            }
            else
            {
                return player2Cards;
            }
        }

        public List<byte> GetDroppedCards()
        {
            return droppedCards;
        }



        public void AddCardValuesToPlayer(string playerId, List<byte> cardValues)
        {
            if (playerId.Equals(player1Id))
            {
                player1Cards.AddRange(cardValues);
                player1Cards.Sort();
            }
            else
            {
                player2Cards.AddRange(cardValues);
                player2Cards.Sort();
            }
        }

        public void AddCardsToDroppedCards(List<byte> cardValues)
        {
            droppedCards.AddRange(cardValues);
            //droppedCards.Sort();
        }

        public void AddCardToDroppedCards(byte cardValues)
        {
            droppedCards.Add(cardValues);
            //droppedCards.Sort();
        }

        public void AddCardValueToPlayer(string playerId, byte cardValue)
        {
            if (playerId.Equals(player1Id))
            {
                player1Cards.Add(cardValue);
                player1Cards.Sort();
            }
            else
            {
                player2Cards.Add(cardValue);
                player2Cards.Sort();
            }
        }

        public void RemoveCardValueFromPlayer(MyPlayer player, byte cardValueToRemove)
        {
            if (player.PlayerId.Equals(player1Id))
            {
                player1Cards.Remove(cardValueToRemove);
            }
            else
            {
                player2Cards.Remove(cardValueToRemove);
            }
        }

        public void RemoveCardValuesFromPlayer(MyPlayer player, List<byte> cardValuesToRemove)
        {
            if (player.PlayerId.Equals(player1Id))
            {
                player1Cards.RemoveAll(cv => cardValuesToRemove.Contains(cv));
            }
            else
            {
                player2Cards.RemoveAll(cv => cardValuesToRemove.Contains(cv));
            }
        }

        public void AddBooksForPlayer(MyPlayer player, int numberOfNewBooks)
        {
            if (player.PlayerId.Equals(player1Id))
            {
                numberOfBooksForPlayer1 += numberOfNewBooks;
            }
            else
            {
                numberOfBooksForPlayer2 += numberOfNewBooks;
            }
        }

        public bool GameFinished()
        {
            if (poolOfCards.Count == 0)
            {
                return true;
            }

            if (player1Cards.Count == 0)
            {
                return true;
            }

            if (player2Cards.Count == 0)
            {
                return true;
            }

            return false;
        }

        public string WinnerPlayerId()
        {
            int player1Sum = 0;
            for (int i = 0; i < player1Cards.Count; i++)
            {
                player1Sum += player1Cards[i];
            }
            int player2Sum = 0;
            for (int i = 0; i < player2Cards.Count; i++)
            {
                player2Sum += player2Cards[i];
            }


            if (player2Sum > player1Sum)
            {
                return player1Id;
            }
            else
            {
                return player2Id;
            }
        }
    }
}