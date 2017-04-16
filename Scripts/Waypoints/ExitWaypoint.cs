using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Waypoint
{
    public class ExitWaypoint : MonoBehaviour
    {

        public bool isLoseWaypoint = false;
        public Player.Score ScoreScript;

        // Use this for initialization
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {

        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("AI"))
            {
                //ScoreScript.RemainingAI--;
                //ScoreScript.UpdateOverlay();
                if (isLoseWaypoint)
                {
                    other.gameObject.SetActive(false);
                    ScoreScript.Lose();
                } else
                {
                    Debug.Log("Origin Waypoint Reached");
                    if (other.gameObject.GetComponent<AI.WaypointSelection>().fleeing)
                    {
                        ScoreScript.RemainingAI--;
                        ScoreScript.UpdateOverlay();
                        other.gameObject.SetActive(false);
                        if(ScoreScript.RemainingAI <= 0)
                            ScoreScript.Win(); 
                    }
                    else
                    {
                    }
                }
            }
        }
    }
}