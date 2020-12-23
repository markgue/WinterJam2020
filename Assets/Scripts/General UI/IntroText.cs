using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroText : MonoBehaviour
{
    public GameObject nextText;
    public string sceneAfterTheSequence;

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            if (nextText) {
                nextText.SetActive(true);
                gameObject.SetActive(false);
            }
            else {
                GameManager.instance.ToSpecificScene(sceneAfterTheSequence);
            }
        }
    }
}
