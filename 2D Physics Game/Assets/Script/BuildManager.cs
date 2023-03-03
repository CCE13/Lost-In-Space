using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UI;
using System.Runtime.CompilerServices;

namespace Managers
{
    /// <summary>
    /// Controls the building of teleporters and the editing of teleporters
    /// </summary>
    public class BuildManager : MonoBehaviour,IResetable
    {
        public static BuildManager Instance;

        [SerializeField]private GameObject _currentlyEditing;
        [SerializeField] private GameObject _currentlyMoving;
        [SerializeField] private bool canEdit = true;
        [SerializeField] private AudioClip _spawnPortalSFX;

        public static event Action<GameObject> IsEditing;

        private bool _offsetSet;
        private Vector3 _offset;
        

        private void Awake()
        {
            Instance = this;
        }
        private void Start()
        {
            GameplayController.FinishEditing += FinishEditing;
            GameManager.Simulated += ()=> canEdit = false;

        }
        private void OnDestroy()
        {
            GameplayController.FinishEditing -= FinishEditing;
            GameManager.Simulated -= () => canEdit = false;
        }

        /// <summary>
        /// Spawns the teleportor
        /// </summary>
        /// <param name="objectToSpawn"></param>
        public void SpawnTeleportor(GameObject objectToSpawn)
        {
            if (!GameManager.Instance.CanAddTeleporter())
            {
                Debug.Log("cannot put teleporters anymore");
                return;
            }
            GameObject teleporterToSpawn = Instantiate(objectToSpawn, Vector3.zero, Quaternion.Euler(0,0,180));
            SetCurrentEditingObject(teleporterToSpawn);
            AudioManager.instance.PlayEffect(_spawnPortalSFX, 0.5f);

            
        }
        private void Update()
        {
            if (!_currentlyEditing) return;
            if (!canEdit) return;
            if (_currentlyMoving)
            {
                Follow(Input.mousePosition);
            }
                
        }

        /// <summary>
        /// Follows the finger press when there is a teleportor to move
        /// </summary>
        /// <param name="pos"></param>
        private void Follow(Vector2 pos)
        {
            Vector3 position = Camera.main.ScreenToWorldPoint(pos);
            if (!_offsetSet)
            {
                _offset = _currentlyEditing.transform.position - new Vector3(position.x, position.y, 0f);
                _offsetSet = true;
            }
            _currentlyEditing.transform.position = new Vector3(position.x, position.y, 0f) + _offset;
        }

        /// <summary>
        /// Sets thwe current editing object to whatever object the player has clicked on
        /// </summary>
        /// <param name="obj"></param>
        public void SetCurrentEditingObject(GameObject obj)
        {
            if (!canEdit) return;
            _currentlyEditing = obj;
            IsEditing?.Invoke(_currentlyEditing);
        }

        /// <summary>
        /// Sets the current moving object  to the object the player is dragging.
        /// </summary>
        /// <param name="obj"></param>
        public void SetCurrentlyMovingObject(GameObject obj)
        {
            if (!canEdit) return;
            _currentlyMoving = obj;
            if(!_currentlyMoving) _offsetSet = false;
            IsEditing?.Invoke(_currentlyEditing);
        }


        /// <summary>
        /// Closes the editing panel and resets the current editing and moving object to <see langword="null"/>.
        /// </summary>
        private void FinishEditing()
        {
            SetCurrentEditingObject(null);
            SetCurrentlyMovingObject(null);
            _offsetSet = false;
        }


        /// <summary>
        /// Enables editing on reset.
        /// </summary>
        public void OnReset()
        {
            canEdit = true;
        }
    }

}
