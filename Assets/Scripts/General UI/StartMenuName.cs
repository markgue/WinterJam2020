using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StartMenuName : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        gameObject.GetComponent<TextMeshProUGUI>().text = "You are ... " + GameManager.instance.playerName;
    }
}
