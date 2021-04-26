using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace QGAMES
{
    public class CardAnimation
    {
        public Card card;
        public Vector2 destination;
        public Quaternion rotation;
        public Vector3 scale;
        
        public CardAnimation(Card c, Vector2 pos)
        {
            card = c;
            destination = pos;
            rotation = Quaternion.identity;
        }

        public CardAnimation(Card c, Vector2 pos, Quaternion rot)
        {
            card = c;
            destination = pos;
            rotation = rot;
        }

        public CardAnimation(Card c, Vector2 pos, Quaternion rot,Vector3 scl)
        {
            card = c;
            destination = pos;
            rotation = rot;
            scale = scl;

        }

        public bool Play()
        {
            bool finished = false;

            if (Vector2.Distance(card.transform.position, destination) < Constants.CARD_SNAP_DISTANCE)
            {
                card.transform.position = destination;
                finished = true;
            }
            else
            {
                card.transform.position = Vector2.MoveTowards(card.transform.position, destination, Constants.CARD_MOVEMENT_SPEED * Time.deltaTime);
                card.transform.rotation = Quaternion.Lerp(card.transform.rotation, rotation, Constants.CARD_ROTATION_SPEED * Time.deltaTime);
                if (card.scaleUp) {
                    card.transform.localScale = new Vector3(0.65f,0.65f,0f);
                }
            }

            return finished;
        }
    }

    /// <summary>`
    /// Controls all card animations in the game
    /// </summary>
    public class CardAnimator : MonoBehaviour
    {
        public GameObject CardPrefab;

        public List<Card> DisplayingCards;

        public Queue<CardAnimation> cardAnimations;

        CardAnimation currentCardAnimation;

        Vector2 startPosition = new Vector2(-0.8f,1.5f);

        public Vector2 droppedCardPosition = new Vector2(-0.8f, 0.0f);

        public Vector3 droppedCardScale = new Vector3(0.35f,0.35f,0f);


        // invoked when all queued card animations have been played
        public UnityEvent OnAllAnimationsFinished = new UnityEvent();

        bool working = false;

        void Awake()
        {
            cardAnimations = new Queue<CardAnimation>();
            InitializeDeck();
        }

        void InitializeDeck()
        {
            DisplayingCards = new List<Card>();

            for (byte value = 0; value < 52; value++)
            {
                Vector2 newPosition = startPosition + Vector2.right * Constants.DECK_CARD_POSITION_OFFSET * value;
                GameObject newGameObject = Instantiate(CardPrefab, newPosition, Quaternion.identity);
                newGameObject.transform.parent = transform;
                Card card = newGameObject.GetComponent<Card>();
                card.SetDisplayingOrder(-1);
                card.transform.position = newPosition;
                DisplayingCards.Add(card);
            }
        }

        public void DealDisplayingCards(MyPlayer player, int numberOfCard)
        {
            int start = DisplayingCards.Count - 1;
            int finish = DisplayingCards.Count - 1 - numberOfCard;

            List<Card> cardsToRemoveFromDeck = new List<Card>();

            for (int i = start; i > finish; i--)
            {
                Card card = DisplayingCards[i];
                player.ReceiveDisplayingCard(card);
                cardsToRemoveFromDeck.Add(card);
                AddCardAnimation(card, player.NextCardPosition());
            }

            foreach (Card card in cardsToRemoveFromDeck)
            {
                DisplayingCards.Remove(card);
            }
        }

        public void DealDisplayingCardsToLocalPlayer(MyPlayer player, int numberOfCard)
        {
            int start = DisplayingCards.Count - 1;
            int finish = DisplayingCards.Count - 1 - numberOfCard;

            List<Card> cardsToRemoveFromDeck = new List<Card>();

            for (int i = start; i > finish; i--)
            {
                Card card = DisplayingCards[i];
                card.scaleUp = true;
                player.ReceiveDisplayingCard(card);
                cardsToRemoveFromDeck.Add(card);
                AddCardAnimation(card, player.NextCardPosition(),player.playerAngle,player.playerScale);
            }

            foreach (Card card in cardsToRemoveFromDeck)
            {
                DisplayingCards.Remove(card);
            }
        }

        public Card DropFirstCard(byte value) {
            int numberOfDisplayingCard = DisplayingCards.Count;
            if (numberOfDisplayingCard > 0)
            {
                Card card = DisplayingCards[numberOfDisplayingCard - 1];
                card.SetCardValue(value);
                DropCardAnimation(card,1);
                DisplayingCards.Remove(card);
                return card;
            }
            return null;
        }
        public void DropCardAnimation(Card card,int val) {
                card.SetFaceUp(true);
                Vector2 temp = droppedCardPosition + Vector2.right * Constants.PLAYER_CARD_POSITION_OFFSET * val;
                AddCardAnimation(card, temp);
        }
        public void DropCardAnimation(Card card,int val,MyPlayer player) {
                card.SetFaceUp(true);
                Vector2 temp = droppedCardPosition + Vector2.right * Constants.PLAYER_CARD_POSITION_OFFSET * val;                
                AddCardAnimation(card, temp, player.playerAngle,droppedCardScale);                
                
        }

        public void NextDroppedCardPosition() {

        }

        public void DrawDroppedCard(MyPlayer player , Card card)
        {
            player.ReceiveDisplayingCard(card);
            AddCardAnimation(card, player.NextCardPosition(),player.playerAngle,player.playerScale);
            
        }

        public void DrawDisplayingCard(MyPlayer player)
        {
            int numberOfDisplayingCard = DisplayingCards.Count;

            if (numberOfDisplayingCard > 0)
            {
                Card card = DisplayingCards[numberOfDisplayingCard - 1];
                player.ReceiveDisplayingCard(card);
                AddCardAnimation(card, player.NextCardPosition());

                DisplayingCards.Remove(card);
            }
        }

        public void DrawDisplayingCard(MyPlayer player, byte value)
        {
            int numberOfDisplayingCard = DisplayingCards.Count;

            if (numberOfDisplayingCard > 0)
            {
                Card card = DisplayingCards[numberOfDisplayingCard - 1];
                card.SetCardValue(value);
                card.SetFaceUp(true);
                player.ReceiveDisplayingCard(card);
                AddCardAnimation(card, player.NextCardPosition(),player.playerAngle,player.playerScale);

                DisplayingCards.Remove(card);
            }
        }

        public void AddCardAnimation(Card card, Vector2 position)
        {            
            CardAnimation ca = new CardAnimation(card, position);
            cardAnimations.Enqueue(ca);
            working = true;
        }

        public void AddCardAnimation(Card card, Vector2 position, Quaternion rotation)
        {
            CardAnimation ca = new CardAnimation(card, position, rotation);
            cardAnimations.Enqueue(ca);
            working = true;
        }

        public void AddCardAnimation(Card card, Vector2 position, Quaternion rot,Vector3 scale)
        {
            CardAnimation ca = new CardAnimation(card, position,rot, scale);
            cardAnimations.Enqueue(ca);
            working = true;
        }

        private void Update()
        {
            if (currentCardAnimation == null)
            {
                NextAnimation();
            }
            else
            {
                if (currentCardAnimation.Play())
                {
                    NextAnimation();
                }
            }
        }

        void NextAnimation()
        {
            currentCardAnimation = null;

            if (cardAnimations!= null && cardAnimations.Count > 0)
            {
                CardAnimation ca = cardAnimations.Dequeue();
                currentCardAnimation = ca;
            }
            else
            {
                if (working)
                {
                    working = false;
                    OnAllAnimationsFinished.Invoke();
                }
            }
        }
    }
}
