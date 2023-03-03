using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Managers;
using TMPro;
using Teleportation;
using System;
using UnityEngine.UI;

namespace UI
{
    public class GameplayController : UIController
    {
        [SerializeField] private Transform _starHolder;
        [SerializeField] private TMP_Text _timeText;
        [SerializeField] private GameObject _editor;

        private int _minPassed;
        private int _secondsPassed;
        private GameObject _objectEditing;
        private GameObject _panel;


        public static event Action FinishEditing;
        private void Awake()
        {
            _panel = transform.GetChild(0).gameObject;
        }
        private void Start()
        {
            
            SubscribeToEvents();
            ResetStarUI();
        }
        private void OnDestroy()
        {
            UnsubscribeToEvents();
        }

        /// <summary>
        /// Starts the simulation
        /// </summary>
        public void StartGame()
        {
            GameManager.Instance.StartSimulation();

        }

        /// <summary>
        /// Resets objects to the original position
        /// </summary>
        public void ResetGame()
        {
            GameManager.Instance.ResetSimulation();
        }
        #region EventSubscription
        /// <summary>
        /// Subscribe to events;
        /// </summary>
        private void SubscribeToEvents()
        {
            GameManager.UpdateStarUI += UpdateStarUI;
            GameManager.UpdateTimePassed += UpdateTimeUI;
            BuildManager.IsEditing += Editing;
            GameManager.Simulated += StopEditing;
            EndGame.GameEnded += DisablePanel;
        }

        /// <summary>
        /// Unsubscribe to evets
        /// </summary>
        private void UnsubscribeToEvents()
        {
            GameManager.UpdateStarUI -= UpdateStarUI;
            GameManager.UpdateTimePassed -= UpdateTimeUI;
            BuildManager.IsEditing -= Editing;
            GameManager.Simulated -= StopEditing;
            EndGame.GameEnded -= DisablePanel;
        }

        #endregion

        #region StarUI

        /// <summary>
        /// Resets the Star UI
        /// </summary>
        private void ResetStarUI()
        {
            for (int i = 0; i < _starHolder.childCount; i++)
            {
                _starHolder.GetChild(i).gameObject.SetActive(false);
            }
        }

        /// <summary>
        /// Updates the star ui based on the number of stars collected
        /// </summary>
        /// <param name="starCount"></param>
        private void UpdateStarUI(int starCount)
        {
            if(starCount == 0)
            {
                ResetStarUI();
                return;
            }
            _starHolder.Find($"Star {starCount}")?.gameObject.SetActive(true);
        }
        #endregion

        #region TimePassed
        private void UpdateTimeUI(float currentTime)
        {
            _secondsPassed = (int)Mathf.Floor(currentTime % 60);
            _minPassed = (int)Mathf.Floor(currentTime / 60);
            _timeText.text = $"{_minPassed.ToString("00")}:{_secondsPassed.ToString("00")}";
        }
        #endregion

        #region EditObject


        /// <summary>
        /// Set the currently editing object to <paramref name="objEditing"/>
        /// </summary>
        /// <param name="objEditing"></param>
        private void Editing(GameObject objEditing)
        {
            _objectEditing = objEditing;
            if (_objectEditing == null) return;
            _editor.SetActive(true);
            _editor.GetComponentInChildren<Slider>().value = _objectEditing.transform.eulerAngles.z;
        }

        /// <summary>
        /// Rotates the current editing object by <paramref name="amount"/>
        /// </summary>
        /// <param name="amount"></param>
        public void RotateEditingObject(float amount)
        {
            _objectEditing.transform.rotation = Quaternion.Euler(0, 0, amount);
        }

        /// <summary>
        /// Stops the object editing and sets the current object editing to null
        /// </summary>
        public void StopEditing()
        {
            _objectEditing = null;
            FinishEditing?.Invoke();
            _editor.SetActive(false);
        }
        #endregion

        /// <summary>
        /// Disables the gameplay panel
        /// </summary>
        private void DisablePanel()
        {
            _panel?.SetActive(false);
        }
    }
}

