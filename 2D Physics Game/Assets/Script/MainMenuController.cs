using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    public class MainMenuController : MonoBehaviour
    {
        [SerializeField] private GameObject _mainMenuPanel;
        [SerializeField] private GameObject _creditsPanel;
        [SerializeField] private GameObject _confirmationPanel;

        [SerializeField] private string _levelToLoad;

        [SerializeField]private List<ScreenTargets> _teleportorScreenTargets;
        public void Start()
        {
            Time.timeScale = 1f;
            ReturnToMainPanel();
            foreach (var item in _teleportorScreenTargets)
            {
                item.objectToSet.position = Camera.main.ScreenToWorldPoint(new Vector3(item.target.position.x, item.target.position.y, 10f));
            }
           
        }

        /// <summary>
        /// Loads the levelToLoad
        /// </summary>
        public void StartGame()
        {
            SceneManager.LoadScene(_levelToLoad);
        }

        /// <summary>
        /// Shows the credit panel
        /// </summary>
        public void ShowCredits()
        {
            _creditsPanel.SetActive(true);
            _mainMenuPanel.SetActive(false);
            _confirmationPanel.SetActive(false);
        }

        /// <summary>
        /// Returns to the main panel
        /// </summary>
        public void ReturnToMainPanel()
        {
            _creditsPanel.SetActive(false);
            _mainMenuPanel.SetActive(true);
            _confirmationPanel.SetActive(false);
        }
        public void ClearPlayerPrefs()=>PlayerPrefs.DeleteAll();
    }


    /// <summary>
    /// Class to link the object to set to the target position.
    /// </summary>
    [Serializable]
    public class ScreenTargets
    {
        public Transform target;
        public Transform objectToSet;
    }
}

