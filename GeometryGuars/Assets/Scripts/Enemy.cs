using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Unit
{
    [Header("Scoring")]
    [SerializeField] private int _socreValue = 100; //Score added on death
    [SerializeField] private float _difficulty = 1f;

    public float Difficulty => _difficulty;

    //The player the enemy is chasing
    private Player _player;

    // Start is called before the first frame update
    protected override void Start()
    {
        //call the base start method
        base.Start();

        //Find player method 1 (tags)
        //_player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

        //find player method 2 (type)
        _player = FindObjectOfType<Player>();
    }

    // Update is called once per frame
    void Update()
    {
         //Destroy if player is null
        if(_player == null){
            Death();
            return;
        }

        Vector3 myPosition = transform.position;
        Vector3 playerPosition = _player.transform.position;

        //direction to plkayer
        Vector3 dirToPlayer = (playerPosition - myPosition).normalized;

        //Move towards player
        MoveInDirection(dirToPlayer, Time.deltaTime);
    }

    //Override death function
    protected override void Death(){
        //Add score
        GameManager.Instance.AddScore(_socreValue);
        base.Death();
    }
}
