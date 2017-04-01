using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI
{
    public class CurrentRoom : MonoBehaviour
    {
        [HideInInspector]
        public GameObject currentRoom = null;

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        void OnTriggerEnter(Collider col)
        {
            if (col.CompareTag("Room"))
            {
                currentRoom = col.gameObject;
                //Debug.Log("Entering room: " + col.name);
            }
        }
    }
}