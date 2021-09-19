using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AI_Controller : MonoBehaviour
{
    public static AI_Controller instance;

    private GameObject player_controller;
    public List<GameObject> player_pawns;
    public List<GameObject> AI_pawns;
    public int random_AI_Pawn;
    public int random_Player_Pawn;
    public int force = 30;
    public GameObject sphere;

    GameController gameController;

    public UnityEvent AI_Pawn_Killed;

    [SerializeField] private ParticleSystem explosion;



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
        int timeToWait = UnityEngine.Random.Range(1, 3);        
        
        StartCoroutine(WaitAndGo(timeToWait));
    }

    IEnumerator WaitAndGo(int timeToWait)
    {

        //Debug.Log("AI Turn!");
        yield return new WaitForSeconds(1);

        random_AI_Pawn = UnityEngine.Random.Range(0, AI_pawns.Count - 1);
        random_Player_Pawn = UnityEngine.Random.Range(0, player_pawns.Count - 1);

        Vector3 AI_pos = AI_pawns[random_AI_Pawn].GetComponent<Transform>().position;
        Vector3 Pl_pos = player_pawns[random_Player_Pawn].GetComponent<Transform>().position;

        Vector3 moveDir = (Pl_pos - AI_pos).normalized * force;

        sphere.transform.position = AI_pos;
        sphere.SetActive(true);

        RotatePawnToVector(AI_pawns[random_AI_Pawn], moveDir);

        yield return new WaitForSeconds(timeToWait);

        AI_pawns[random_AI_Pawn].GetComponent<Rigidbody>().AddForce(moveDir, ForceMode.VelocityChange);

        sphere.SetActive(false);

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

                PlayExplosionEffect(pawnToDestroy);

                Destroy(pawnToDestroy);
                AI_Pawn_Killed.Invoke();

            }            
        }
    }

    void RotatePawnToVector(GameObject selectedPawn, Vector3 launchVector)
    {
        selectedPawn.transform.rotation = Quaternion.LookRotation(launchVector);
    }

    void PlayExplosionEffect(GameObject obj)
    {
        ParticleSystem newExplosion = explosion;
        //Color objColor = obj.GetComponentInChildren<Material>().color;
        newExplosion.GetComponent<Renderer>().sharedMaterial.color = Color.red;
        var expeffect = Instantiate(newExplosion, obj.transform.position, obj.transform.rotation);
        //Destroy(expeffect, 3);
    }
}
