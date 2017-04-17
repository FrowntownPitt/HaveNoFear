using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rooms
{
    public class Room : MonoBehaviour
    {
        public bool lightFlick;
        public bool LFGoing;
        public bool doorFlick;
        public bool DFGoing;
        public bool throwFlick;
        public bool TFGoing;
        public bool yellFlick;
        public bool YFGoing;
        public float idleTime;
        // Use this for initialization
        void Start()
        {
            
        }
        //knows when the AI are in the room
        void OnTriggerStay(Collider other)
        {
            //Rooms know if a specific scare event has occurred with flick bools
            if(lightFlick)
            {
                //coroutine to allow cooldowns between scares of the same type and to reset bools
                StartCoroutine(LFPause(7));
                lightFlick = false;
                LFGoing = true;
            }
            //going bools are used for cooldowns
            if(LFGoing)
            {
                if (other.transform.tag == "AI")
                {
                    AI.ScareHandler temp = other.gameObject.GetComponent<AI.ScareHandler>();
                    temp.AddScare(AI.ScareHandler.Scares.FLICKER);
                }
            }
            if (doorFlick)
            {
                StartCoroutine(DFPause(7));
                doorFlick = false;
                DFGoing = true;
            }
            if (DFGoing)
            {
                if (other.transform.tag == "AI")
                {
                    AI.ScareHandler temp = other.gameObject.GetComponent<AI.ScareHandler>();
                    temp.AddScare(AI.ScareHandler.Scares.DOOR);
                }
            }
            if (throwFlick)
            {
                StartCoroutine(TFPause(7));
                throwFlick = false;
                TFGoing = true;
            }
            if (TFGoing)
            {
                if (other.transform.tag == "AI")
                {
                    AI.ScareHandler temp = other.gameObject.GetComponent<AI.ScareHandler>();
                    temp.AddScare(AI.ScareHandler.Scares.THROW);
                }
            }
            if (yellFlick)
            {
                StartCoroutine(YFPause(7));
                yellFlick = false;
                YFGoing = true;
            }
            if (YFGoing)
            {
                if (other.transform.tag == "AI")
                {
                    AI.ScareHandler temp = other.gameObject.GetComponent<AI.ScareHandler>();
                    temp.AddScare(AI.ScareHandler.Scares.YELL);
                    other.GetComponent<AI.WaypointSelection>().InterruptSelection(AI.WaypointSelection.INTERRUPTS.FLEE);
                }
            }
        }

        IEnumerator LFPause(float t)
        {
            yield return new WaitForSeconds(t);
            LFGoing = false;
            lightFlick = false;
        }
        IEnumerator DFPause(float t)
        {
            yield return new WaitForSeconds(t);
            DFGoing = false;
            doorFlick = false;
        }
        IEnumerator TFPause(float t)
        {
            yield return new WaitForSeconds(t);
            TFGoing = false;
            throwFlick = false;
        }
        IEnumerator YFPause(float t)
        {
            yield return new WaitForSeconds(t);
            YFGoing = false;
            yellFlick = false;
        }
        void OnTriggerEnter(Collider other)
        {
            if(other.transform.tag == "Selectable")
            {
                throwFlick = true;
            }
        }
        void OnTriggerExit(Collider other)
        {
            if(other.transform.tag == "Selectable")
            {
                throwFlick = true;
            }
        }
        // Update is called once per frame
        void Update()
        {

        }
    }
}