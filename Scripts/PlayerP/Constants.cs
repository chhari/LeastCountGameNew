using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.ComponentModel;

namespace QGAMES
{
    public static class Constants
    {
        public const float PLAYER_CARD_POSITION_OFFSET = 0.4f;
        public const float PLAYER_BOOK_POSITION_OFFSET = 2f;
        public const float DECK_CARD_POSITION_OFFSET = 0.03f;
        public const string CARD_BACK_SPRITE = "cardBack_red5";
        public const float CARD_SELECTED_OFFSET = 0.3f;
        public const int PLAYER_INITIAL_CARDS = 5;
        public const float CARD_MOVEMENT_SPEED = 5.0f;
        public const float CARD_SNAP_DISTANCE = 0.01f;
        public const float CARD_ROTATION_SPEED = 8f;
        public const float BOOK_MAX_RANDOM_ROTATION = 15f;
        public const byte POOL_IS_EMPTY = 255;
        public const string PLAYER_READY = "IsPlayerReady";
        public const string INITIALIZING_CARDS = "IntializingPlayerCards";
        public const string INITIALIZING_DROPPEDCARD = "IntializingDroppedCards";
        public const string GAME_STATE_CHANGED = "GameStateChanged";
        public const string PLAYER_MOVE = "PlayerMove";
        public const string PLAYER_TURN = "PlayerIdOfTurn";
        public const string PLAYER_AVATAR = "PLAYER_AVATAR"; 

        public const string PLAYER_NAME = "PLAYER_NAME";
        public const string CREATEORJOIN = "CREATE_OR_JOIN";
        public const string CREATE = "CREATE";
        public const string JOIN = "JOIN";
        public const string JOINRANDOM = "JOINRANDOM";
        public const string ROOMCODE = "CODE";
        public const string GAMEVERSION = "1";

        public const string DROP = "Drop Card";
        public const string SHOW = "Show";
        public const string DRAWDROPPEDCARDS = "Draw Dropped Cards";
        public const string DRAWFROMDECK = "Draw from Deck";
        public const byte SHUFFLE_EVCODE  = 1;
        public const byte DROP_EVCODE  = 3;
        public const byte DRAW_EVCODE  = 2;
    }

    public enum Suits
    {
        NoSuits = -1,
        Spades = 0,
        Clubs = 1,
        Diamonds = 2,
        Hearts = 3,
    }

    public enum Ranks
    {
        [Description("No Ranks")]
        NoRanks = -1,
        [Description("A")]
        Ace = 1,
        [Description("2")]
        Two = 2,
        [Description("3")]
        Three = 3,
        [Description("4")]
        Four = 4,
        [Description("5")]
        Five = 5,
        [Description("6")]
        Six = 6,
        [Description("7")]
        Seven = 7,
        [Description("8")]
        Eight = 8,
        [Description("9")]
        Nine = 9,
        [Description("10")]
        Ten = 10,
        [Description("J")]
        Jack = 11,
        [Description("Q")]
        Queen = 12,
        [Description("K")]
        King = 13,
    }
}
