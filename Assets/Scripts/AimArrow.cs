using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimArrow : MonoBehaviour
{
    public Player_Controller pl_Controller;
    public GameController gameController;
    public GameObject aimArrow;
    //public GameObject sphere;
    public float x = 0.8f;
    public float y = 1;
    public float multiplier = 3f;

    void Start()
    {
        pl_Controller = GameObject.Find("PlayerController(Script)").GetComponent<Player_Controller>();
        gameController = GameObject.Find("GameController(Script)").GetComponent<GameController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ArrowAim(Vector3 vector, GameObject targetPosition)
    {

        Vector3 launchVector = vector;
        Vector3 normLaunchVector = launchVector.normalized;

        aimArrow.transform.forward = normLaunchVector;

        Vector3 lv = new Vector3(x, y, launchVector.magnitude * multiplier);

        aimArrow.transform.localScale = lv;

        aimArrow.transform.position = targetPosition.transform.position;

        //sphere.transform.position = targetPosition.transform.position;


        //Vector3 launchVector = pl_Controller.launchVector;
        //Vector3 normLaunchVector =  launchVector.normalized;

        //aimArrow.transform.forward = normLaunchVector;

        //Vector3 lv = new Vector3(x, y, launchVector.magnitude * multiplier);

        //aimArrow.transform.localScale = lv;

        //aimArrow.transform.position = pl_Controller.selectedPawn.transform.position;

    }
}
