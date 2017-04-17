using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Waypoints
{
    // Waypoint container.  Place on each waypoint object and point it to all
    // waypoints in the adjacent rooms.
    public class Waypoint : MonoBehaviour
    {

        public List<WaypointEnum> nextWaypoints;
        public GameObject FleeWaypoint;
        public List<WaypointEnum> EndWaypoints;
        public List<RoomEnum> HallwayRooms;
        public List<RoomEnum> Leaf;

        [HideInInspector]
        public bool visited = false;

        [System.Serializable]
        public class WaypointEnum
        {
            public GameObject Waypoint;
            public int chance;
            //[HideInInspector]
            //public bool visited = false;
        }

        [System.Serializable]
        public class RoomEnum
        {
            public GameObject Room;
        }
    }
}