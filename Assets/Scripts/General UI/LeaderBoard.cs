using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LeaderBoard : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI myText;
    
    // Start is called before the first frame update
    void Start()
    {
        myText.text = "";
        for (int i = 0; i < GameManager.instance.board.names.Length; i ++) {
            myText.text += GameManager.instance.board.names[i] + ": " + GameManager.instance.board.scores[i] + "\n";
        }
    }
}
