using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Managers;

namespace Collection
{
    public class StarCollectable : MonoBehaviour, ICollectable,IResetable
    {
        public static event Action CollectedStar;
        [SerializeField] private Gradient _rgb;

        private SpriteRenderer _renderer;
        private GameObject _star;
        private CircleCollider2D _triggerCollider;


        private void Awake()
        {
            _renderer = GetComponentInChildren<SpriteRenderer>();
            _triggerCollider = GetComponent<CircleCollider2D>();
        }
        private void Start()
        {

            _star = transform.GetChild(0).gameObject;
        }

        /// <summary>
        /// Collects the star
        /// </summary>
        public void Collected()
        {
            _star.SetActive(false);
            _triggerCollider.enabled = false;
            CollectedStar?.Invoke();
        }

        /// <summary>
        /// Reset the settings for the star
        /// </summary>
        public void OnReset()
        {
            _star.SetActive(true);
            _triggerCollider.enabled = true;
        }

        private void Update()
        {
            _renderer.color = _rgb.Evaluate(Mathf.PingPong(Time.unscaledTime/3 ,1f));
        }

    }
}

