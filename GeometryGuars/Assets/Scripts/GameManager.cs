using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //Corners used to determine enemy spawn positions
    [SerializeField] private Vector3[] _spawnCorners;

    //Enemy prefans
    [SerializeField] private Enemy[] _enemies = new Enemy[0];

    //Time between enemies
    [SerializeField] private float _spawnTick  = 1f;

    [Header("Difficulty")]
    [SerializeField] private AnimationCurve _difficultyIncrease;
    [SerializeField] private float _targetDifficulty = 1f;
    [SerializeField] private float _currentDifficulty = 0;
    [SerializeField] private float _safeSpawnDistance = 10f;

    [Header("GameOver")]
    [SerializeField] private GameObject _gameOverScreen;
    [SerializeField] private AudioSource _music;

    //Total score
    public int Score{get; private set;}

    //singleton
    private static GameManager _instance;
    public static GameManager Instance => _instance;

    private Player _player;

    // Start is called before the first frame update
    void Start()
    {
        if(_instance != null){
            Destroy(gameObject);
            return;
        }
        Debug.LogWarning("Duplicate singleton of type GameManager created, destoying!");
        _instance = this;
        _player = FindObjectOfType<Player>();
        InvokeRepeating("SpawnRandomEnemy", 1f, _spawnTick);
    }

    private void SpawnRandomEnemy(){
        //if player us null, stop
        if(_player == null){
            CancelInvoke("SpawnRandomEnemy");
            GameOver();
            return;
        }

        //Skip if difficulty is too high
        if(_currentDifficulty > _targetDifficulty){
            return;
        }

        //pick a random enemy to spawn
        Enemy toSpawn = _enemies[Random.Range(0, _enemies.Length)];

        //Add to current difficulty
        _currentDifficulty += toSpawn.Difficulty;

        //find a spawn point
        Vector3 spawnPoint = GetSpawnPoint();

        for (int i = 0; i < 20; i++)
        {
            //Breaj out of loop if spawnPoint is safe
            if(Vector3.Distance(spawnPoint, _player.transform.position) > _safeSpawnDistance){
                break;
            }
            //Find a new random spawn point
            spawnPoint = GetSpawnPoint();
        }

        //Spawn the enemy
        Instantiate(toSpawn, spawnPoint, Quaternion.identity);
    }

    //This executes when the gameObject is selected
    private void OnDrawGizmosSelected()
    {
        for (int i = 0; i < _spawnCorners.Length; i++)
        {
            Gizmos.DrawWireSphere(_spawnCorners[i], 0.5f);
            Gizmos.color = Color.green;

        }
    }

    //find a random spawn point along corners
    private Vector3 GetSpawnPoint(){

        //Find total length of corners
        float totalLength = 0f;
        for (int i = 0; i < _spawnCorners.Length; i++)
        {
            Vector3 currentCorner = _spawnCorners[i];
            int nextCornerIndex = i+1;
            if(nextCornerIndex >= _spawnCorners.Length) nextCornerIndex = 0;
            Vector3 nextCorner = _spawnCorners[nextCornerIndex];

            float segmentLength = Vector3.Distance(currentCorner, nextCorner);
            totalLength += segmentLength;
        }
        //generate length for spawn position
        float spawnPositionLenght = totalLength * Random.value;

        //Find spawn position alogn length
        for (int i = 0; i < _spawnCorners.Length; i++)
        {
             Vector3 currentCorner = _spawnCorners[i];
            int nextCornerIndex = i+1;
            if(nextCornerIndex >= _spawnCorners.Length) nextCornerIndex = 0;
            Vector3 nextCorner = _spawnCorners[nextCornerIndex];

            float segmentLength = Vector3.Distance(currentCorner, nextCorner);

            if(spawnPositionLenght > segmentLength){
                spawnPositionLenght -= segmentLength;
                continue;
            }
            //Direction nof segment
            Vector3 segmentDirection = (nextCorner - currentCorner).normalized;
            //Point on segment
            Vector3 point = currentCorner + segmentDirection * spawnPositionLenght;
            return point;
        }
        return _spawnCorners[0];
    }
    public void AddScore(int amount){
        Score += amount;
    }
    

    // Update is called once per frame
    void Update()
    {
        //get current difficulty increase
        float currentDifficultyIncrease = _difficultyIncrease.Evaluate(Time.timeSinceLevelLoad) * Time.deltaTime;
        //add to target difficulty
        _targetDifficulty += currentDifficultyIncrease;
    }
    private void GameOver(){
        //StartCoroutine("GameOverRoutine");
        StartCoroutine(GameOverRoutine());


    }

    private IEnumerator GameOverRoutine(){
        //wait ls before continuing
        _music.Stop();
        yield return new WaitForSeconds(1.5f);

        

        //enable game over screen
        _gameOverScreen.SetActive(true);
    }
}
