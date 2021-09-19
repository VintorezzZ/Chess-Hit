using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Player_Controller : MonoBehaviour
{
    public new Camera camera;
    public GameController gameController;
    public AimArrow aimArrow;
    public LayerMask layerMask;
    public List<GameObject> playerPawns;
    public GameObject selectedPawn;
    public Vector3 mouseClickPosition;
    public float multiplier = 5;
    public float launchForce = 35;
    public Vector3 launchVector;

    public UnityEvent Player_Pawn_Killed;

    void Start()
    {
        gameController = GameObject.Find("GameController(Script)").GetComponent<GameController>();
        aimArrow = GameObject.Find("AimArrow" ).GetComponent<AimArrow>();
    }

    void Update()
    {
        CheckPawnToDestroy();

        if (gameController.playerTurn == false)
            return;

        if (Input.GetMouseButton(0))
        {
            if (selectedPawn == null)
                Select_Pawn();                

            SetVector();
            aimArrow.ArrowAim();
        }           

        if (Input.GetMouseButtonUp(0))
        {
            LaunchPawn();
            GameController.instance.OnPlayer_Moved();            
        }
        
    }


    void Select_Pawn()
    {
        RaycastHit hit;
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, 50000, layerMask))
        {            
            GameObject obj = hit.transform.gameObject;
            if (playerPawns.Contains(obj))
            {
                selectedPawn = obj;
                mouseClickPosition = Input.mousePosition;
                aimArrow.aimArrow.SetActive(true);
            }
        }
    }

    void SetVector()
    {
        float x = mouseClickPosition.x - Input.mousePosition.x;
        float y = mouseClickPosition.y - Input.mousePosition.y;
        float correctX = x / Screen.width;
        float correctZ = y / Screen.height;
        Vector3 correctVector = new Vector3(correctX, 0, correctZ);
        launchVector = Vector3.ClampMagnitude(correctVector * multiplier, 1);
    }

    void LaunchPawn()
    {      
        selectedPawn.GetComponent<Rigidbody>().AddForce(launchVector * launchForce, ForceMode.VelocityChange);
        selectedPawn = null;
        aimArrow.aimArrow.SetActive(false);
    }

    void CheckPawnToDestroy()
    {
        if (playerPawns != null)
        {
            for (int i = 0; i < playerPawns.Count; i++)
            {
                if (playerPawns[i].transform.position.y < gameController.y_coord)
                {
                    GameObject pawnToDestroy = playerPawns[i];
                    playerPawns.Remove(playerPawns[i]);
                    Destroy(pawnToDestroy);
                    Player_Pawn_Killed.Invoke();
                }
            
            }
        }

      
    }

}
