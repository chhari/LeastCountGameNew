using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace QGAMES
{
    /// <summary>
    /// Stores the important data of the game
    /// We will encypt the fields in a multiplayer game.
    /// </summary>
    [Serializable]
    public class ProtectedData
    {
        [SerializeField]
        Dictionary<int, List<byte>> playerValues = new Dictionary<int, List<byte>>();
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


        public ProtectedData(Dictionary<int, List<byte>> temp)
        {
            playerValues = temp;
        }

        public void SetDroppedCards() {

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
            return playerValues[int.Parse(player.PlayerId)];            
        }

        public List<byte> GetDroppedCards()
        {
            return droppedCards;
        }

        public void AddCardValuesToPlayer(string playerId, List<byte> cardValues)
        {

            playerValues[int.Parse(playerId)].AddRange(cardValues);
            playerValues[int.Parse(playerId)].Sort();
        }

        public void AddCardsToDroppedCards( List<byte> cardValues)
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
            playerValues[int.Parse(playerId)].Add(cardValue);
        }

        public void RemoveCardValueFromPlayer(MyPlayer player, byte cardValueToRemove)
        {            
            playerValues[int.Parse(player.PlayerId)].Remove(cardValueToRemove);
        } 

        public void RemoveCardValuesFromPlayer(MyPlayer player, List<byte> cardValuesToRemove)
        {            
            playerValues[int.Parse(player.PlayerId)].RemoveAll(cv => cardValuesToRemove.Contains(cv));
        }

        public void AddBooksForPlayer(MyPlayer player, int numberOfNewBooks)
        {
            //if (player.PlayerId.Equals(player1Id))
            //{
            //    numberOfBooksForPlayer1 += numberOfNewBooks;
            //}
            //else
            //{
            //    numberOfBooksForPlayer2 += numberOfNewBooks;
            //}
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

        public int WinnerPlayerId()
        {            
            List<int> sums = new List<int>();
            Dictionary<int, int> declaringWinner = new Dictionary<int, int>();
            foreach (int keys in playerValues.Keys)
            {
                List<byte> temp = new List<byte>();
                temp = playerValues[keys];
                declaringWinner.Add(keys, temp.Sum(d => d));
            }

            var maxValue = declaringWinner.Values.Max(); // 4

            var keyOfMaxValue = declaringWinner.Aggregate((x, y) => x.Value > y.Value ? x : y).Key; //

            return keyOfMaxValue;
        }
    }
}