using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightRoom : MonoBehaviour {

    public GameObject home;
	// Use this for initialization
	void Start () {
		
	}
	
    public void Scare()
    {
        Rooms.Room temp = home.GetComponent<Rooms.Room>();
        temp.lightFlick = true;
    }
	// Update is called once per frame
	void Update () {
		
	}
}
