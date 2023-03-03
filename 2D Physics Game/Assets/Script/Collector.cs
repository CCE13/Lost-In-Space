using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Collection
{
    public class Collector : MonoBehaviour
    {
        /// <summary>
        /// Checks if the <paramref name="collision"/> gameobject contains the ICollectabl Interface and calls the Collected function if there is.
        /// </summary>
        /// <param name="collision"></param>
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.GetComponent<ICollectable>() != null)
            {
                collision.GetComponent<ICollectable>().Collected();
            }
        }
    }
}

