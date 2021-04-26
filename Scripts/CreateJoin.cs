using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace QGAMES {

    public class CreateJoin : MonoBehaviour
    {
        // Start is called before the first frame update
        public GameObject CanvasBackground;
        public GameObject CanvasForeground;
        public GameObject JoinRoom;
        public TMP_InputField JoinRoomInputFeild;


        void Start()
        {
            JoinRoom.SetActive(false);
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void onJoinUpButtonClicked() {
            if (CanvasForeground.activeSelf)
            {
                CanvasForeground.SetActive(false);
            }
            if (!JoinRoom.activeSelf)
            {
                JoinRoom.SetActive(true);
            }
        }

        bool IsAllDigits(string s)
        {
            foreach (char c in s)
            {
                if (!char.IsDigit(c))
                    return false;
            }
            return true;
        }

        public void onCreateUpButtonClicked()
        {
            if (JoinRoom.activeSelf)
            {
                JoinRoom.SetActive(false);
            }
            if (!CanvasForeground.activeSelf)
            {
                CanvasForeground.SetActive(true);
            }
        }

        public void onClickCreateRoom() {
            PlayerPrefs.SetString(Constants.CREATEORJOIN,Constants.CREATE);
            SceneManager.LoadScene(Loader.Scene.JoinFriendsScreen.ToString());
        }

        public void onClickJoinRoom() {
            string joinCode = JoinRoomInputFeild.text;
            if (IsAllDigits(joinCode))
            {
                PlayerPrefs.SetString(Constants.CREATEORJOIN, Constants.JOIN);
                PlayerPrefs.SetString(Constants.ROOMCODE,joinCode);
                SceneManager.LoadScene(Loader.Scene.JoinFriendsScreen.ToString());
            }
            else {
                JoinRoomInputFeild.text = "Enter a valid code";
            }
        }

    }
}
