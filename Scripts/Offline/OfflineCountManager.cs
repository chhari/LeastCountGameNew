using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace QGAMES
{
    [Serializable]
    public class OfflineCountManager
    {
        MyPlayer localPlayer;
        MyPlayer remotePlayer;
        public List<Card> DroppedCards = new List<Card>();

        [SerializeField]
        OfflineProtectedData protectedData;


        public OfflineCountManager(MyPlayer local, MyPlayer remote)
        {
            localPlayer = local;
            remotePlayer = remote;
            protectedData = new OfflineProtectedData(localPlayer.PlayerId, remotePlayer.PlayerId);
        }

        public void Shuffle()
        {
            List<byte> cardValues = new List<byte>();

            for (byte value = 0; value < 52; value++)
            {
                cardValues.Add(value);
            }

            List<byte> poolOfCards = new List<byte>();

            for (int index = 0; index < 52; index++)
            {
                int valueIndexToAdd = UnityEngine.Random.Range(0, cardValues.Count);

                byte valueToAdd = cardValues[valueIndexToAdd];
                poolOfCards.Add(valueToAdd);
                cardValues.Remove(valueToAdd);
            }

            protectedData.SetPoolOfCards(poolOfCards);
        }

        public void DealCardValuesToPlayer(MyPlayer player, int numberOfCards)
        {
            List<byte> poolOfCards = protectedData.GetPoolOfCards();

            int numberOfCardsInThePool = poolOfCards.Count;
            int start = numberOfCardsInThePool - 1 - numberOfCards;

            List<byte> cardValues = poolOfCards.GetRange(start, numberOfCards);
            poolOfCards.RemoveRange(start, numberOfCards);

            protectedData.AddCardValuesToPlayer(player.PlayerId, cardValues);
        }

        public byte DrawCardValue()
        {
            List<byte> poolOfCards = protectedData.GetPoolOfCards();

            int numberOfCardsInThePool = poolOfCards.Count;

            if (numberOfCardsInThePool > 0)
            {
                byte cardValue = poolOfCards[numberOfCardsInThePool - 1];
                poolOfCards.Remove(cardValue);

                return cardValue;
            }

            return Constants.POOL_IS_EMPTY;
        }

        public byte FirstDroppedCard()
        {
            List<byte> poolOfCards = protectedData.GetPoolOfCards();

            int numberOfCardsInThePool = poolOfCards.Count;

            if (numberOfCardsInThePool > 0)
            {
                byte cardValue = poolOfCards[numberOfCardsInThePool - 1];
                poolOfCards.Remove(cardValue);
                AddCardToDroppedCards(cardValue);

                return cardValue;
            }

            return Constants.POOL_IS_EMPTY;
        }

        public int GetDroppedCardsCount() {
            return protectedData.GetDroppedCards().Count;
        }

        public Card DrawDroppedCard()
        {
            
            int numberOfCardsInThePool = DroppedCards.Count;

            if (numberOfCardsInThePool > 1)
            {
                Card card = DroppedCards[numberOfCardsInThePool - 2];
                DroppedCards.Remove(card);

                return card;
                    
            }

            return null;
        }

        public List<byte> PlayerCards(MyPlayer player)
        {
            return protectedData.PlayerCards(player);
        }

        public void AddCardValuesToPlayer(MyPlayer player, List<byte> cardValues)
        {
            protectedData.AddCardValuesToPlayer(player.PlayerId, cardValues);
        }

        public void AddCardValueToPlayer(MyPlayer player, byte cardValue)
        {
            protectedData.AddCardValueToPlayer(player.PlayerId, cardValue);
        }

        public void AddCardsToDroppedCards(List<byte> cardValue)
        {
            protectedData.AddCardsToDroppedCards(cardValue);
        }

        public void AddCardToDroppedCards(byte cardValue)
        {
            protectedData.AddCardToDroppedCards(cardValue);
        }

        public void RemoveCardValuesFromPlayer(MyPlayer player, List<byte> cardValuesToRemove)
        {
            protectedData.RemoveCardValuesFromPlayer(player, cardValuesToRemove);
        }

        //public void AddBooksForPlayer(Player player, int numberOfNewBooks)
        //{
        //    protectedData.AddBooksForPlayer(player, numberOfNewBooks);
        //}

        public MyPlayer Winner()
        {
            string winnerPlayerId = protectedData.WinnerPlayerId();
            if (winnerPlayerId.Equals(localPlayer.PlayerId))
            {
                return localPlayer;
            }
            else
            {
                return remotePlayer;
            }
        }

        public bool GameFinished()
        {
            return protectedData.GameFinished();
        }

        public void RepositionDroppedCards(CardAnimator cardAnimator)
        {
            for(int i=0;i< DroppedCards.Count;i++)
            {
                Vector2 newPos =  cardAnimator.droppedCardPosition + Vector2.right * Constants.PLAYER_CARD_POSITION_OFFSET * i;
                cardAnimator.AddCardAnimation(DroppedCards[i], newPos);
            }
        }

        public void AddToDropCardsReference(Card card) {
            DroppedCards.Add(card);
        }

        public bool DropCardsFromPlayer(MyPlayer player, Card selectedCard)
        {
            List<byte> playerCards = protectedData.PlayerCards(player);
            DroppedCards.Add(selectedCard);

            protectedData.AddCardToDroppedCards(selectedCard.GetValue());
   

            protectedData.RemoveCardValueFromPlayer(player, selectedCard.GetValue());



            return true;
        }

        public int TakeCardFromDroppedCards(MyPlayer player, Ranks ranks)
        {
            List<byte> xdroppedCards = protectedData.GetDroppedCards();

            byte result = xdroppedCards[xdroppedCards.Count - 1]; ;

            protectedData.AddCardValueToPlayer(player.PlayerId, result);

            return result;
        }

        public Dictionary<Ranks, List<byte>> GetBooks(MyPlayer player)
        {
            List<byte> playerCards = protectedData.PlayerCards(player);

            var groups = playerCards.GroupBy(Card.GetRank).Where(g => g.Count() == 4);

            if (groups.Count() > 0)
            {
                Dictionary<Ranks, List<byte>> setOfFourDictionary = new Dictionary<Ranks, List<byte>>();

                foreach (var group in groups)
                {
                    List<byte> cardValues = new List<byte>();

                    foreach (var value in group)
                    {
                        cardValues.Add(value);
                    }

                    setOfFourDictionary[group.Key] = cardValues;
                }

                return setOfFourDictionary;
            }
            
            return null;
        }

        //public Ranks SelectRandomRanksFromPlayersCardValues(Player player)
        //{
        //    List<byte> playerCards = protectedData.PlayerCards(player);
        //    int index = UnityEngine.Random.Range(0, playerCards.Count);

        //    return Card.GetRank(playerCards[index]);
        //}

        public MyPlayer getWinner(MyPlayer player1,MyPlayer player2) {
            List<byte> player1Cards = protectedData.PlayerCards(player1);
            List<byte> player2Cards = protectedData.PlayerCards(player2);
            if (player1Cards.Sum(x => Convert.ToInt32(x)) > player2Cards.Sum(x => Convert.ToInt32(x))){
                return player2;
            }
            else {
                return player1;
            }

        }


        public byte SelectBiggestRankFromPlayersCardValues(MyPlayer player)
        {
            List<byte> playerCards = protectedData.PlayerCards(player);
            playerCards.Sort();
            return playerCards[playerCards.Count - 1];
        }

        public int SelectInRandomFromDeckOrDroppedCard() {
            System.Random rand = new System.Random();

            if (rand.NextDouble() >= 0.5)
                return 0;
            else
                return 1;
        }

       
    }
}
