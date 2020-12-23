using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuMusic : MonoBehaviour
{
    public static MenuMusic instance = null;
    
    private void Awake() {
        if (instance != null && instance != this) {
            Destroy(gameObject);
        }
        else {
            instance = this;
        }
        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
        gameObject.GetComponent<AudioSource>().volume = GameManager.instance.volume;

        if (GameManager.instance.isGameScene() || GameManager.instance.isFailScene()) {
            gameObject.GetComponent<AudioSource>().mute = true;
        }
        else {
            gameObject.GetComponent<AudioSource>().mute = false;
        }
    }
}
