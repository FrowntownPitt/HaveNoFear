using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace AI
{
    public class NavMeshMovement : MonoBehaviour
    {

        private WaypointSelection WaypointScript;
        //[HideInInspector]
        public NavMeshAgent agent;
        private bool targetSet = false;

        // Use this for initialization
        void Start()
        {
            //agent = this.GetComponent<NavMeshAgent>();
            WaypointScript = this.GetComponent<WaypointSelection>();
        }

        // Update is called once per frame
        void Update()
        {
            //if(WaypointScript.moving && WaypointScript.targetWaypoint != null)
            //{
            //    agent.Resume();
            //}

            //if (!WaypointScript.moving)
            //{
            //    //Debug.Log("Stopping movement");
            //    //agent.Stop();
            //}
        }

        public void setTarget()
        {
            //Debug.Log("Setting target");
            if (WaypointScript.targetWaypoint != null)
                agent.destination = WaypointScript.targetWaypoint.transform.position;
        }
    }
}