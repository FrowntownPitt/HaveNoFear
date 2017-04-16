using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace Player
{
    public class Score : MonoBehaviour
    {

        public int RemainingAI = 0;
        public Text OverlayText;
        public int WinSceneNumber;
        public int LoseSceneNumber;

        // Use this for initialization
        void Start()
        {
            OverlayText.text = "Remaining: " + RemainingAI;
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void UpdateOverlay()
        {
            OverlayText.text = "Remaining: " + RemainingAI;
        }

        public void Win()
        {
            SceneManager.LoadScene(WinSceneNumber);
        }

        public void Lose()
        {
            SceneManager.LoadScene(LoseSceneNumber);
        }
    }
}