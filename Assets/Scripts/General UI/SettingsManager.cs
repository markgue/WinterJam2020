using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsManager : MonoBehaviour
{
    // singleton /////////////////////////////////////
    public static SettingsManager instance = null;


    // setting parameters ////////////////////////////
    public float volume;


    // system messages ///////////////////////////////
    private void Awake() {
        if (instance == null) {
            instance = this;
        }
        else if (instance != this) {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }
}
