using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorRoom : MonoBehaviour {
    public GameObject home;
    public GameObject home2;
    // Use this for initialization
    void Start () {
		
	}
    //scaring by light flicker is room specific using this method call
    public void Scare()
    {
        Rooms.Room temp = home.GetComponent<Rooms.Room>();
        temp.lightFlick = true;
        Rooms.Room temp2 = home2.GetComponent<Rooms.Room>();
        temp2.doorFlick = true;
    }

    // Update is called once per frame
    void Update () {
		
	}
}
