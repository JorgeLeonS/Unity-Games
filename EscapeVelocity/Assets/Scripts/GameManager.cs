using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    [Header("Difficulty")]

    [SerializeField] private float _gameSpeed = 2f;

    [SerializeField] private float _speedIncrease = 0.05f;

    [Header("Asteroid Spawning")]
    [SerializeField] private float _asteroidSpawnDistance = 3f;

    [SerializeField] private Rigidbody2D[] _asteroidPrefabs;

    [SerializeField] private float _spamWidth = 3f;

    public float TotalDistance => _totalDistance;

    private float _lastAsteroidDistance;
    private float _totalDistance;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //increae game speed
        _gameSpeed += _speedIncrease * Time.deltaTime;

        //add to total distance
        _totalDistance += _gameSpeed * Time.deltaTime;

        // check if we can spawn an asteroid
        if(_totalDistance > _lastAsteroidDistance + _asteroidSpawnDistance){
            //set current spawn distance
            _lastAsteroidDistance = _totalDistance;

            //sekect random asteroid prefab
            Rigidbody2D selectedAsteroidPrefab = _asteroidPrefabs[Random.Range(0, _asteroidPrefabs.Length)];

            //find random spawn position
            Vector3 spawnPosition = transform.position + new Vector3(Random.Range(-_spamWidth, _spamWidth), 0f,0f);

            //Instantiate the selected prefab
            Rigidbody2D spawnedAsteroid = Instantiate(selectedAsteroidPrefab);
            spawnedAsteroid.transform.position = spawnPosition;
            spawnedAsteroid.velocity = new Vector2(0f, -_gameSpeed);
        }


    }
}
