using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Collection;
using Teleportation;
using Managers;

namespace Player
{
    public class PlayerController : MonoBehaviour,ITeleporable,IResetable
    {
        private Rigidbody2D _rb;
        private Collider2D _collider;

        private Vector2 _startPosition;
        private TrailRenderer _trail;

        public bool inMainMenu;
        private void Awake()
        {
            _collider = GetComponent<Collider2D>();
            _rb = GetComponent<Rigidbody2D>();
            _trail = GetComponent<TrailRenderer>();
            _startPosition = transform.position;
        }
        private void Start()
        {
            VelocityChanger.ChangeVelocity += VelocityChange;
            GameManager.Simulated += StartSimulation;
            if (inMainMenu) { return; }
            _rb.gravityScale = 0f;
            _collider.enabled = false;
        }
        private void OnDestroy()
        {
            VelocityChanger.ChangeVelocity -= VelocityChange;
            GameManager.Simulated -= StartSimulation;
        }
        /// <summary>
        /// Setup when the simulation starts
        /// </summary>
        private void StartSimulation()
        {
            _rb.gravityScale = 1f;
            _collider.enabled = true;
        }
        /// <summary>
        /// Changes the velocity based on the <paramref name="multiplier"/>
        /// </summary>
        /// <param name="multiplier"></param>
        private void VelocityChange(float multiplier)
        {
            if (multiplier == 0) return;
            _rb.velocity *= multiplier;
        }


        public void OnReset()
        {
            StartCoroutine(ResetDelay());
        }
        /// <summary>
        /// Resets the player to its original position and settings with a delay
        /// </summary>
        private IEnumerator ResetDelay()
        {
            _rb.constraints = RigidbodyConstraints2D.FreezeAll;
            _rb.gravityScale = 0f;
            _trail.Clear();
            _collider.enabled = false;
            transform.position = _startPosition;
            transform.rotation = Quaternion.identity;
            _rb.velocity = Vector2.zero;
            _rb.angularVelocity = 0f;
            yield return new WaitForSeconds(0.8f);
            _rb.constraints = RigidbodyConstraints2D.None;
        }
    }
}


