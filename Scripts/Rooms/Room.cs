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

        void OnTriggerStay(Collider other)
        {
            if(lightFlick)
            {
                StartCoroutine(LFPause(1));
                lightFlick = false;
                LFGoing = true;
            }
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
                StartCoroutine(DFPause(1));
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
                StartCoroutine(TFPause(1));
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
                StartCoroutine(YFPause(1));
                yellFlick = false;
                YFGoing = true;
            }
            if (YFGoing)
            {
                if (other.transform.tag == "AI")
                {
                    AI.ScareHandler temp = other.gameObject.GetComponent<AI.ScareHandler>();
                    temp.AddScare(AI.ScareHandler.Scares.YELL);
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