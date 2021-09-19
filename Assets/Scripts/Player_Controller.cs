using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Player_Controller : MonoBehaviour
{
    public static Player_Controller instance;

    public new Camera camera;
    public GameController gameController;
    public AimArrow aimArrow;
    public GameObject sphere;
    public LayerMask layerMask;
    public List<GameObject> playerPawns;
    public GameObject selectedPawn;
    public Vector3 mouseClickPosition;
    public float multiplier = 5;
    public float launchForce = 35;
    public Vector3 launchVector;

    public CinemachineFreeLook _freeLookComponent;

    public UnityEvent Player_Pawn_Killed;

    [SerializeField] private ParticleSystem explosion;
    private bool isCameraMoving;

    void Start()
    {
        aimArrow = GameObject.Find("AimArrow" ).GetComponent<AimArrow>();
        //aimArrow = gameObject.GetComponent<AimArrow>();
        gameController = GameObject.Find("GameController(Script)").GetComponent<GameController>();
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

            if(selectedPawn != null)
            {
                SetVector();
                aimArrow.ArrowAim(launchVector, selectedPawn);

            }
        }
        
        CameraControl();

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
        if (isCameraMoving)
            return;
        if (Physics.Raycast(ray, out hit, 1000, layerMask))
        {
            GameObject obj = hit.transform.gameObject;
            if (playerPawns.Contains(obj))
            {
                selectedPawn = obj;

                mouseClickPosition = Input.mousePosition;
                aimArrow.aimArrow.SetActive(true);

                sphere.transform.position = selectedPawn.transform.position;
                sphere.SetActive(true);
            }        
        }
    }

    void SetVector()
    {
        //if (selectedPawn == null)
        //    return;
        float x = mouseClickPosition.x - Input.mousePosition.x;
        float y = mouseClickPosition.y - Input.mousePosition.y;
        float correctX = x / Screen.width;
        float correctZ = y / Screen.height;
        Vector3 correctVector = new Vector3(correctX, 0, correctZ);
        launchVector = Vector3.ClampMagnitude(correctVector * multiplier, 1);

        RotatePawnToVector(selectedPawn, launchVector);
    }

    void LaunchPawn()
    {      
        selectedPawn.GetComponent<Rigidbody>().AddForce(launchVector * launchForce, ForceMode.VelocityChange);
        selectedPawn = null;
        aimArrow.aimArrow.SetActive(false);
        sphere.SetActive(false);
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

                    PlayExplosionEffect(pawnToDestroy);

                    Destroy(pawnToDestroy);
                    Player_Pawn_Killed.Invoke();
                }            
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
        newExplosion.GetComponent<Renderer>().sharedMaterial.color = Color.cyan;
        var expeffect = Instantiate(newExplosion, obj.transform.position, obj.transform.rotation);
        //Destroy(expeffect, 3);
    }

    void CameraControl()
    {
        if (Input.GetMouseButton(0) && selectedPawn != null)
        {
            
            _freeLookComponent.enabled = false;
            isCameraMoving = false;
        }
        else if(Input.GetMouseButton(0))       
        {
            _freeLookComponent.enabled = true;
            isCameraMoving = true;
        }
    }

}
