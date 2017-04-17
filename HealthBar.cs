using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour {

    public GameObject AI;
    public float height = 1.5f;

    public Text HealthText;

    //private RectTransform transform;

	// Use this for initialization
	void Start () {
        //transform = GetComponent<RectTransform>();
	}
	
	// Update is called once per frame
	void Update () {
        // Make the health bar follow the AI
        // and update the text
        transform.localPosition = new Vector3(AI.transform.position.x, AI.transform.position.y + height, AI.transform.position.z);
        HealthText.text = (int)(AI.GetComponent<AI.Fearometer>().amount * 100) + "";
	}
}
