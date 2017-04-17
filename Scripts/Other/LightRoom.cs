using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightRoom : MonoBehaviour {

    public GameObject home;
	// Use this for initialization
	void Start () {
		
	}
	//scaring by light flicker is room specific using this method call
    public void Scare()
    {
        Rooms.Room temp = home.GetComponent<Rooms.Room>();
        temp.lightFlick = true;
    }
	// Update is called once per frame
	void Update () {
		
	}
}
