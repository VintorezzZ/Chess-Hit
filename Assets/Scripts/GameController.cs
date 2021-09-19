using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public static GameController instance;
    public bool playerTurn;
    AI_Controller ai_Controller;
    Player_Controller pl_Controller;
    public int y_coord = -5;

    public int level = 0;

    public bool gameOver;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    public void Start()
    {
        Load();
        ai_Controller = GameObject.Find("AI Controller(Script)").GetComponent<AI_Controller>();
        pl_Controller = GameObject.Find("PlayerController(Script)").GetComponent<Player_Controller>();
        playerTurn = true;
        gameOver = false;
    }

    public event Action player_Moved;
    public event Action AI_Moved;
    public event Action AI_Move;
    //public event Action AI_Pawn_Killed;
    //public event Action Player_Pawn_Killed;
    //public UnityEvent AI_Pawn_Killed;
    //public UnityEvent Player_Pawn_Killed;

    public void OnPlayer_Moved()
    {
        playerTurn = false;
        AI_Move?.Invoke();
    }

    public void OnAI_Moved()
    {
        playerTurn = true;
    }

    public void OnAI_Move()
    {
        ai_Controller.AI_Turn();
    }

    public void On_AI_Pawn_Killed()
    {
        if (ai_Controller.AI_pawns.Count == 0 && !gameOver)
        {
            gameOver = true;
            level++;

            Debug.Log("AI_Lose!");
            Save();
            Invoke("ReloadScene", 2);
        }
        
        
    }

    public void On_Player_Pawn_Killed()
    {
        if (pl_Controller.playerPawns.Count == 0 && !gameOver)
        {       
            gameOver = true;
            level = 0;
            Debug.Log("Player_Lose!");
            Save();
            Invoke("ReloadScene", 2);
        }
    }

    void Save()
    {
        string path = Application.dataPath + "/Saves.json";
        int currentLevel = level;
        Data data = new Data { level = currentLevel };
        string jsonString = JsonUtility.ToJson(data);
        File.WriteAllText(path, jsonString);
        Debug.Log("Saved! " + data.level);
    }

    public void Load()
    {
        if (File.Exists(Application.dataPath + "/Saves.json"))
        {
            string savedString = File.ReadAllText(Application.dataPath + "/Saves.json");
            Debug.Log("Loaded: " + savedString);

            Data data = JsonUtility.FromJson<Data>(savedString);

            level = data.level;

            Debug.Log("level: " + level);
        }
        else
        {
            Debug.Log("No save! Loading default level...");

            level = 1;
        }
    }  

    void ReloadScene()
    {
        SceneManager.LoadScene(0);
    }
}



 [Serializable] 
public class Data
{
    public int level;
}
    