using System.Collections;
using System.Collections.Generic;
using AI;
using UnityEngine;
//Lets you write on a wall
public class WriteOnWall : MonoBehaviour
{
    public GameObject Writing1;
    public GameObject Writing2;
    public GameObject Writing3;
    public float Dist;
    public bool EPress = false;
    private GameObject AOE;
    void Start()
    {
        AOE = GameObject.FindGameObjectWithTag("Player");
    }

	// Update is called once per frame
	void Update ()
    {
	    if (Input.GetKeyDown(KeyCode.E))
	    {
            EPress = true;
	    }
        //controls are E and left click
        if (Input.GetMouseButtonDown(0) && EPress)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            Physics.Raycast(ray, out hit, 100f);
            float yRot;
            GameObject temp;
            //chooses one of the writings randomly and places it on the wall you click
            if (Vector3.Distance(AOE.transform.position, hit.point) < AOE.transform.GetComponent<CapsuleCollider>().radius + Dist)
            {
                if (hit.collider.tag == "Wall")
                {
                    if (hit.transform.rotation.y == 0)
                        yRot = -90;
                    else
                        yRot = 0;
                    if ((new System.Random()).NextDouble() <= .33)
                        temp = Instantiate(Writing1, hit.point, Quaternion.Euler(0, yRot, 0)) as GameObject;
                    else if ((new System.Random()).NextDouble() <= .66)
                        temp = Instantiate(Writing2, hit.point, Quaternion.Euler(0, yRot, 0)) as GameObject;
                    else if ((new System.Random()).NextDouble() <= 1.00)
                        temp = Instantiate(Writing3, hit.point, Quaternion.Euler(0, yRot, 0)) as GameObject;
                }
            }
        }
        if(Input.GetKeyUp(KeyCode.E))
        {
            EPress = false;
        }
    }
}
