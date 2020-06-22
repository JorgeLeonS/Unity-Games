using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Chomper : Unit
{
    [Header("AI")]
    [SerializeField] private float _attackDistance = 2f;

    private Player _player;
    private NavMeshAgent _navMeshAgent;

    // Start is called before the first frame update
    void Start()
    {
        _player = FindObjectOfType<Player>();
        _navMeshAgent = GetComponent<NavMeshAgent>();

        _navMeshAgent.speed = _moveVelocity;
        _navMeshAgent.acceleration = _moveVelocity * 2f;
        _navMeshAgent.angularSpeed = 1080;

        _navMeshAgent.updatePosition = false;
        _navMeshAgent.updateRotation = false;
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        /* 
        //Dumb AI
        Vector3 vectorToPlayer = _player.transform.position - transform.position;
        MoveInput = vectorToPlayer;
        */

        if(!IsAlive) return;

        //navigation AI
        //Chase player if distance> attack distance
        float navMeshAgentDistance = Vector3.Distance(_player.transform.position, _navMeshAgent.nextPosition);
        if(navMeshAgentDistance > _attackDistance){
            _navMeshAgent.isStopped = false;
            _navMeshAgent.SetDestination(_player.transform.position);
        }else{
            _navMeshAgent.isStopped = true;
            TryAtttack();
        }

        //Move character to navMeshAgent
        Vector3 vectorToNavMeshAgent = _navMeshAgent.nextPosition - transform.position;
        MoveInput = vectorToNavMeshAgent;
    }
}
