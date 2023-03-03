using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Managers;


namespace UI
{
    public class GameEndController : UIController
    {
        [SerializeField] private GameObject _gameEndScreen;
        [SerializeField] private GameObject _nextLevelButton;

        private string _sceneToLoad;

        // Start is called before the first frame update
        void Start()
        {
            _gameEndScreen.SetActive(false);
            EndGame.GameEnded += GameEnded;
            
        }
        private void OnDestroy()
        {
            EndGame.GameEnded -= GameEnded;
        }
        /// <summary>
        /// Calls when the game is ended. Plays an animation for the game end panel.
        /// </summary>
        private void GameEnded()
        {

            StartCoroutine(AnimationDeley());
            _sceneToLoad = $"Level {GameManager.Instance.CurrentLevel + 1}";
            
            if (GameManager.Instance.StarsCollected == 0 && PlayerPrefs.GetInt(_sceneToLoad) == 0)
            {
                _nextLevelButton.SetActive(false);
            }
            else
            {
                _nextLevelButton.SetActive(true);
            }
            if(!Application.CanStreamedLevelBeLoaded(_sceneToLoad)) _nextLevelButton.SetActive(false);
            UnlockNextLevel(_sceneToLoad);
            SetStarCollected($"Level {GameManager.Instance.CurrentLevel}");

        }

        /// <summary>
        /// Loads the next level;
        /// </summary>
        public void NextLevel()
        {
            SceneManager.LoadScene(_sceneToLoad);
        }
        /// <summary>
        /// Delay to play to animation.
        /// </summary>
        /// <returns></returns>
        private IEnumerator AnimationDeley()
        {
            yield return new WaitForSeconds(1f);
            _gameEndScreen.SetActive(true);
            yield return new WaitForSeconds(1f);
            for (int i = 1; i <= GameManager.Instance.StarsCollected; i++)
            {
                _gameEndScreen.transform.Find($"Star {i}").GetChild(0).gameObject.SetActive(true);
                yield return new WaitForSeconds(0.3f);
            }
        }

        /// <summary>
        /// Checks if the player has collected any stars and unlocks the next level if they did.
        /// </summary>
        /// <param name="level"></param>
        private void UnlockNextLevel(string level)
        {
            if (GameManager.Instance.StarsCollected != 0)
            {
                PlayerPrefs.SetInt(level, 1);
            }

            
        }

        /// <summary>
        /// Sets the number of stars collected in the game end panel
        /// </summary>
        /// <param name="level"></param>
        private void SetStarCollected(string level)
        {
            int bestStars = PlayerPrefs.GetInt(level + "StarsCollected");

            if(GameManager.Instance.StarsCollected > bestStars)
            {
                PlayerPrefs.SetInt(level + "StarsCollected", GameManager.Instance.StarsCollected);
            }

        }
    }
}

