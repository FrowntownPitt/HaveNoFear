using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI {
    public class ScareHandler : MonoBehaviour {

        public enum Scares
        {
            FLICKER,
            DOOR,
            THROW,
            YELL
        };

        Fearometer fearometer;

        public float fearCost;

        //Fearometer fearometer;
        Queue<Scares> ScareQueue = new Queue<Scares>();

        // Use this for initialization
        void Start() {
            fearometer = GetComponent<Fearometer>();
        }

        // Update is called once per frame
        void Update() {

        }

        public void AddScare(Scares scare)
        {
            if (!ScareQueue.Contains(scare))
            {
                fearometer.amount += fearCost;
                ScareQueue.Enqueue(scare);
                //Debug.Log("Adding scare: " + scare);
                StartCoroutine(RemoveScare(1.0f));
            }
        }

        IEnumerator RemoveScare(float time)
        {
            yield return new WaitForSeconds(time);
            ScareQueue.Dequeue();
            //Debug.Log("Removing scare: " + ScareQueue.Dequeue());
        }
    }
}