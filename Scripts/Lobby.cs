using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

namespace QGAMES
{
    public class Lobby : MonoBehaviour
    {
        // Start is called before the first frame update
        public TextMeshProUGUI player_name;

        void Awake()
        {
            if (!PlayerPrefs.HasKey(Constants.PLAYER_NAME))
            {
                SetPreferences();
            }
            else {
                string PlayerName = PlayerPrefs.GetString(Constants.PLAYER_NAME);
                player_name.text = PlayerName;
            }
            
        }

        
        public void onPlayWithFriendsButton() {            
            SceneManager.LoadScene(Loader.Scene.CreateRoomScreen.ToString());
        }

        // Update is called once per frame
        void Update()
        {

        }

        void SetPreferences() {
            PlayerPrefs.SetString(Constants.PLAYER_NAME, "HARI");            
            player_name.text = "HARI";

        }
    }
}
