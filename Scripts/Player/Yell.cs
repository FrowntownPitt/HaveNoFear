using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Yell : MonoBehaviour {
    public bool reached = false;
    private float volumeMax = .015f;
    private string listen;
    private AudioClip record = new AudioClip();
    private int data = 128;

	// Use this for initialization
	void Start ()
    {
        if (listen == null)
            listen = Microphone.devices[0];
        record = Microphone.Start(listen, true, 999, 44100);
    }
	
	// Update is called once per frame
	void Update ()
    {
        float maximum = 0;
        float[] colData = new float[data];
        int micPosition = Microphone.GetPosition(null) - (data + 1);
        if (micPosition >= 0)
        {
            record.GetData(colData, micPosition);
            for (int i = 0; i < data; i++)
            {
                float dataMax = (colData[i] * colData[i]);
                maximum = dataMax / 64;
            }
        }
        if (maximum > volumeMax)
        {
            reached = true;
        }
    }
    
    void OnTriggerStay(Collider other)
    {
        Rooms.Room temp = other.gameObject.GetComponent<Rooms.Room>();
        temp.yellFlick = true;
    }
}
