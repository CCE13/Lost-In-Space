using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Managers
{
    public class Level : MonoBehaviour
    {
        [SerializeField] private Transform _starHolder;
        [SerializeField] private Color _locked;
        [SerializeField] private Color _unlocked;
        private TMP_Text _levelNumber;

        private bool _levelUnlocked;
        private Button _button;
        private TMP_Text _levelText;
        private int _starCount;
        // Start is called before the first frame update
        private void OnValidate()
        {
            int level = transform.GetSiblingIndex() + 1;
            _levelNumber = GetComponentInChildren<TMP_Text>();
            _levelNumber.text = level.ToString();
            name = $"Level {level}";
        }
        private void Awake()
        {
            _button = GetComponent<Button>();
            _levelText = GetComponentInChildren<TMP_Text>();
            _button.onClick.AddListener(() => EnterLevel());
        }
        private void OnDestroy()
        {
            _button.onClick.RemoveAllListeners();
        }


        /// <summary>
        /// Checks if the level has been unlocked
        /// </summary>
        void Start()
        {
            PlayerPrefs.SetInt("Level 1", 1);
            //checks if the level is unlocked
            if (PlayerPrefs.GetInt(name) == 0)
            {
                _levelUnlocked = false;
                Locked();
            }
            else
            {
                _levelUnlocked = true;
                Unlocked();
            }

            CheckStarCount();
        }

        /// <summary>
        /// Loads the level based on the gameobject name
        /// </summary>
        public void EnterLevel()
        {
            if (!_levelUnlocked) return;
            SceneManager.LoadScene(name);
        }

        /// <summary>
        /// Locks the level
        /// </summary>
        private void Locked()
        {
            _button.interactable = false;
            _levelText.color = _locked;
        }

        /// <summary>
        /// Unlocks the level
        /// </summary>
        private void Unlocked()
        {
            _button.interactable = true;
            _levelText.color = _unlocked;
        }


        /// <summary>
        /// Checks the star count of the level and displays it on the UI
        /// </summary>
        private void CheckStarCount()
        {
            _starCount = PlayerPrefs.GetInt(name + "StarsCollected");
            if (_starCount == 0)
            {
                for (int i = 0; i < _starHolder.childCount; i++)
                {
                    _starHolder.GetChild(i).GetComponent<Image>().color = new Color(106 / 255f, 106 / 255f, 106 / 255f, 1f);
                }
                return;
            }
            for (int i = 0; i < _starCount; i++)
            {
                _starHolder.GetChild(i).GetComponent<Image>().color = Color.white;
            }

        }
    }
}

