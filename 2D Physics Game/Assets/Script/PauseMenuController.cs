using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UI
{
    public class PauseMenuController : UIController
    {
        [SerializeField] private GameObject _pauseMenuPanel;
        public static bool Paused { get; private set; }
        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.Escape))
            {
                PauseGame();
            }
        }
        public void PauseGame()
        {
            if (Paused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
        /// <summary>
        /// Resumes the game
        /// </summary>
        private void Resume()
        {
            
            Paused = false;
            Time.timeScale = 1f;
            _pauseMenuPanel.SetActive(false);
        }

        /// <summary>
        /// Pauses the game
        /// </summary>
        private void Pause()
        {
            Paused = true;
            Time.timeScale = 0f;
            _pauseMenuPanel.SetActive(true);
        }
    }
}

