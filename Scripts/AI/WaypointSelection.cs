using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

namespace AI
{
    public class WaypointSelection : MonoBehaviour
    {

        public GameObject initialStartingPoint;
        public int LoseSceneNumber;

        public float speed;
        public int previousWaypointBias = 0;

        private bool attacking;
        private GameObject target;
        private NavMeshAgent agent;

        public Fearometer fearometer;

        [HideInInspector]
        public bool moving = false;
        [HideInInspector]
        public GameObject targetWaypoint;
        [HideInInspector]
        public GameObject previousWaypoint;

        [HideInInspector]
        public bool idlingInRoom = false;
        [HideInInspector]
        public GameObject Room;
        [HideInInspector]
        public float idlingStart;

        [HideInInspector]
        public bool roamingHallway = false;
        [HideInInspector]
        public GameObject Hallway;
        [HideInInspector]
        public float hallwayStart;

        private Stack<GameObject> visitedStackAll = new Stack<GameObject>();
        private List<Waypoints.Waypoint> visitedList = new List<Waypoints.Waypoint>();
        [HideInInspector]
        public bool fleeing = false;

        // These have different meanings, but will interrupt the current movement of the AI
        public enum INTERRUPTS {
            JUMPSCARE,
            FLEE,
            ESCAPE
        }

        // Use this for initialization
        void Start()
        {
            fearometer = GetComponent<Fearometer>();
            agent = GetComponent<NavMeshMovement>().agent;
        }

        // Update is called once per frame
        void Update()
        {
            if(attacking)
            {
                agent.destination = target.transform.position;
            }
            //if (Input.GetKeyDown(KeyCode.Alpha1))
            //{
            //    InterruptSelection(INTERRUPTS.ESCAPE);
            //}
            //if (Input.GetKeyDown(KeyCode.Alpha2))
            //{
            //    InterruptSelection(INTERRUPTS.FLEE);
            //}
        }

        public void InterruptSelection(INTERRUPTS interrupt)
        {
            if (interrupt == INTERRUPTS.JUMPSCARE)
            {
                //Debug.Log("INTERRUPT: Jumpscare occurred");
            }
            if (interrupt == INTERRUPTS.FLEE)
            {
                // If the AI should flee, it will go back a few rooms (by 
                // way of the last waypoints it visited
                if (visitedStackAll.Count() > 0)
                {
                    while (visitedStackAll.Count() > 0)
                    {
                        GameObject previousWaypoint = targetWaypoint;
                        targetWaypoint = visitedStackAll.Pop();
                        if (previousWaypoint.name.Equals(targetWaypoint.name))
                        {
                            continue;
                        }
                        // Remove those popped waypoints from the visited list
                        while (visitedList.Contains(targetWaypoint.GetComponent<Waypoints.Waypoint>()))
                            visitedList.Remove(targetWaypoint.GetComponent<Waypoints.Waypoint>());
                        
                        // There is a random chance of continuing to get spooked farther back
                        // based on your fearometer
                        if (!((new System.Random()).NextDouble() < fearometer.amount))
                        {

                            break;
                        }
                    }
                }
                else
                    targetWaypoint = previousWaypoint;

                GetComponent<NavMeshMovement>().setTarget();
                idlingInRoom = false;
                roamingHallway = false;
                fearometer.amount = fearometer.amount - .1f;

                // There is a random chance of a FLEE turning into an ESCAPE based on the fearometer
                if((visitedStackAll.Count() <= 1) || (new System.Random()).NextDouble() < (fearometer.amount / 3f))
                {
                    InterruptSelection(INTERRUPTS.ESCAPE);
                }
            }
            if (interrupt == INTERRUPTS.ESCAPE)
            {
                // This will make the AI go back to their starting point and leave
                targetWaypoint = initialStartingPoint;
                fleeing = true;
                idlingInRoom = false;
                roamingHallway = false;
                GetComponent<NavMeshMovement>().setTarget();
                visitedList.Clear();
            }
        }

        public void Attack(GameObject player)
        {
            attacking = true;
            target = player;
        }

        // Timer to change scene to the lose scene (after the AI's attack animation is finished)
        public IEnumerator Die()
        {
            yield return new WaitForSeconds(1f);
            SceneManager.LoadScene(LoseSceneNumber);
        }

        void OnTriggerEnter(Collider col)
        {
            if (col.gameObject.tag == "Player")
            {
                // If the AI and player collide, the player should die.  Make the AI do its attack animation.
                StartCoroutine(Die());
                GetComponent<NavMeshMovement>().animator.SetFloat("Speed", 0f);
                GetComponent<NavMeshMovement>().agent.enabled = false;
                GetComponent<NavMeshMovement>().animator.SetBool("Attack", true);
                col.GetComponent<NavMeshAgent>().enabled = false;
            }
            if (attacking && col.CompareTag("Waypoint"))
            {
                attacking = false;
                InterruptSelection(WaypointSelection.INTERRUPTS.FLEE);
            }
            if(col.gameObject.name.Equals(initialStartingPoint.name) && fleeing)
            {
            }
            if (col.CompareTag("Waypoint"))
            {
                // The AI has now visited this waypoint, add it to the stack
                visitedList.Add(col.gameObject.GetComponent<Waypoints.Waypoint>());
                if (visitedStackAll.Count > 0 && (visitedStackAll.Peek().name != col.gameObject.name) || visitedStackAll.Count == 0)
                {
                    //Debug.Log(this.name + " pushing current collider to stack: " + col.name);
                    visitedStackAll.Push(col.gameObject);
                }
            }

            if(col.gameObject == targetWaypoint)
            {
                moving = false;
                targetWaypoint = null;
            }
            if (col.gameObject.name.Equals("End Waypoint")){
                //gameObject.SetActive(false);
                //Debug.Log("Reached End");
            }
            else if(targetWaypoint == null && col.gameObject.CompareTag("Waypoint"))
            {
                // Do some maths here to pick a random waypoint option
                if(col.gameObject.GetComponent<Waypoints.Waypoint>().nextWaypoints.Count > 0)
                {
                    previousWaypoint = col.gameObject;
                    // If there is a leaf room, go ahead and idle in there
                    if (previousWaypoint.GetComponent<Waypoints.Waypoint>().Leaf.Count > 0)
                    {
                        bool skipIdle = false;
                        if(GetComponent<AI.CurrentRoom>().currentRoom != null)
                        {
                            if(previousWaypoint.GetComponent<Waypoints.Waypoint>().Leaf[0].Room == GetComponent<AI.CurrentRoom>().currentRoom)
                            {
                                skipIdle = true;
                            }
                        }
                        if (!skipIdle)
                        {
                            targetWaypoint = previousWaypoint;
                            idlingInRoom = true;
                            Room = previousWaypoint.GetComponent<Waypoints.Waypoint>().Leaf[0].Room;
                            idlingStart = Time.time;
                            return;
                        }
                    }

                    // Add all of the next waypoints each 'chance' times, for biasing.
                    List<GameObject> list = new List<GameObject>();
                    for (int i = 0; i < col.gameObject.GetComponent<Waypoints.Waypoint>().nextWaypoints.Count; i++)
                    {
                        if (visitedList.Contains(col.gameObject.GetComponent<Waypoints.Waypoint>().nextWaypoints[i].Waypoint.GetComponent<Waypoints.Waypoint>()))
                        {
                            continue;
                        }
                        for (int j = 0; j < col.gameObject.GetComponent<Waypoints.Waypoint>().nextWaypoints[i].chance; j++)
                        {
                            if (col.gameObject.GetComponent<Waypoints.Waypoint>().nextWaypoints[i].Waypoint == previousWaypoint && j > 0)
                            {
                                j += previousWaypointBias;
                            }
                            list.Add(col.gameObject.GetComponent<Waypoints.Waypoint>().nextWaypoints[i].Waypoint);
                        }
                    }
                    System.Random randGen = new System.Random();

                    bool chooseVisited = false;

                    if (list.Count > 0)
                    {
                        int rand = randGen.Next(0, list.Count);
                        targetWaypoint = list[rand];
                        // pick a visited waypoint on chance based on the fearometer
                        if (randGen.NextDouble() < fearometer.amount)
                        {
                            //Debug.Log("Choosing previously visited at random.");
                            chooseVisited = true;
                        }
                    }
                    else
                    {
                        // If there are no unvisited waypoints, pick a waypoint that was visited
                        chooseVisited = true;
                    }

                    if (chooseVisited)
                    {
                        List<GameObject> visitedList = new List<GameObject>();
                        for (int i = 0; i < col.gameObject.GetComponent<Waypoints.Waypoint>().EndWaypoints.Count; i++)
                        {
                            for (int j = 0; j < col.gameObject.GetComponent<Waypoints.Waypoint>().nextWaypoints[i].chance; j++)
                            {
                                if (col.gameObject.GetComponent<Waypoints.Waypoint>().nextWaypoints[i].Waypoint == previousWaypoint && j > 0)
                                {
                                    j += previousWaypointBias;
                                }
                                visitedList.Add(col.gameObject.GetComponent<Waypoints.Waypoint>().EndWaypoints[i].Waypoint);
                            }
                        }
                        
                        previousWaypoint = col.gameObject;
                        if (visitedList.Count > 0)
                        {
                            int rand = randGen.Next(0, visitedList.Count);
                            //Debug.Log("Chose exit point: " + rand + ":" + visitedList[rand].name);
                            targetWaypoint = visitedList[rand];
                            //Debug.Log("Random waypoint chosen: " + targetWaypoint.name);
                        }
                        else
                        {
                            //Debug.Log("REACHED END");
                        }
                    }

                    // Start the idling program
                    if (targetWaypoint != null)
                    {
                        GetComponent<AI.NavMeshMovement>().setTarget();
                        moving = true;
                        Waypoints.Waypoint prev = previousWaypoint.GetComponent<Waypoints.Waypoint>();
                        Waypoints.Waypoint targ = targetWaypoint.GetComponent<Waypoints.Waypoint>();

                        List<GameObject> prevHalls = new List<GameObject>();
                        for (int i = 0; i < prev.HallwayRooms.Count; i++)
                        {
                            prevHalls.Add(prev.HallwayRooms[i].Room);
                        }
                        List<GameObject> nextHalls = new List<GameObject>();
                        for (int i = 0; i < targ.HallwayRooms.Count; i++)
                        {
                            nextHalls.Add(targ.HallwayRooms[i].Room);
                        }

                        IEnumerable<GameObject> both = prevHalls.Intersect(nextHalls);
                        GameObject bothRoom = null;
                        if (both.Count() > 0)
                            bothRoom = both.First();
                        if(bothRoom != null)
                        {
                            Room = bothRoom;
                            roamingHallway = true;
                            hallwayStart = Time.time;
                            //Debug.Log("Both room: " + bothRoom.name);
                        }

                        moving = false;

                        

                    }
                }
            }
        }
    }
}