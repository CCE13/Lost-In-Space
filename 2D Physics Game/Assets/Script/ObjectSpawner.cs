using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


namespace Managers
{
    public class ObjectSpawner : MonoBehaviour
    {
        [SerializeField] private GameObject _objectToSpawn;

        private TMP_Text _textCount;

        private void Awake()
        {
            _textCount = GetComponentInChildren<TMP_Text>();
        }
        private void Start()
        {
           UpdateTeleporterLeft(GameManager.Instance.TeleportersLeft);
            GameManager.UpdateTeleporterCountUI += UpdateTeleporterLeft;
        }
        private void OnDestroy()
        {
            GameManager.UpdateTeleporterCountUI -= UpdateTeleporterLeft;
        }

        /// <summary>
        /// Spawns the object to spawn
        /// </summary>
        public void SpawnObject()
        {
            if(GameManager.S_SIMULATING) { return; }
            BuildManager.Instance.SpawnTeleportor(_objectToSpawn);
        }

        /// <summary>
        /// Updates the teleportor count
        /// </summary>
        /// <param name="count"></param>
        private void UpdateTeleporterLeft(int count)
        {
            _textCount.text = count.ToString();
        }
    }
}

