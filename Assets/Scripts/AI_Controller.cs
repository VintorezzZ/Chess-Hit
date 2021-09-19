using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AI_Controller : MonoBehaviour
{
    private GameObject player_controller;
    public List<GameObject> player_pawns;
    public List<GameObject> AI_pawns;
    public int random_AI_Pawn;
    public int random_Player_Pawn;
    public int force = 30;


    GameController gameController;

    public UnityEvent AI_Pawn_Killed;

    private void Start()
    {
        GameController.instance.AI_Move += AI_Turn;

        player_controller = GameObject.Find("PlayerController(Script)");
        player_pawns = player_controller.GetComponent<Player_Controller>().playerPawns;

        gameController = GameObject.Find("GameController(Script)").GetComponent<GameController>();        
    }


    public void Update()
    {
        CheckPawnToDestroy();
    }



    public void AI_Turn()
    {
        int timeToWait = UnityEngine.Random.Range(2, 4);
        StartCoroutine(WaitAndGo(timeToWait));
    }

    IEnumerator WaitAndGo(int timeToWait)
    {
        yield return new WaitForSeconds(timeToWait);

        Debug.Log("AI Turn!");

        random_AI_Pawn = UnityEngine.Random.Range(0, AI_pawns.Count - 1);
        random_Player_Pawn = UnityEngine.Random.Range(0, player_pawns.Count - 1);

        Vector3 AI_pos = AI_pawns[random_AI_Pawn].GetComponent<Transform>().position;
        Vector3 Pl_pos = player_pawns[random_Player_Pawn].GetComponent<Transform>().position;

        Vector3 moveDir = (Pl_pos - AI_pos).normalized * force;

        AI_pawns[random_AI_Pawn].GetComponent<Rigidbody>().AddForce(moveDir, ForceMode.VelocityChange);

        GameController.instance.OnAI_Moved();
    }

    private void CheckPawnToDestroy()
    {
        for (int i = 0; i < AI_pawns.Count; i++)
        {
            if (AI_pawns[i].transform.position.y < gameController.y_coord)
            {
                GameObject pawnToDestroy = AI_pawns[i];
                AI_pawns.Remove(AI_pawns[i]);
                Destroy(pawnToDestroy);
                AI_Pawn_Killed.Invoke();

            }            
        }

        //if (AI_pawns.Count == 0)
        //{
        //    AI_Pawn_Killed.Invoke();
        //    Debug.Log("AI_Lose!");
        //}
    }   
}
