using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI
{
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
                //Debug.Log("Roaming Hallway");
                
                Rooms.Room room = waypointScript.Room.GetComponent<Rooms.Room>();
                if (Time.time - waypointScript.hallwayStart > waypointScript.Room.GetComponent<Rooms.Room>().idleTime)
                {
                    //Debug.Log("Stopping roaming Hallway");
                    waypointScript.roamingHallway = false;
                    waypointScript.GetComponent<NavMeshMovement>().setTarget();
                    waypointScript.moving = true;
                }
                else
                {
                    if (!movingToRandom)
                    {
                        float x = Random.Range(room.transform.position.x - room.transform.localScale.x / 2, room.transform.position.x + room.transform.localScale.x / 2);
                        float z = Random.Range(room.transform.position.z - room.transform.localScale.z / 2, room.transform.position.z + room.transform.localScale.z / 2);

                        //targetObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
                        //targetObject.transform.position = new Vector3(x, 0, z);
                        //targetObject.SetActive(true);
                        //targetObject.GetComponent<BoxCollider>().enabled = false;

                        targetLocation = new Vector3(x, 0, z);

                        GetComponent<NavMeshMovement>().agent.destination = targetLocation;

                        GetComponent<NavMeshMovement>().agent.Resume();
                        movingToRandom = true;
                        //Debug.Log("Targeting random point");
                        //Debug.Log("Room: " + room.name);
                        //Debug.Log("x, z: (" + x + ", " + z + ")");
                    }
                }
                if (targetLocation == null)
                {

                }
                if (movingToRandom)
                {
                    Vector3 intendedDir = GetComponent<NavMeshMovement>().agent.desiredVelocity.normalized;
                    float speedMod = Vector3.Dot(transform.forward, intendedDir);
                    GetComponent<NavMeshMovement>().agent.speed = NavAgentSpeed * Mathf.Max(speedMod, 0.1f);

                    if (Vector3.Distance(this.transform.position, targetLocation) < HallwayStoppingDistance)
                    {
                        GameObject.Destroy(targetObject);
                        //Debug.Log("Reached target random point");
                        movingToRandom = false;
                    }
                }
            }
            else
            {
                if (movingToRandom)
                {
                    GetComponent<NavMeshMovement>().agent.speed = NavAgentSpeed;
                    movingToRandom = false;

                }

            }

        }
    }
}