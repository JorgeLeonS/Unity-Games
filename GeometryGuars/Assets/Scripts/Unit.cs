using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{

    [Header("Movement")]
    [SerializeField] private float _acceleration = 10f;
    [SerializeField] private float _turnSpeed = 360f;

    [Header("health")]
    [SerializeField] private int _health = 1;
    [SerializeField] private int _maxHealth = 1;
    [SerializeField] private ParticleSystem _deathParticles;

    [Header("Contact Damage")]
    [SerializeField] private int _contactDamageAmount;
    [SerializeField] private string _contactDamageTag;

   
    //Current health percentage (0 to 1)
    public float HealthPercentage => (float) _health / _maxHealth;

    private Rigidbody _rigidbody;

    //Move in a given direction
    protected void MoveInDirection(Vector3 direction, float deltaTime){
        //Ignore low input
        if(direction.magnitude < 0.1f){
            return;
        }
        //Rotate towards direction
        //Find target rotation
        Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);
        Quaternion currentRotation = transform.rotation;

        //Find new rotation
        Quaternion rotation = Quaternion.RotateTowards(currentRotation, targetRotation, _turnSpeed * deltaTime);
        _rigidbody.MoveRotation(rotation);

        //Move forwards
        _rigidbody.AddForce(transform.forward * _acceleration * deltaTime);
    }

    //When health is 0
    protected virtual void Death(){
        Instantiate(_deathParticles, transform.position, transform.rotation);
        Destroy(gameObject);
    }
    private void OnCollisionEnter(Collision other)
    {
        //Check if the other objects has  a rigidbody, and has thw right tag
        if(other.rigidbody != null && other.rigidbody.CompareTag(_contactDamageTag)){
            //try to damage the unit
            other.rigidbody.GetComponent<Unit>()?.Damage(_contactDamageAmount);
        }
    }

    public void Damage(int amount){
        _health -= amount;

        if(_health <= 0){
            Death();
        }
    }


    // Start is called before the first frame update
    protected virtual void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
       
    }
}
