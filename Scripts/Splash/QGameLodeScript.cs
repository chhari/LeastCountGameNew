using UnityEngine;
using System.Collections;

public class QGameLodeScript : MonoBehaviour {

    // Use this for initializati on

    void Awake() {
        
    }

	void Start () {
        Loader.Load(Loader.Scene.QGamesLogoScreen);
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
