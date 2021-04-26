using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


namespace QGAMES
{
    /// <summary>
    /// Stores the important data of the game
    /// We will encypt the fields in a multiplayer game.
    /// </summary>
    [Serializable]
    public class OfflineCountData
    {
        [SerializeField]
        List<byte> poolOfCards = new List<byte>();
        [SerializeField]
        List<byte> player1Cards = new List<byte>();
        [SerializeField]
        List<byte> player2Cards = new List<byte>();
        [SerializeField]
        List<byte> droppedCards = new List<byte>();
        //[SerializeField]
        //int numberOfBooksForPlayer2;
        [SerializeField]
        string player1Id;
        [SerializeField]
        string player2Id;

        public OfflineCountData(string p1Id, string p2Id)
        {
            player1Id = p1Id;
            player2Id = p2Id;
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

        public void AddCardValuesToPlayer(MyPlayer player, List<byte> cardValues)
        {
            if (player.PlayerId.Equals(player1Id))
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

        public void AddCardValueToPlayer(MyPlayer player, byte cardValue)
        {
            if (player.PlayerId.Equals(player1Id))
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

        //public void AddBooksForPlayer(Player player, int numberOfNewBooks)
        //{
        //    if (player.PlayerId.Equals(player1Id))
        //    {
        //        numberOfBooksForPlayer1 += numberOfNewBooks;
        //    }
        //    else
        //    {
        //        numberOfBooksForPlayer2 += numberOfNewBooks;
        //    }
        //}

        public bool GameFinished()
        {
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