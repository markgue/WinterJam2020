using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SummaryText : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        string textToShow = GameManager.instance.completionCount.ToString()
            + " out of 1.9 billion children in the world had a good Christmas because of you, " + GameManager.instance.playerName + ".";
        gameObject.GetComponent<TextMeshProUGUI>().text = textToShow;
    }
}
