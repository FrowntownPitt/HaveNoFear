using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI
{
    public class CurrentRoom : MonoBehaviour
    {
        [HideInInspector]
        // Save the current room the AI is located, used for room-specific scares
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
            }
        }
    }
}