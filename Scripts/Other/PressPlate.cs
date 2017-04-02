using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressPlate : MonoBehaviour {
	// Use this for initialization
	void Start ()
    {
		
	}
	

    void OnTriggerEnter(Collider other)
    {
        if(other.transform.tag == "Player" || other.transform.tag == "AI")
        {
            Transform dad = transform.parent.transform;
            transform.parent = null;
            dad.transform.Rotate(0, -90, 0);
            transform.parent = dad;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.transform.tag == "Player" || other.transform.tag == "AI")
        {
            Transform dad = transform.parent.transform;
            transform.parent = null;
            dad.transform.Rotate(0, 90, 0);
            transform.parent = dad;
        }
    }
	// Update is called once per frame
	void Update () {
		
	}
}
