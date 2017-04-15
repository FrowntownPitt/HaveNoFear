using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI
{
    public class Vision : MonoBehaviour
    {

        public GameObject RaycastObject;
        public GameObject LowRaycastObject;
        public List<int> ScaryLayers = new List<int>();

        public float VisionDistance;
        // Radius of the spherecast (cylinder shooting straight from the origin)
        public float VisionRadius;

        private bool spooked = false;
        private bool looking = true;
        public float spookPauseTime;

        [SerializeField]
        public class LayerLevels
        {
            public int layer;
            public int layerlevel;
        }

        int LayerMask;

        // Use this for initialization
        void Start()
        {
            for(int i=0; i<ScaryLayers.Count; i++)
            {
                int maskBits = ScaryLayers[i];
                LayerMask = LayerMask | (1 << maskBits);
            }
        }

        // Update is called once per frame
        void Update()
        {
            Debug.DrawRay(RaycastObject.transform.position, RaycastObject.transform.forward * VisionDistance);
            RaycastHit ray;
            RaycastHit lowRay;
            if(Physics.SphereCast(RaycastObject.transform.position, VisionRadius, RaycastObject.transform.forward, out ray, VisionDistance, LayerMask))
            {
                Debug.Log("Found: " + ray.collider.name);
                if (ray.collider.gameObject.CompareTag("AI") && !spooked)
                {
                    GetComponent<WaypointSelection>().InterruptSelection(WaypointSelection.INTERRUPTS.FLEE);
                    StartCoroutine(SpookPause(spookPauseTime));
                }
            }
            if (Physics.SphereCast(LowRaycastObject.transform.position, VisionRadius, LowRaycastObject.transform.forward, out lowRay, VisionDistance, LayerMask))
            {
                Debug.Log("Found: " + ray.collider.name);
                if (ray.collider.gameObject.CompareTag("Player") && !spooked)
                {
                    float attackChance = 1f - GetComponent<Fearometer>().amount;
                    if ((new System.Random()).NextDouble() < attackChance && looking)
                    {
                        GetComponent<WaypointSelection>().Attack(lowRay.collider.gameObject);
                        looking = false;
                        StartCoroutine(SpookPause(spookPauseTime));
                    }
                    else
                    {
                        GetComponent<WaypointSelection>().InterruptSelection(WaypointSelection.INTERRUPTS.FLEE);
                        StartCoroutine(SpookPause(spookPauseTime));
                    }
                }
            }

        }

        IEnumerator SpookPause(float t)
        {
            
            yield return new WaitForSeconds(t);
            spooked = false;
            looking = true;
        }
    }
}