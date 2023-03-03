using System.Collections;
using UnityEngine;
using Managers;
using System;

namespace Teleportation
{
    public class Teleporter : MonoBehaviour
    {
        [field: SerializeField] public GameObject TeleportorLinkedTo { get; private set; }
        [SerializeField] private bool _activated;
        [SerializeField] private float minVelocity;
        [SerializeField] private float maxVelocity;
        [SerializeField] private bool canEdit;
        [SerializeField] private AudioClip _teleportingSFX;
        [field: SerializeField] public Collider2D TriggerCollider { get; private set; }
        private ParticleSystem _particles;
        private SpriteRenderer _spriteRenderer;
        private void Start()
        {
            _particles = GetComponentInChildren<ParticleSystem>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
            GameManager.Instance?.AddTeleporter(this);

        }

        /// <summary>
        /// Checks if the <paramref name="collision"/> has a ITeleportable interface.
        /// Sends the <paramref name="collision"/> game object to the linked teleporter and transfers the velocity based on the rotation of the linked object.
        /// </summary>
        /// <param name="collision"></param>
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (!_activated) return;
            if (collision.GetComponent<ITeleporable>() != null)
            {
                var teleporterCollider = TeleportorLinkedTo.GetComponent<Teleporter>().TriggerCollider;
                teleporterCollider.enabled = false;
                Rigidbody2D ballRb = collision.attachedRigidbody;
                float enterVelocity = ballRb.velocity.magnitude;
                ballRb.velocity = enterVelocity < minVelocity ? minVelocity * -TeleportorLinkedTo.transform.up : enterVelocity * -TeleportorLinkedTo.transform.up;
                ballRb.velocity = Vector2.ClampMagnitude(ballRb.velocity, maxVelocity);
                collision.transform.position = TeleportorLinkedTo.transform.Find("Release Point").position;
                AudioManager.instance.PlayEffect(_teleportingSFX,1f);
                collision.GetComponent<TrailRenderer>()?.Clear();
                StartCoroutine(EnableColliderDelay(teleporterCollider));
                
            }
        }

        /// <summary>
        /// Enables the collider after a delay.
        /// </summary>
        /// <param name="teleporterCollider"></param>
        /// <returns></returns>
        private IEnumerator EnableColliderDelay(Collider2D teleporterCollider)
        {
            yield return new WaitForSeconds(0.2f);
            teleporterCollider.enabled = true;
        }

        /// <summary>
        /// Activates the teleportor
        /// </summary>
        public void ActivateTeleporter()
        {
            _activated = true;
        }

        /// <summary>
        /// Links the teleportor
        /// </summary>
        /// <param name="teleporterToLinkTo"></param>
        public void LinkTeleporter(GameObject teleporterToLinkTo)
        {
            TeleportorLinkedTo = teleporterToLinkTo;
        }

        /// <summary>
        /// Sets the color of the teleportor and the particle effect related to it.
        /// </summary>
        /// <param name="color"></param>
        public void SetColor(Color color)
        {
            var particleSystem = _particles.main;
            _particles.Stop();
                particleSystem.startColor = color;
            _particles.Play();
            _spriteRenderer.color = color;
        }

        /// <summary>
        /// Moves the teleportor on mouse drag.
        /// </summary>
        private void OnMouseDrag()
        {
            if (!canEdit) return;
            BuildManager.Instance.SetCurrentEditingObject(gameObject);
            BuildManager.Instance.SetCurrentlyMovingObject(gameObject);
            
        }

        /// <summary>
        /// Places the teleportor at where the player last released his finger.
        /// </summary>
        private void OnMouseUp()
        {
            if (!canEdit) return;
            BuildManager.Instance.SetCurrentlyMovingObject(null);

        }
    }
}

