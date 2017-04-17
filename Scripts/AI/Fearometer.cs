using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI
{
    // Container for the fearometer.
    public class Fearometer : MonoBehaviour
    {
        // The actual value of the fearometer
        public float amount;

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