using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Player _playerPrefab;
    [SerializeField] private int _playerCount = 2;
    [SerializeField] private Color[] _playerColors = new Color[0];

    private int _playerIDCounter;

    // Start is called before the first frame update
    void Start()
    {
        //Find spawn points in level
        GameObject[] spawnPoints = GameObject.FindGameObjectsWithTag("SpawnPoint");

        //Find max number of players allowed
        int maxPlayers = Mathf.Min(_playerCount, spawnPoints.Length);
        Debug.Log("maxplayers: " + maxPlayers);

        //Spawn players
        for (int i = 0; i < maxPlayers; i++)
        {
            Vector2 spawnPosition = spawnPoints[i].transform.position;
            int playerID = _playerIDCounter++;
            Color playerColor = _playerColors[i];

            //Instantiate new player
            Player newPlayer = Instantiate(_playerPrefab, spawnPosition, Quaternion.identity);
            newPlayer.Initialized(playerID, playerColor);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
