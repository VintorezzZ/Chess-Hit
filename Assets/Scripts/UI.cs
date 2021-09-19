using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    public static UI instance;

    public Text level;


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



    void Start()
    {
        level.text = "Level: " + GameController.instance.level;
    }


    void Update()
    {
        
    }


    public void OnChangeLevel()
    {
        level.text = "Level: " + GameController.instance.level;
    }

}
