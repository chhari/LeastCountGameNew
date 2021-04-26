using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoaderCallback : MonoBehaviour
{

    private static int count = 0;

    private void Update(){

        count++;
        
        if (count == 40)
        {
            
            Loader.LoaderCallback();
        }
    }

}
