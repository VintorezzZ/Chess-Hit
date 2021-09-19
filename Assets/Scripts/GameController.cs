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

    public List<GameObject> allGrounds;

    public int newLevel = 0;

    private void Awake()
    {
        if (!instance)
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
        Debug.Log(newLevel);
        Debug.Log(level);
        changeLevel.Invoke();
        ChangeGround();
        ai_Controller = GameObject.Find("AI Controller(Script)").GetComponent<AI_Controller>();
        pl_Controller = GameObject.Find("PlayerController(Script)").GetComponent<Player_Controller>();
        playerTurn = true;
        gameOver = false;
    }

    public event Action AI_Move;
    public UnityEvent changeLevel;


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

            //Debug.Log("AI_Lose!");
            Invoke("ReloadScene", 2);
            level++;
            //changeLevel.Invoke();
            //ChangeGround();
            Save();
        }       
    }

    public void On_Player_Pawn_Killed()
    {
        if (pl_Controller.playerPawns.Count == 0 && !gameOver)
        {       
            gameOver = true;
            //Debug.Log("Player_Lose!");
            Invoke("ReloadScene", 2);
        }
    }

    #region json save/load
    //void Save()
    //{
    //    string path = Application.dataPath + "/Saves.json";
    //    int _Level = level;
    //    int _newLevel = newLevel;
    //    Data data = new Data { level = _Level, newLevel = _newLevel };
    //    string jsonString = JsonUtility.ToJson(data);
    //    File.WriteAllText(path, jsonString);
    //    Debug.Log("Saved! " + data.level + " newLevel: " + data.newLevel);
    //}

    //public void Load()
    //{
    //    if (File.Exists(Application.dataPath + "/Saves.json"))
    //    {
    //        string savedString = File.ReadAllText(Application.dataPath + "/Saves.json");
    //        Debug.Log("Loaded: " + savedString);

    //        Data data = JsonUtility.FromJson<Data>(savedString);

    //        level = data.level;
    //        newLevel = data.newLevel;

    //        Debug.Log("level: " + level + " newLevel: " + newLevel);
    //    }
    //    else
    //    {
    //        Debug.Log("No save! Loading default level...");

    //        level = 0;
    //        newLevel = 0;
    //    }
    //}

    #endregion


    #region PlayerPrefs save/load
    public void Save()
    {
        PlayerPrefs.SetInt("CurrentLevel", level);
        PlayerPrefs.SetInt("NewLevel", newLevel);
    }

    public void Load()
    {
        level = PlayerPrefs.GetInt("CurrentLevel");
        newLevel = PlayerPrefs.GetInt("NewLevel");
    }
    #endregion

    void ReloadScene()
    {
        SceneManager.LoadScene(0);
    }

    void ChangeGround()
    {
        if (newLevel == 0)
        {
            allGrounds[newLevel].SetActive(true);
        }
        else
        {
            allGrounds[newLevel - 1].SetActive(false);
            allGrounds[newLevel].SetActive(true);     
        }
        newLevel++;
        if (newLevel > allGrounds.Count - 1)
        {
            newLevel = 0;
        }
    }


}



// [Serializable] 
//public class Data
//{
//    public int level;
//    public int newLevel;
//}
    