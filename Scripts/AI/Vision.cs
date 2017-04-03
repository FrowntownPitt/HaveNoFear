using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI
{
    public class Vision : MonoBehaviour
    {

        public GameObject RaycastObject;
        public List<int> ScaryLayers = new List<int>();

        public float VisionDistance;
        // Radius of the spherecast (cylinder shooting straight from the origin)
        public float VisionRadius;

        private bool spooked = false;
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
            if(Physics.SphereCast(RaycastObject.transform.position, VisionRadius, RaycastObject.transform.forward, out ray, VisionDistance, LayerMask))
            {
                Debug.Log("Found: " + ray.collider.name);
                if (ray.collider.gameObject.CompareTag("AI") && !spooked)
                {
                    GetComponent<WaypointSelection>().InterruptSelection(WaypointSelection.INTERRUPTS.FLEE);
                    StartCoroutine(SpookPause(spookPauseTime));
                }
            }
            
        }

        IEnumerator SpookPause(float t)
        {
            
            yield return new WaitForSeconds(t);
            spooked = false;
        }
    }
}