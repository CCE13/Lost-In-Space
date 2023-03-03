using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Managers
{
    public class EndGame : MonoBehaviour
    {
        public static event Action GameEnded;

        /// <summary>
        /// Calls the GameEnded event and stops the player from moving
        /// </summary>
        /// <param name="collision"></param>
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                collision.attachedRigidbody.constraints = RigidbodyConstraints2D.FreezeAll;
                GameEnded?.Invoke();
                GameManager.S_SIMULATING = false;
            }
        }
    }
}


