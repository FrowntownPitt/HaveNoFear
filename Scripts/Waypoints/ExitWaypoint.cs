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
                    // Go to the lose screen, the AI reached the end
                    other.gameObject.SetActive(false);
                    ScoreScript.Lose();
                } else
                {
                    // Otherwise, the AI reached the start. Reduce the score and 
                    // if the score reaches 0 you win!
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