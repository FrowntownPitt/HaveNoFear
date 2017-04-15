using System.Collections;
using System.Collections.Generic;
using AI;
using UnityEditor;
using UnityEngine;

public class WriteOnWall : MonoBehaviour
{
    public GameObject Writing;
    public float Dist;
    public bool EPress = false;
    private GameObject AOE;
    void Start()
    {
        AOE = GameObject.FindGameObjectWithTag("Player");
    }

	// Update is called once per frame
	void Update () {
	    if (Input.GetKeyDown(KeyCode.E))
	    {
            EPress = true;
	    }
        if (Input.GetMouseButtonDown(0) && EPress)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            Physics.Raycast(ray, out hit, 100f);
            float yRot;
            if (Vector3.Distance(AOE.transform.position, hit.point) < AOE.transform.GetComponent<CapsuleCollider>().radius + Dist)
            {
                if (hit.transform.rotation.y == 0)
                    yRot = -90;
                else
                    yRot = 0;
                GameObject newObject = Instantiate(Writing, hit.point, Quaternion.Euler(0, yRot, 0)) as GameObject;
            }
        }
        if(Input.GetKeyUp(KeyCode.E))
        {
            EPress = false;
        }
    }
}
