using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Teleportation;
using System;
using Collection;
using UI;
using UnityEngine.SceneManagement;
using System.Text.RegularExpressions;

namespace Managers
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;

        [Header("Teleportation")]
        [SerializeField] private List<Teleporter> _teleporters;
        [SerializeField] private List<Color> _teleporterColors;
        [SerializeField] private int _maxTeleporters;

        [Header("Stars")]
        [SerializeField] private int _starsCollected;
        [SerializeField] private AudioClip _starSFX;
        public int StarsCollected
        {
            get { return _starsCollected; }
        }

        public int TeleportersLeft
        {
            get { return _maxTeleporters - _teleporters.Count; }
        }

        public int CurrentLevel { get; private set; }


        private float _timePassed;
        public static event Action<int> UpdateStarUI;
        public static event Action<float> UpdateTimePassed;
        public static event Action Simulated;
        public static event Action<int> UpdateTeleporterCountUI;

        public static bool S_SIMULATING;
        private void Awake()
        {
            Instance = this;
        }
        private void Start()
        {
            SubscribeToEvents();
            CurrentLevel = int.Parse(Regex.Replace(SceneManager.GetActiveScene().name, "[^0-9]", ""));
            Debug.Log(CurrentLevel);
            
        }
        private void OnDestroy()
        {
            UnsubscribeToEvents();
        }

        private void Update()
        {
            if (PauseMenuController.Paused) return;
            _timePassed += Time.unscaledDeltaTime;
            UpdateTimePassed?.Invoke(_timePassed);
        }
        #region EventSubscription
        private void SubscribeToEvents()
        {
            StarCollectable.CollectedStar += AddStars;
        }
        private void UnsubscribeToEvents()
        {
            StarCollectable.CollectedStar -= AddStars;
        }
        #endregion

        /// <summary>
        /// Starts the simulation
        /// </summary>
        public void StartSimulation()
        {
            Simulated?.Invoke();
            S_SIMULATING = true;
        }

        /// <summary>
        /// Resets the simulation
        /// </summary>
        public void ResetSimulation()
        {
            foreach (var item in FindObjectsOfType<Resetable>())
            {
                item.GetComponent<IResetable>().OnReset();
            } 
            ResetStarCount();
            S_SIMULATING = false;
        }

        /// <summary>
        /// Adds 1 to the star count and calls an event to update the ui
        /// </summary>
        private void AddStars()
        {
            _starsCollected++;
            UpdateStarUI?.Invoke(_starsCollected);
            float pitch = _starsCollected > 1 ? 1 + (_starsCollected / 3) : 1;
            AudioManager.instance.PlayEffect(_starSFX, pitch,0.5f);
        }

        /// <summary>
        /// Resets the star count to 0
        /// </summary>
        private void ResetStarCount()
        {
            _starsCollected = 0;
            UpdateStarUI?.Invoke(_starsCollected);
        }

        #region Teleportation
        /// <summary>
        /// Adds the teleportor spawned in to the list and updates the teleportor count
        /// </summary>
        /// <param name="teleporter"></param>
        public void AddTeleporter(Teleporter teleporter)
        {
            _teleporters.Add(teleporter);
            LinkTeleporters();
            UpdateTeleporterCountUI?.Invoke(TeleportersLeft);
        }

        /// <summary>
        /// Link the teleportors to their respective teleportors based on the list position
        /// </summary>
        private void LinkTeleporters()
        {
            for (int i = 0; i < _teleporters.Count; i += 2)
            {
                var teleporter1 = _teleporters[i];
                if (i + 1 == _teleporters.Count)
                {
                    teleporter1.SetColor(Color.white);
                    Debug.Log("No Connecting Teleporter");
                    break;
                }
                var teleporter2 = _teleporters[i + 1];
                teleporter1.LinkTeleporter(_teleporters[i + 1].gameObject);
                teleporter2.LinkTeleporter(_teleporters[i].gameObject);


                teleporter1.ActivateTeleporter();
                teleporter2.ActivateTeleporter();

                switch (i)
                {
                    case 0:
                        teleporter1.SetColor(_teleporterColors[i]);
                        teleporter2.SetColor(_teleporterColors[i]);
                        break;
                    default:
                        teleporter1.SetColor(_teleporterColors[i / 2]);
                        teleporter2.SetColor(_teleporterColors[i / 2]);
                        break;
                }

            }
        }
        /// <summary>
        ///Checks if the number of teleportors in the list is more than the max teleportor threshhold
        /// </summary>
        /// <returns></returns>
        public bool CanAddTeleporter()
        {
            return _teleporters.Count < _maxTeleporters;
        }
        #endregion
    }
}

