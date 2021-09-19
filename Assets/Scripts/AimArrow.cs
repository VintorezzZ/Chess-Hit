using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimArrow : MonoBehaviour
{
    public Player_Controller pl_Controller;
    public GameObject aimArrow;
    public float x = 0.8f;
    public float y = 1;
    public float multiplier = 3f;

    void Start()
    {
        pl_Controller = GameObject.Find("PlayerController(Script)").GetComponent<Player_Controller>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ArrowAim()
    {
        Vector3 launchVector = pl_Controller.launchVector;
        Vector3 normLauchVector =  launchVector.normalized;

        aimArrow.transform.forward = normLauchVector;

        Vector3 lv = new Vector3(x, y, launchVector.magnitude * multiplier);

        aimArrow.transform.localScale = lv;

        aimArrow.transform.position = pl_Controller.selectedPawn.transform.position;

    }
}
