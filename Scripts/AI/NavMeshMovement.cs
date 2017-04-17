using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace AI
{
    public class NavMeshMovement : MonoBehaviour
    {

        public Animator animator;

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
            // Make the animation move to the speed of the nav agent
            animator.SetFloat("Speed", agent.speed);
        }

        // Set the destination of the agent to the target decided by WaypointSelection
        public void setTarget()
        {
            if (WaypointScript.targetWaypoint != null)
                agent.destination = WaypointScript.targetWaypoint.transform.position;
        }
    }
}