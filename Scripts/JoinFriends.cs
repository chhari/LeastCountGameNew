using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace QGAMES
{
    public class JoinFriends : MonoBehaviourPunCallbacks
    {

        public TextMeshProUGUI RoomCode;
        public int roomCodeInt;
        public GameObject shareButton;
        public TextMeshProUGUI NotificationText;
        private Dictionary<int, GameObject> playerListEntries;

        public Image player1Im;
        public GameObject player2Im;
        public GameObject player3Im;
        public GameObject player4Im;
        public GameObject player5Im;
        public GameObject player6Im;


        private void Awake()
        {
            PhotonNetwork.AutomaticallySyncScene = true;
        }
        // Start is called before the first frame update
        void Start()
        {
            shareButton.SetActive(false);
            Connect();
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void Connect()
        {
            Debug.Log("came else ");
            PhotonNetwork.ConnectUsingSettings();
            PhotonNetwork.GameVersion = Constants.GAMEVERSION;
        }


        public override void OnConnectedToMaster()
        {
            Debug.Log("PUN Basics Tutorial/Launcher: OnConnectedToMaster() was called by PUN");
            string result = PlayerPrefs.GetString(Constants.CREATEORJOIN);
            Debug.Log(PhotonNetwork.CloudRegion);

            if (result == Constants.CREATE)
            {
                CreateRoom();
            }
            else if (result == Constants.JOIN)
            {
                string input = PlayerPrefs.GetString(Constants.ROOMCODE);
                Debug.Log(input);
                JoinRoom(input);
            }
            else if (result == Constants.JOINRANDOM) {
                OnJoinRandomRoomInput();
            }
        }

        public void CreateRoom()
        {
            if (PhotonNetwork.IsConnected)
            {
                roomCodeInt = Random.Range(20000, 50000);
                //NicknameInputField.text = r.ToString();
                Debug.Log("roomName" + roomCodeInt.ToString());
                PhotonNetwork.CreateRoom(roomCodeInt.ToString(), new RoomOptions { MaxPlayers = 6 });
            }
            else
            {
                Debug.Log("not connected");

            }
        }

        public override void OnJoinRandomFailed(short returnCode, string message)
        {
            CreateRoom();
        }

        public void OnJoinRandomRoomInput()
        {
            PhotonNetwork.JoinRandomRoom();
        }

        public void JoinRoom(string input)
        {

            ShowNotification("Joining Room "+ input);
            ExitGames.Client.Photon.Hashtable props = new ExitGames.Client.Photon.Hashtable
            {
                {Constants.PLAYER_AVATAR, PlayerPrefs.GetString(Constants.PLAYER_AVATAR)},
                {Constants.PLAYER_NAME,PlayerPrefs.GetString(Constants.PLAYER_NAME)}
            };
            PhotonNetwork.LocalPlayer.SetCustomProperties(props);
            PhotonNetwork.JoinRoom(input);
        }

        public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
        {
            //Destroy(playerListEntries[otherPlayer.ActorNumber].gameObject);
            //playerListEntries.Remove(otherPlayer.ActorNumber);
        }

        public override void OnLeftRoom()
        {
            //to clear local variables ,so that when in enters again we dont have to work
            //foreach (GameObject entry in playerListEntries.Values)
            //{
            //    Destroy(entry.gameObject);
            //}

            //playerListEntries.Clear();
            //playerListEntries = null;
        }

        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            object isPlayerName;
            object avatar;
            ShowNotification("Player Entered");
            if (newPlayer.CustomProperties.TryGetValue(Constants.PLAYER_NAME, out isPlayerName))
            {
                newPlayer.CustomProperties.TryGetValue(Constants.PLAYER_AVATAR, out avatar);
                ShowNotification("Player Entered" + avatar);
                ShowJoinedPlayer(newPlayer.ActorNumber, (string)avatar);

            }

        }

        public override void OnCreateRoomFailed(short returnCode, string message)
        {
            Debug.Log("Failed Creating  in");
        }

        public override void OnCreatedRoom()
        {
            if (PhotonNetwork.IsMasterClient)
            {
                Debug.Log("OnPlayerEnteredRoom");
                ShowNotification("Room Created");
                ShowCode();
                //ShowReadyOnePlayer();

            }
        }

        public void ShowCode() {
            RoomCode.text = roomCodeInt.ToString();
            shareButton.SetActive(true);
        }

        public void ShowNotification(string Text) {
            NotificationText.text = Text;
        }

        public void onClickShareButton() {
            if (shareButton.activeSelf)
            {
                new NativeShare().SetSubject(roomCodeInt.ToString()).SetText("Have fun playing least count").Share();
            }
        }

        public void onBackButtonClicked()
        { 

            PhotonNetwork.LeaveRoom();            
        }

        public override void OnJoinedRoom()
        {
            
            if (playerListEntries == null)
            {
                playerListEntries = new Dictionary<int, GameObject>();
            }


            foreach (Player p in PhotonNetwork.PlayerList)
            {
                object isPlayerName;
                object avatar;
                if (p.CustomProperties.TryGetValue(Constants.PLAYER_NAME, out isPlayerName))
                {
                    p.CustomProperties.TryGetValue(Constants.PLAYER_AVATAR, out avatar);
                    ShowJoinedPlayer(p.ActorNumber,(string) avatar);

                }
            }

        }

       
        void ShowJoinedPlayer(int pNo,string avatar) {
            string avName = "avatars/avatars0" + avatar;
            if (pNo == 1) {
                player1Im.GetComponent<Image>().sprite = Resources.Load<Sprite>(avName);
            }
            else if (pNo == 2)
            {
                player2Im.GetComponent<Image>().sprite = Resources.Load<Sprite>(avName);
            }
            else if (pNo == 3)
            {
                player3Im.GetComponent<Image>().sprite = Resources.Load<Sprite>(avName);
            }
            else if (pNo == 4)
            {
                player4Im.GetComponent<Image>().sprite = Resources.Load<Sprite>(avName);
            }
            else if (pNo == 5)
            {
                player5Im.GetComponent<Image>().sprite = Resources.Load<Sprite>(avName);
            }
            else if (pNo == 6)
            {
                player5Im.GetComponent<Image>().sprite = Resources.Load<Sprite>(avName);
            }

        }

        public void startGameButton() {

            if (PhotonNetwork.IsMasterClient)
            {
                PhotonNetwork.CurrentRoom.IsOpen = false;
                PhotonNetwork.CurrentRoom.IsVisible = false;

                PhotonNetwork.LoadLevel("MultiGameScreen");
            }
            else
            {
                NotificationText.text = "Only Master can start the game ";
            }            
        }


    }
}
