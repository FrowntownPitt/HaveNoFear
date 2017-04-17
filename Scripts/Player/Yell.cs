using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Yell : MonoBehaviour {
    public bool reached = false;
    private float volumeMax = .015f;
    private string listen;
    private AudioClip record = new AudioClip();
    private int data = 128;

	// detects when the player yells
	void Start ()
    {
        //begins recording
        if (listen == null)
            listen = Microphone.devices[0];
        record = Microphone.Start(listen, true, 999, 44100);
    }
	
	// Update is called once per frame
	void Update ()
    {
        //maximum is the audio data's peak
        float maximum = 0;
        //stores audio data
        float[] colData = new float[data];
        int micPosition = Microphone.GetPosition(null) - (data + 1);
        //when sounds are heard
        if (micPosition >= 0)
        {
            //data is added
            record.GetData(colData, micPosition);
            //loops through data and finds a general value to assign audio. In our particular
            //case I could only max out my recording device
            //so maximum is basically a threshold that has to be met
            for (int i = 0; i < data; i++)
            {
                float dataMax = (colData[i] * colData[i]);
                maximum = dataMax / 64;
            }
        }
        //if sufficiently loud
        if (maximum > volumeMax)
        {
            reached = true;
        }
    }
    //only works if AI is in same room as player
    void OnTriggerStay(Collider other)
    {
        if (reached)
        {
            Rooms.Room temp = other.gameObject.GetComponent<Rooms.Room>();
            temp.yellFlick = true;
            reached = false;
        }
    }
}
