using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMusic : MonoBehaviour
{
    [SerializeField]
    private int ordersToCompleteBeforeMusic;
    [SerializeField]
    GameObject musicSource;
    
    // Update is called once per frame
    void Update()
    {
        musicSource.GetComponent<AudioSource>().volume = GameManager.instance.volume;

        if (GameManager.instance.completionCount >= ordersToCompleteBeforeMusic) {
            musicSource.SetActive(true);
        }
        else {
            musicSource.SetActive(false);
        }
    }
}
