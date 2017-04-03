using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayMovement : MonoBehaviour
{
    public float speed;
    public GameObject cam;
    public float camSpeed;

    private RaycastHit target;
    private float startTime;
    private bool moving = false;
    private UnityEngine.AI.NavMeshAgent player;
    private bool rightP = false;
    private bool downP = false;
    private bool upP = false;
    private bool leftP = false;

    bool findingpath = false;

    // Use this for initialization
    void Start ()
    {
        player = GetComponent<UnityEngine.AI.NavMeshAgent>();
    }
	
	// Update is called once per frame
	void Update ()
    {

        Vector3 intendedDir = player.desiredVelocity.normalized;
        float speedMod = Vector3.Dot(transform.forward, intendedDir);
        player.speed = speed * Mathf.Max(speedMod, 0.1f);

        if (Input.GetMouseButtonDown(1))
        {
            moving = true;
            findingpath = true;
            Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out target, 100);
        }
        if (findingpath && moving)
        {
            findingpath = false;
            player.destination = target.point;
        }
        if(moving && Vector3.Distance(transform.position, player.destination) <= 1.0){
            moving = false;
            //Debug.Log("Reached destination.");
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            rightP = true;

        }
        if (Input.GetKeyUp(KeyCode.D))
            rightP = false;
        if (Input.GetKeyDown(KeyCode.W))
        {
            upP = true;

        }
        if (Input.GetKeyUp(KeyCode.W))
            upP = false;
        if (Input.GetKeyDown(KeyCode.A))
        {
            leftP = true;

        }
        if (Input.GetKeyUp(KeyCode.A))
            leftP = false;
        if (Input.GetKeyDown(KeyCode.S))
        {
            downP = true;

        }
        if (Input.GetKeyUp(KeyCode.S))
            downP = false;
    }
    private void FixedUpdate()
    {
        if(rightP)
            cam.transform.Translate(camSpeed, 0, 0);
        if (upP)
            cam.transform.Translate(0, camSpeed, 0);
        if (downP)
            cam.transform.Translate(0, camSpeed * -1f, 0);
        if (leftP)
            cam.transform.Translate(camSpeed * -1f, 0, 0);
        cam.transform.Translate(Vector3.forward * Input.GetAxis("Mouse ScrollWheel"));
        if (Input.mousePosition.x < 25)
            cam.transform.Translate(camSpeed * -1f, 0, 0);
        if (Input.mousePosition.x > Screen.width - 25)
            cam.transform.Translate(camSpeed, 0, 0);
        if (Input.mousePosition.y < 25)
            cam.transform.Translate(0, camSpeed * -1f, 0);
        if (Input.mousePosition.y > Screen.height - 25)
            cam.transform.Translate(0, camSpeed, 0);
    }
}
