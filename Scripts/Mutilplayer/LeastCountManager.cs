using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace QGAMES
{
    [Serializable]
    public class LeastCountManager
    {        
        public List<Card> DroppedCards = new List<Card>();

        [SerializeField]
        ProtectedData protectedData;


        public LeastCountManager(Dictionary<int,MyPlayer> players)
        {
            Dictionary<int, List<byte>> temp = new Dictionary<int, List<byte>>();
            foreach (int i in players.Keys) {
                temp.Add(i, new List<byte>());
            }
            protectedData = new ProtectedData(temp);
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

        public void SetPoolOfCards(List<byte> poolOfCards) {
            protectedData.SetPoolOfCards(poolOfCards);
        }


        public List<byte> DealCardValuesToPlayer(MyPlayer player, int numberOfCards)
        {
            List<byte> poolOfCards = protectedData.GetPoolOfCards();

            int numberOfCardsInThePool = poolOfCards.Count;
            int start = numberOfCardsInThePool - 1 - numberOfCards;

            List<byte> cardValues = poolOfCards.GetRange(start, numberOfCards);
            poolOfCards.RemoveRange(start, numberOfCards);
            
            protectedData.AddCardValuesToPlayer(player.PlayerId, cardValues);
            return cardValues;


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

        public void AddCardValuesToPlayer(string playerId, List<byte> cardValues)
        {
            protectedData.AddCardValuesToPlayer(playerId, cardValues);
        }

        public void AddCardValueToPlayer(string playerId, byte cardValue)
        {
            protectedData.AddCardValueToPlayer(playerId, cardValue);
        }

        public void AddCardsToDroppedCards(List<byte> cardValue)
        {
            protectedData.AddCardsToDroppedCards(cardValue);
        }

        public void AddCardToDroppedCards(byte cardValue)
        {
            protectedData.AddCardToDroppedCards(cardValue);
        }

        public List<byte> GetDroppedCardValues() {
            return  protectedData.GetDroppedCards();
           
        }

        public void RemoveCardValuesFromPlayer(MyPlayer player, List<byte> cardValuesToRemove)
        {
            protectedData.RemoveCardValuesFromPlayer(player, cardValuesToRemove);
        }

        public void RemoveCardValueFromPlayer(MyPlayer player, byte cardValueToRemove)
        {
            protectedData.RemoveCardValueFromPlayer(player, cardValueToRemove);
        }

        //public void AddBooksForPlayer(Player player, int numberOfNewBooks)
        //{
        //    protectedData.AddBooksForPlayer(player, numberOfNewBooks);
        //}

        public int Winner()
        {
            return protectedData.WinnerPlayerId();
            
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

        public List<byte> GetPoolOfCards()
        {
            return protectedData.GetPoolOfCards();
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
