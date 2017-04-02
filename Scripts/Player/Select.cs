using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Select : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100f))
            {
                if (hit.transform.tag == "Light")
                {
                    Collider col = hit.collider;
                    if(col.transform.GetChild(0).gameObject.activeSelf)
                        col.transform.GetChild(0).gameObject.SetActive(false);
                    else
                        col.transform.GetChild(0).gameObject.SetActive(true);
                }
                if(hit.transform.tag == "Selectable")
                {
                    Moveable temp = hit.collider.gameObject.GetComponent<Moveable>();
                    if (!temp.selected && temp.inRange)
                        temp.selected = true;
                    else if (temp.selected && temp.inRange)
                        temp.selected = false;
                }
                if(hit.transform.tag == "Door")
                {
                    PressPlate temp = hit.collider.gameObject.GetComponent<PressPlate>();
                    temp.Toggle();
                }
            }
        }
    }
}
