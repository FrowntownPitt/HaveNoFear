using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI
{
    // Calculates random target locations while the AI is idling in a room
    public class Hallway : MonoBehaviour
    {

        public WaypointSelection waypointScript;
        public NavMeshMovement navMeshMovementScript;
        Vector3 targetLocation;

        GameObject targetObject;

        public float HallwayStoppingDistance;

        bool movingToRandom = false;

        float NavAgentSpeed;

        // Use this for initialization
        void Start()
        {
            navMeshMovementScript = this.GetComponent<NavMeshMovement>();
            NavAgentSpeed = navMeshMovementScript.agent.speed;
            //waypointScript = GetComponent<WaypointSelection>();
        }

        // Update is called once per frame
        void Update()
        {
            if (waypointScript.roamingHallway)
            {
                Rooms.Room room = waypointScript.Room.GetComponent<Rooms.Room>();

                if (Time.time - waypointScript.hallwayStart > waypointScript.Room.GetComponent<Rooms.Room>().idleTime)
                {
                    // Stop roaming the hallway if our timer ran out
                    waypointScript.roamingHallway = false;
                    waypointScript.GetComponent<NavMeshMovement>().setTarget();
                    waypointScript.moving = true;
                }
                else
                {
                    if (!movingToRandom)
                    {
                        // Pick a random location in the room and set the nav agent's course
                        float x = Random.Range(room.transform.position.x - room.transform.localScale.x / 2, room.transform.position.x + room.transform.localScale.x / 2);
                        float z = Random.Range(room.transform.position.z - room.transform.localScale.z / 2, room.transform.position.z + room.transform.localScale.z / 2);
                        
                        targetLocation = new Vector3(x, 0, z);

                        GetComponent<NavMeshMovement>().agent.destination = targetLocation;

                        GetComponent<NavMeshMovement>().agent.Resume();
                        movingToRandom = true;
                    }
                }
                if (movingToRandom)
                {
                    // Crimp the forward movement while turning
                    Vector3 intendedDir = GetComponent<NavMeshMovement>().agent.desiredVelocity.normalized;
                    float speedMod = Vector3.Dot(transform.forward, intendedDir);
                    GetComponent<NavMeshMovement>().agent.speed = NavAgentSpeed * Mathf.Max(speedMod, 0.1f);
                    
                    // If we reached our destination, make the random selection start again
                    if (Vector3.Distance(this.transform.position, targetLocation) < HallwayStoppingDistance)
                    {
                        movingToRandom = false;
                    }
                }
            }
            else
            {
                if (movingToRandom)
                {
                    // If we are done roaming in the hallway, restore the nav agent
                    // speed and stop this script's logic.
                    GetComponent<NavMeshMovement>().agent.speed = NavAgentSpeed;
                    movingToRandom = false;
                }

            }

        }
    }
}