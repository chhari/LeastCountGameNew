using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

namespace QGAMES {
    public class Avatars : MonoBehaviour
    {
       
        // Start is called before the first frame update
        void Start()
        {
            string s;
            if (PlayerPrefs.HasKey(Constants.PLAYER_AVATAR))
            {
                s = PlayerPrefs.GetString(Constants.PLAYER_AVATAR);
            }
            else {
                int avatarNo = Random.Range(1, 10);
                Debug.Log(avatarNo.ToString());
                PlayerPrefs.SetString(Constants.PLAYER_AVATAR, avatarNo.ToString());
                s = PlayerPrefs.GetString(Constants.PLAYER_AVATAR); 
            }
                        
            string avName = "avatars/avatars0" + s;

            Debug.Log(avName);           
            GetComponent<Image>().sprite = Resources.Load<Sprite>(avName);
            
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
