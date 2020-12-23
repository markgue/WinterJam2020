using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuGMAgent : MonoBehaviour
{
    public void SetMaxFails(int optIndex) {
        GameManager.instance.SetMaxFails(optIndex);
    }

    public void SetVolume(float newVolume) {
        GameManager.instance.SetVolume(newVolume);
    }

    public void SetName(string newName) {
        GameManager.instance.SetName(newName);
    }
}
