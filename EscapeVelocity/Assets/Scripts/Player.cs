using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{

    //ship acceleration
    [SerializeField] private float _acceleration = 5f;

    //ship shields
    [SerializeField] private int _shieldCount = 2;

    [Header("Prefabs")]
    //animation prefab for shield hit
    [SerializeField] private GameObject _shieldHit;
    //animation prefab for destruction
    [SerializeField] private GameObject _destroyed;


    [Header("Events")]
    //death event
    public UnityEvent DeathEvent;

    public int ShieldCount => _shieldCount;

    //movement input
    private float _input;

    private Rigidbody2D _rigidbody;

    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        //hotizontal input (between -1 and 1)
        _input = Input.GetAxis("Horizontal");    
    }

    // FixedUpdate is called  50 times per second 
    private void FixedUpdate()
    {
         Vector2 moveForce = new Vector2(_acceleration * _input, 0f);

         _rigidbody.AddForce(moveForce);
         //_rigidbody.velocity = moveForce;
    }

    //called when the player begins overlaping a trigger
    private void OnTriggerEnter2D(Collider2D other)
    {
        //check if player is ovelaping an asteroid
        if(other.CompareTag("Asteroid")){
            //remove one shield
            _shieldCount--;

            //Play shield hit animation
            if(_shieldCount >= 1){
                GameObject spawnedShieldHit = Instantiate(_shieldHit);
                spawnedShieldHit.transform.position = transform.position;
                Destroy(spawnedShieldHit, 1f);
            }

             //Run out of shields
          
            
            Debug.Log("Shield Damage");
             //if we run out of shields and aer damaged again
            if(_shieldCount == 0){
                //invoke event and destoy ship
                DeathEvent.Invoke();
                Destroy(gameObject);
                GameObject spawnedDestroyed = Instantiate(_destroyed);
                spawnedDestroyed.transform.position = transform.position;
                Destroy(spawnedDestroyed, 1f);
            }
        }
    }
}
