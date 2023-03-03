using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


namespace Collection
{
    public class VelocityChanger : MonoBehaviour, ICollectable,IResetable
    {
        [field: SerializeField] public float Multiplier { get; private set; }
        public static event Action<float> ChangeVelocity;

        private GameObject _boost;
        private void Start()
        {
            _boost = transform.GetChild(0).gameObject;
        }

        /// <summary>
        /// Changes the players velocity
        /// </summary>
        public void Collected()
        {
            ChangeVelocity?.Invoke(Multiplier);
            _boost.SetActive(false);
        }

        /// <summary>
        /// Reset the settings for the object
        /// </summary>
        public void OnReset()
        {
            _boost.SetActive(true);
        }
    }
}

