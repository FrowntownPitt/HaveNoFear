using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI
{
    public class Fearometer : MonoBehaviour
    {

        public float amount;

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if(amount < 0f)
            {
                amount = 0f;
            }
        }
    }
}