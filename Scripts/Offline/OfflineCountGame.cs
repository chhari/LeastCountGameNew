using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Unity;
using UnityEngine.UI;
using TMPro;

namespace QGAMES
{
    public class OfflineCountGame : MonoBehaviour
    {
        public TextMeshProUGUI MessageText;

        public TextMeshProUGUI DroporDrawDeckText;

        public TextMeshProUGUI ShoworDrawDroppedText;

        CardAnimator cardAnimator;

        public OfflineCountManager leastCountManager;

        public List<Transform> PlayerPositions = new List<Transform>();
        public List<Transform> BookPositions = new List<Transform>();

        MyPlayer localPlayer;
        MyPlayer remotePlayer;

        MyPlayer winner;

        MyPlayer currentTurnPlayer;
        MyPlayer currentTurnTargetPlayer;

        Card selectedCard;
        Ranks selectedRank;
        Ranks deckOrDroppedCard;

        public enum GameState
        {
            Idle,
            GameStarted,
            TurnStarted,
            TurnSelectingDroppingCard,
            TurnConfirmDroppingCard,
            TurnDrawingCard,
            TurnDrawingCardConfirmed,
            Show,
            GameFinished
        };



        public GameState gameState = GameState.Idle;

        private void Awake()
        {
            localPlayer = new MyPlayer();
            localPlayer.PlayerId = "offline-player";
            localPlayer.PlayerName = "Player";
            localPlayer.Position = PlayerPositions[0].position;
            //localPlayer.BookPosition = BookPositions[0].position;

            remotePlayer = new MyPlayer();
            remotePlayer.PlayerId = "offline-bot";
            remotePlayer.PlayerName = "Bot";
            remotePlayer.Position = PlayerPositions[1].position;
            //remotePlayer.BookPosition = BookPositions[1].position;
            remotePlayer.IsAI = true;

            cardAnimator = FindObjectOfType<CardAnimator>();
        }

        void Start()
        {
            gameState = GameState.GameStarted;
            GameFlow();
        }

        //****************** Game Flow *********************//
        public void GameFlow()
        {
            if (gameState > GameState.GameStarted)
            {
                CheckPlayersBooks();
                SetButtonsText();
                ShowAndHidePlayersDisplayingCards();

                if (leastCountManager.GameFinished())
                {
                    gameState = GameState.GameFinished;
                }
            }

            switch (gameState)
            {
                case GameState.Idle:
                    {
                        Debug.Log("IDEL");
                        break;
                    }
                case GameState.GameStarted:
                    {
                        Debug.Log("GameStarted");
                        OnGameStarted();
                        break;
                    }
                case GameState.TurnStarted:
                    {
                        Debug.Log("TurnStarted");
                        OnTurnStarted();
                        break;
                    }
                case GameState.TurnSelectingDroppingCard:
                    {
                        Debug.Log("TurnSelectingNumber");
                        OnTurnSelectingDroppingCard();
                        break;
                    }
                case GameState.TurnConfirmDroppingCard:
                    {
                        Debug.Log("TurnComfirmedSelectedNumber");
                        OnTurnConfirmDroppingCard();
                        break;
                    }
                case GameState.TurnDrawingCard:
                    {
                        Debug.Log("TurnWaitingForOpponentConfirmation");
                        OnTurnDrawingCard();
                        break;
                    }
                case GameState.TurnDrawingCardConfirmed:
                    {
                        Debug.Log("TurnOpponentConfirmed");
                        OnTurnDrawingCardConfirmed();
                        break;
                    }
                case GameState.Show:
                    {
                        Debug.Log("TurnGoFish");
                        OnShow();
                        break;
                    }
                case GameState.GameFinished:
                    {
                        Debug.Log("GameFinished");
                        OnGameFinished();
                        break;
                    }
            }
        }

        void OnGameStarted()
        {
            leastCountManager = new OfflineCountManager(localPlayer, remotePlayer);
            leastCountManager.Shuffle();
            leastCountManager.DealCardValuesToPlayer(localPlayer, Constants.PLAYER_INITIAL_CARDS);
            leastCountManager.DealCardValuesToPlayer(remotePlayer, Constants.PLAYER_INITIAL_CARDS);
            byte droppedCardValue = leastCountManager.FirstDroppedCard();

            cardAnimator.DealDisplayingCardsToLocalPlayer(localPlayer, Constants.PLAYER_INITIAL_CARDS);
            cardAnimator.DealDisplayingCards(remotePlayer, Constants.PLAYER_INITIAL_CARDS);
            Card addToDroppedCards = cardAnimator.DropFirstCard(droppedCardValue);
            leastCountManager.AddToDropCardsReference(addToDroppedCards);
            gameState = GameState.TurnStarted;
        }

        void OnTurnStarted()
        {
            SwitchTurn();
            gameState = GameState.TurnSelectingDroppingCard;
            GameFlow();
        }

        public void OnTurnSelectingDroppingCard()
        {
            ResetSelectedCard();

            if (currentTurnPlayer == localPlayer)
            {
                SetMessage($"Your turn. Pick a card from your hand.select a card");
            }
            else
            {
                SetMessage($"{currentTurnPlayer.PlayerName}'s turn");
            }

            if (currentTurnPlayer.IsAI)
            {
                byte biggestCard = leastCountManager.SelectBiggestRankFromPlayersCardValues(currentTurnPlayer);
                Card selectedCard = currentTurnPlayer.DropCardFromPlayer(cardAnimator, biggestCard, currentTurnPlayer.IsAI);
                leastCountManager.DropCardsFromPlayer(currentTurnPlayer, selectedCard);
                cardAnimator.DropCardAnimation(selectedCard, leastCountManager.GetDroppedCardsCount());
                //todo
                gameState = GameState.TurnConfirmDroppingCard;
                GameFlow();
                
            }
        }

        public void OnTurnConfirmDroppingCard()
        {
            if (currentTurnPlayer.IsAI)
            {
                SetMessage($" {currentTurnPlayer.PlayerName}dropped {selectedRank},click ok for bot to make next move");
            }
            else {
                SetMessage($" {currentTurnPlayer.PlayerName}dropped {selectedRank},click draw from deck or draw from dropped buttons");
            }

        }

        public void OnTurnDrawingCard()
        {
            
        }

        public void OnTurnDrawingCardConfirmed()
        {
           
            gameState = GameState.TurnStarted;
            GameFlow();

        }

        public void OnShow() {
            gameState = GameState.GameFinished;

        }


        public void OnGameFinished()
        {
            if (leastCountManager.Winner() == localPlayer)
            {
                SetMessage($"You WON!");
            }
            else
            {
                SetMessage($"You LOST!");
            }
        }

        //****************** Helper Methods *********************//
        public void ResetSelectedCard()
        {
            if (selectedCard != null)
            {
                selectedCard.OnSelected(false);
                selectedCard = null;
                selectedRank = 0;
            }
        }

        void SetMessage(string message)
        {
            MessageText.text = message;
        }

        public void SwitchTurn()
        {
            if (currentTurnPlayer == null)
            {
                currentTurnPlayer = localPlayer;
                currentTurnTargetPlayer = remotePlayer;
                return;
            }

            if (currentTurnPlayer == localPlayer)
            {
                currentTurnPlayer = remotePlayer;
                currentTurnTargetPlayer = localPlayer;
            }///
            else
            {
                currentTurnPlayer = localPlayer;
                currentTurnTargetPlayer = remotePlayer;
            }
        }

        public void CheckPlayersBooks()
        {
            List<byte> playerCardValues = leastCountManager.PlayerCards(localPlayer);
            localPlayer.SetCardValues(playerCardValues);
            
            playerCardValues = leastCountManager.PlayerCards(remotePlayer);
            remotePlayer.SetCardValues(playerCardValues);
        }

        public void SetButtonsText() {
            if (gameState < GameState.TurnConfirmDroppingCard) {
                ShoworDrawDroppedText.text = "Show";
                DroporDrawDeckText.text = "Drop Card";
            }else
            {
                ShoworDrawDroppedText.text = "Draw Dropped Cards";
                DroporDrawDeckText.text = "Draw from Deck";
            }

        }
        
        public void ShowAndHidePlayersDisplayingCards()
        {
            localPlayer.ShowCardValues();
            remotePlayer.ShowCardValues();
        }

        //****************** User Interaction *********************//
        public void OnCardSelected(Card card)
        {
            if (gameState == GameState.TurnSelectingDroppingCard)
            {
                if (card.OwnerId == currentTurnPlayer.PlayerId)
                {
                    if (selectedCard != null)
                    {
                        selectedCard.OnSelected(false);
                        selectedRank = 0;
                    }
                    
                    selectedCard = card;
                    selectedRank = selectedCard.Rank;
                    selectedCard.OnSelected(true);
                    SetMessage($"{currentTurnPlayer.PlayerName} ,do you want to drop  {selectedCard.Rank} ?");
                }
            }
        }

        public void DroporDrawDeckButton() {
            if (selectedCard != null)
            {
                leastCountManager.DropCardsFromPlayer(currentTurnPlayer, selectedCard);
                cardAnimator.DropCardAnimation(selectedCard, leastCountManager.GetDroppedCardsCount());
                currentTurnPlayer.DropCardFromPlayer(cardAnimator, selectedCard.GetValue(), !currentTurnPlayer.IsAI);
                leastCountManager.RepositionDroppedCards(cardAnimator);
                gameState = GameState.TurnConfirmDroppingCard;
                GameFlow();
            }
            if (gameState == GameState.TurnConfirmDroppingCard)
            {
                byte cardValue = leastCountManager.DrawCardValue();

                if (cardValue == Constants.POOL_IS_EMPTY)
                {
                    Debug.LogError("Pool is empty");
                    return;
                }

                cardAnimator.DrawDisplayingCard(currentTurnPlayer, cardValue);
                leastCountManager.AddCardValueToPlayer(currentTurnPlayer, cardValue);

                gameState = GameState.TurnDrawingCardConfirmed;
                GameFlow();
            }
        }

        public void ShoworDrawDroppedButton()
        {
            if (gameState == GameState.TurnSelectingDroppingCard) {
                winner = leastCountManager.getWinner(currentTurnPlayer, currentTurnTargetPlayer);
                SetMessage($" {winner.PlayerName} Won the game ");
                gameState = GameState.Show;
                GameFlow();
            }
            if (gameState == GameState.TurnConfirmDroppingCard)
            {
                byte cardValue = leastCountManager.DrawCardValue();

                if (cardValue == Constants.POOL_IS_EMPTY)
                {
                    Debug.LogError("Pool is empty");
                    return;
                }

                cardAnimator.DrawDisplayingCard(currentTurnPlayer, cardValue);
                leastCountManager.AddCardValueToPlayer(currentTurnPlayer, cardValue);

                gameState = GameState.TurnDrawingCardConfirmed;
                GameFlow();
            }

        }

        public void OnOkSelected()
        {
            if (gameState == GameState.TurnConfirmDroppingCard && currentTurnPlayer.IsAI)
            {
                if (currentTurnPlayer.IsAI)
                {
                    int res = leastCountManager.SelectInRandomFromDeckOrDroppedCard();
                    if (res.Equals(0))
                    {
                        OnDrawFromDeckButton();
                    }
                    else
                    {
                        OnDrawFromLastDroppedButton();
                    }
                }
            }
        }

        public void OnShowButton() {
            winner = leastCountManager.getWinner(currentTurnPlayer, currentTurnTargetPlayer);
            SetMessage($" {winner.PlayerName} Won the game ");
            gameState = GameState.Show;
            GameFlow();

        }

        public void OnDrawFromDeckButton() {
            if (gameState == GameState.TurnConfirmDroppingCard)
            {
                byte cardValue = leastCountManager.DrawCardValue();

                if (cardValue == Constants.POOL_IS_EMPTY)
                {
                    Debug.LogError("Pool is empty");
                    return;
                }

                cardAnimator.DrawDisplayingCard(currentTurnPlayer, cardValue);
                leastCountManager.AddCardValueToPlayer(currentTurnPlayer, cardValue);

                gameState = GameState.TurnDrawingCardConfirmed;
                GameFlow();
            }
            else {
                SetMessage("Drop the card and click on confirm card button first");
            }
        }

        public void OnDrawFromLastDroppedButton() {
            if (gameState == GameState.TurnConfirmDroppingCard)
            {
                Card card = leastCountManager.DrawDroppedCard();
                leastCountManager.AddCardValueToPlayer(currentTurnPlayer, card.GetValue());
                cardAnimator.DrawDroppedCard(currentTurnPlayer, card);
                leastCountManager.RepositionDroppedCards(cardAnimator);

                gameState = GameState.TurnDrawingCardConfirmed;
                GameFlow();
            }
            else {
                SetMessage("Drop the card and click on confirm card button first");
            }

        }

        //
        public void ConfirmDropButton() {

            if (selectedCard != null)
            {
                leastCountManager.DropCardsFromPlayer(currentTurnPlayer, selectedCard);
                cardAnimator.DropCardAnimation(selectedCard,leastCountManager.GetDroppedCardsCount());
                currentTurnPlayer.DropCardFromPlayer(cardAnimator, selectedCard.GetValue(), !currentTurnPlayer.IsAI);
                leastCountManager.RepositionDroppedCards(cardAnimator);
                gameState = GameState.TurnConfirmDroppingCard;
                GameFlow();
            }
            else {
                SetMessage("Select a card from your deck and click confirm");
            }
        }

        //****************** Animator Event *********************//
        public void AllAnimationsFinished()
        {
            GameFlow();
        }
    }
}
