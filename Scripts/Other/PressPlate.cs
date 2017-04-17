using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressPlate : MonoBehaviour {
    public bool open;
    
    // Use this for initialization
	void Start ()
    {
		
	}
    //switches the door from open to closed, or vice versa
	public void Toggle()
    {
        if(open)
        {
            Transform dad = transform.parent.transform;
            transform.parent = null;
            dad.transform.Rotate(0, 90, 0);
            transform.parent = dad;
            open = false;
        }
        else
        {
            Transform dad = transform.parent.transform;
            transform.parent = null;
            dad.transform.Rotate(0, -90, 0);
            transform.parent = dad;
            open = true;
        }
    }
    //detects when player or AI need to pass
    void OnTriggerStay(Collider other)
    {
        if((other.transform.tag == "Player" || other.transform.tag == "AI") && !open)
        {
            Transform dad = transform.parent.transform;
            transform.parent = null;
            dad.transform.Rotate(0, -90, 0);
            transform.parent = dad;
            open = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if ((other.transform.tag == "Player" || other.transform.tag == "AI") && open)
        {
            Transform dad = transform.parent.transform;
            transform.parent = null;
            dad.transform.Rotate(0, 90, 0);
            transform.parent = dad;
            open = false;
        }
    }
	// Update is called once per frame
	void Update () {
		
	}
}
