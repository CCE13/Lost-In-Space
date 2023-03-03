using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace UI
{
    public abstract class UIController : MonoBehaviour
    {

        /// <summary>
        /// Restarts the current active scene
        /// </summary>
        public void Restart()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        /// <summary>
        /// Brings the player back to the main menu
        /// </summary>
        public void ReturnToMainMenu()
        {
            SceneManager.LoadScene("Main Menu");
        }

        /// <summary>
        /// Brings the player back to the level select
        /// </summary>
        public void ReturnToLevelSelect()
        {
            SceneManager.LoadScene("Level Select");
        }
    }
}

