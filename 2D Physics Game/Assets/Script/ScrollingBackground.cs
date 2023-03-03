using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
namespace UI
{
    public class ScrollingBackground : MonoBehaviour
    {
        public static ScrollingBackground instance;

        private RawImage _image;
        [SerializeField] private float _x, _y;

        /// <summary>
        /// Sets this object  to a singleton
        /// </summary>
        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(transform.parent.gameObject);
            }
            else
            {
                Destroy(transform.parent.gameObject);
            }

        }

        private void Start()
        {
            _image = GetComponent<RawImage>();
            SceneManager.sceneLoaded += OnSceneChange;
            Application.targetFrameRate = 60;
            

        }

        private void OnDestroy()
        {
            SceneManager.sceneLoaded -= OnSceneChange;
        }

        private void OnSceneChange(Scene scene, LoadSceneMode mode)
        {
            GetComponentInParent<Canvas>().worldCamera = Camera.main;
        }
        private void Update()
        {
            if (PauseMenuController.Paused) return;
            _image.uvRect = new(_image.uvRect.x + _x, 0f, Screen.width/1920f, 1f);
        }
    }
}

