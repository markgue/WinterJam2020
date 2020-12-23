using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public class GameManager : MonoBehaviour {
    public static GameManager instance;

    public GameObject playerObject;

    // leaderboard

    public struct InternalBoard {
        public string[] names;
        public int[] scores;
    }

    // scene management
    [SerializeField]
    private string startMenu, gameScene, failScene, creditScene;
    private string currentScene;

    // system settings 
    [SerializeField]
    public float volume; // 0 - 1
    [SerializeField]
    private int[] difficultySettings;
    [SerializeField]
    public string playerName = "Nameless Elf";
    public InternalBoard board;

    // workorder system: progress related please reset
    public int maxChanceOfFail;
    public int chancesOfFail;
    public int completionCount = 0;
    public int failCount = 0;

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

    private void Start() {
        // load the leaderboard
        string jsonSource;
        try {
            jsonSource = System.IO.File.ReadAllText(Application.dataPath + "//leaderboard.json");
            board = JsonUtility.FromJson<InternalBoard>(jsonSource);
            Debug.Log("read");
        }
        catch {
            board = new InternalBoard();
            board.names = new string[1] { playerName };
            board.scores = new int[1] { 0 };
            Debug.Log("no file");
        }
        Debug.Log(board);
    }

    private void Update() {
        if (chancesOfFail <= 0 && currentScene == gameScene) {
            // game fails
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

    public void SetVolume(float newVolume) {
        volume = newVolume;
    }

    public void SetName(string newName) {
        playerName = newName;
    }


    // scene loading /////////////////////////////////////////////////
    public bool isGameScene() {
        return currentScene == gameScene;
    }

    public bool isFailScene() {
        return currentScene == failScene;
    }

    public void ToMenuScene() {
        currentScene = startMenu;
        SceneManager.LoadScene(currentScene);
    }

    public void ToGameScene() {
        // first rest a couple of parameters
        chancesOfFail = maxChanceOfFail;
        completionCount = 0;
        failCount = 0;

        // load the leaderboard
        string jsonSource;
        try {
            jsonSource = System.IO.File.ReadAllText(Application.dataPath + "//leaderboard.json");
            board = JsonUtility.FromJson<InternalBoard>(jsonSource);
            Debug.Log("read");
        }
        catch {
            board = new InternalBoard();
            board.names = new string[1] { playerName };
            board.scores = new int[1] { 0 };
            Debug.Log("no file");
        }
        Debug.Log(board);

        currentScene = gameScene;
        SceneManager.LoadScene(currentScene);
    }

    public void ToFailScene() {
        // update the leaderboard
        bool touched = false;
        for (int i = 0; i < board.names.Length; i ++) {
            if (board.names[i] == playerName) {
                touched = true;
                board.scores[i] = Mathf.Max(completionCount, board.scores[i]);
            }
        }
        if (touched == false) {
            List<string> newNames = new List<string>();
            List<int> newScores = new List<int>();
            for (int i = 0; i < board.names.Length; i++) {
                newNames.Add(board.names[i]);
                newScores.Add(board.scores[i]);
            }

            newNames.Add(playerName);
            newScores.Add(completionCount);

            board.names = newNames.ToArray();
            board.scores = newScores.ToArray();
        }
        // output the leaderboard
        System.IO.File.WriteAllText(Application.dataPath + "//leaderboard.json", JsonUtility.ToJson(board));
        Debug.Log(JsonUtility.ToJson(board));

        currentScene = failScene;
        SceneManager.LoadScene(currentScene);
    }

    public void ToCreditScene() {
        currentScene = creditScene;
        SceneManager.LoadScene(currentScene);
    }

    public void ToSpecificScene(string sceneName) {
        if (sceneName == startMenu) {
            ToMenuScene();
        }
        else if (sceneName == gameScene) {
            ToGameScene();
        }
        else if (sceneName == failScene) {
            ToFailScene();
        }
        else if (sceneName == creditScene) {
            ToCreditScene();
        }
    }
}
