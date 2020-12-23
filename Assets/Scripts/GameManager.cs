using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public GameObject playerObject;

    // workorder system
    public int chancesOfFail;
    private int completionCount = 0;
    private int failCount = 0;
    
    // system messages /////////////////////////////////////////////////
    void Awake() {
        if (instance != null && instance != this) {
            Destroy(gameObject);
        }
        else {
            instance = this;
        }
    }

    private void Update() {
        if (chancesOfFail <= 0) {
            // TODO: game fails
        }
    }


    // other manager messages //////////////////////////////////////////
    public void OrderOverdue() {
        chancesOfFail--;
        failCount++;
    }

    public void OrderComplete() {
        completionCount++;
    }
}
