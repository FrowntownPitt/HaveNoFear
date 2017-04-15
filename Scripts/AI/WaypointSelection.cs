using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using UnityEngine.AI;

namespace AI
{
    public class WaypointSelection : MonoBehaviour
    {

        public GameObject initialStartingPoint;

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
        
        //public float randomlyChooseVisited;

        private Stack<GameObject> visitedStackAll = new Stack<GameObject>();
        private List<Waypoints.Waypoint> visitedList = new List<Waypoints.Waypoint>();
        private bool fleeing = false;

        public enum INTERRUPTS {
            JUMPSCARE,
            FLEE,
            ESCAPE
        }
        
        //private float startTime;
        //private float journeyDistance;
        //private Vector3 startLocation;

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
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                InterruptSelection(INTERRUPTS.ESCAPE);
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                InterruptSelection(INTERRUPTS.FLEE);
            }
        }

        public void InterruptSelection(INTERRUPTS interrupt)
        {
            if (interrupt == INTERRUPTS.JUMPSCARE)
            {
                //Debug.Log("INTERRUPT: Jumpscare occurred");
            }
            if (interrupt == INTERRUPTS.FLEE)
            {
                if (visitedStackAll.Count() > 0)
                {
                    //targetWaypoint = visitedStackAll.Pop();
                    while (visitedStackAll.Count() > 0)
                    {
                        GameObject previousWaypoint = targetWaypoint;
                        targetWaypoint = visitedStackAll.Pop();
                        if (previousWaypoint.name.Equals(targetWaypoint.name))
                        {
                            continue;
                        }
                        while(visitedList.Contains(targetWaypoint.GetComponent<Waypoints.Waypoint>()))
                            visitedList.Remove(targetWaypoint.GetComponent<Waypoints.Waypoint>());
                        Debug.Log("Popping at random");
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
            }
            if (interrupt == INTERRUPTS.ESCAPE)
            {
                targetWaypoint = initialStartingPoint;
                fleeing = true;
                idlingInRoom = false;
                roamingHallway = false;
                GetComponent<NavMeshMovement>().setTarget();
                visitedList.Clear();
                //Debug.Log("INTERRUPT: ESCAPE!");
            }
        }

        public void Attack(GameObject player)
        {
            attacking = true;
            target = player;
        }

        void OnTriggerEnter(Collider col)
        {
            if (col.CompareTag("Lose"))
                Debug.Log("YOU LOSE SUCKER");
            if (attacking && col.CompareTag("Waypoint"))
            {
                attacking = false;
                InterruptSelection(WaypointSelection.INTERRUPTS.FLEE);
            }
            //Debug.Log("Collider: " + col.gameObject.name);
            if(col.gameObject.name.Equals(initialStartingPoint.name) && fleeing)
            {
                //Debug.Log("Fleeing the haunted house. Win!");
                gameObject.SetActive(false);
            }
            if (col.CompareTag("Waypoint"))
            {
                visitedList.Add(col.gameObject.GetComponent<Waypoints.Waypoint>());
                if (visitedStackAll.Count > 0 && (visitedStackAll.Peek().name != col.gameObject.name) || visitedStackAll.Count == 0)
                {
                    //Debug.Log(this.name + " pushing current collider to stack: " + col.name);
                    visitedStackAll.Push(col.gameObject);
                }
            }
                //.visited = true;

            if(col.gameObject == targetWaypoint)
            {
                moving = false;
                targetWaypoint = null;
            }
            if (col.gameObject.name.Equals("End Waypoint")){
                gameObject.SetActive(false);
                //Debug.Log("Reached End");
            }
            else if(targetWaypoint == null && col.gameObject.CompareTag("Waypoint"))
            {
                // Do some maths here to pick a random waypoint option
                if(col.gameObject.GetComponent<Waypoints.Waypoint>().nextWaypoints.Count > 0)
                {
                    previousWaypoint = col.gameObject;
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
                            //Debug.Log("Idling in room");
                            return;
                        }
                        //else 
                                //Debug.Log("Not idling in room");
                    }

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
                        //Debug.Log("Choosing waypoint: " + targetWaypoint.name);
                        if (randGen.NextDouble() < fearometer.amount)
                        {
                            //Debug.Log("Choosing previously visited at random.");
                            chooseVisited = true;
                        }
                    }
                    else
                    {
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