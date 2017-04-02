using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressPlate : MonoBehaviour {
    public GameObject door;
	// Use this for initialization
	void Start ()
    {
		
	}
	

    void OnTriggerEnter(Collider other)
    {
        if(other.transform.tag == "Player" || other.transform.tag == "AI")
        {
           door.transform.Rotate(0, -90, 0);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.transform.tag == "Player" || other.transform.tag == "AI")
        {
            door.transform.Rotate(0, 90, 0);
        }
    }
	// Update is called once per frame
	void Update () {
		
	}
}
