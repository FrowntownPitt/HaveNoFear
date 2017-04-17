using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Moveable : MonoBehaviour {

    public bool selected;
    public Material selectable;
    public Material unselectable;
    public float sel;
    public GameObject indic;
    public bool inRange = false;

    private bool cantDrop;
    private bool holding = false;
    private GameObject AOE;
	// Use this for initialization
	void Start ()
    {
        AOE = GameObject.FindGameObjectWithTag("Player");
    }
	// Update is called once per frame
	void Update ()
    {
        //throws held object
        if(Input.GetKeyDown(KeyCode.Q) && holding)
        {
            this.transform.parent = null;
            selected = false;
            this.GetComponent<Rigidbody>().AddForce(AOE.transform.forward * 720);
        }
        if (!inRange)
            selected = false;
        //checks distance to player
        if (Vector3.Distance(AOE.transform.position, this.transform.position) < (AOE.transform.GetComponent<CapsuleCollider>().radius * sel))
        {
            inRange = true;
            if (this.transform.GetChild(0).tag == "FancyChair")
                this.transform.GetChild(0).gameObject.GetComponent<Renderer>().material = selectable;
            else
                this.GetComponent<Renderer>().material = selectable;
        }
        //checks distance to player
        if (Vector3.Distance(AOE.transform.position, this.transform.position) >= (AOE.transform.GetComponent<CapsuleCollider>().radius * sel))
        {
            inRange = false;
            if (this.transform.GetChild(0).tag == "FancyChair")
                this.transform.GetChild(0).gameObject.GetComponent<Renderer>().material = unselectable;
            else
                this.GetComponent<Renderer>().material = unselectable;
        }
        //when held, object no longer obeys physics and becomes indicated
        if (selected)
        {
            indic.SetActive(true);
            transform.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
            transform.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionY;
            if(!holding)
                transform.position = new Vector3(this.transform.position.x, 1, this.transform.position.z);
            holding = true;
            this.gameObject.GetComponent<UnityEngine.AI.NavMeshObstacle>().enabled = false;
            //to move with player
            this.transform.parent = AOE.transform;
            this.GetComponent<BoxCollider>().isTrigger = true;
        }
        if (!selected && !cantDrop)
        {
            indic.SetActive(false);
            holding = false;
            this.transform.parent = null;
            this.gameObject.GetComponent<UnityEngine.AI.NavMeshObstacle>().enabled = true;
            transform.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            this.GetComponent<BoxCollider>().isTrigger = false;
        }
    }

    void OnTriggerEnter(Collider col)
    {
        if(col.tag == "Door")
        {
            cantDrop = true;
        }
    }

    void OnTriggerExit(Collider col)
    {
        if(col.tag == "Door")
        {
            cantDrop = false;
        }
    }
}
