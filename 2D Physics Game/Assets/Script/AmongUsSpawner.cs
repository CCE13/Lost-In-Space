using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Managers
{
    public class AmongUsSpawner : MonoBehaviour
    {
        public static AmongUsSpawner instance;
        [SerializeField] private float _minXSpawnCoordinate;
        [SerializeField] private float _minYSpawnCoordinate;
        [SerializeField] private float _maxYSpawnCoordinate;

        [SerializeField] private List<GameObject> _amongus;
        [SerializeField] private float speed;

        [SerializeField] private float _timePerSpawn;
        private float _ogTime;
        private static int index;


        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
        private void Start()
        {
            index = Random.Range(0, _amongus.Count);
            _ogTime = _timePerSpawn;
        }
        // Start is called before the first frame update
        private void Update()
        {
            _timePerSpawn -= Time.deltaTime;
            if (_timePerSpawn < 0)
            {
                Spawn();
            }

        }
        /// <summary>
        /// Takes a random object from the list and spawns it at a random x y position.
        /// Moves the object across the screen.
        /// </summary>
        private void Spawn()
        {
            if (index > _amongus.Count - 1) index = Random.Range(0, _amongus.Count - 1);
            _amongus[index].transform.position = new Vector2(_minXSpawnCoordinate, Random.Range(_minYSpawnCoordinate, _maxYSpawnCoordinate));
            var rb = _amongus[index].GetComponent<Rigidbody2D>();
            rb.velocity = Vector2.left * speed;
            rb.angularVelocity = -speed * 20f;
            _timePerSpawn = _ogTime;
            index++;
        }
    }
}

