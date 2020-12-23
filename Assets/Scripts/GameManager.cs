using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public GameObject playerObject;

    // scene management
    [SerializeField]
    private string startMenu, gameScene, failScene, creditScene;
    private string currentScene;

    // system settings 
    public float volume { get; set; } // 0 - 1
    [SerializeField]
    private int[] difficultySettings;

    // workorder system
    public int maxChanceOfFail;
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

        DontDestroyOnLoad(gameObject);
    }

    private void Update() {
        if (chancesOfFail <= 0 && currentScene == gameScene) {
            // TODO: game fails
            ToFailScene();
        }
    }


    // work order system ////////////////////////////////////////////
    public void OrderOverdue() {
        chancesOfFail--;
        failCount++;
    }

    public void OrderComplete() {
        completionCount++;
    }


    // system settings ///////////////////////////////////////////////
    public void SetMaxFails(int optIndex) {
        maxChanceOfFail = difficultySettings[optIndex];
    }


    // scene loading /////////////////////////////////////////////////
    public void ToMenuScene() {
        currentScene = startMenu;
        SceneManager.LoadScene(currentScene);
    }

    public void ToGameScene() {
        chancesOfFail = maxChanceOfFail;
        currentScene = gameScene;
        SceneManager.LoadScene(currentScene);
    }

    public void ToFailScene() {
        currentScene = failScene;
        SceneManager.LoadScene(currentScene);
    }

    public void ToCreditScene() {
        currentScene = creditScene;
        SceneManager.LoadScene(currentScene);
    }
}
