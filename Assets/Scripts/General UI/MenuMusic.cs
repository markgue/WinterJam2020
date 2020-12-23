using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuMusic : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        gameObject.GetComponent<AudioSource>().volume = GameManager.instance.volume;
    }
}
