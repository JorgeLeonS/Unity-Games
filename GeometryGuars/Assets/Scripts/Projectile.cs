using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float _velocity = 40f;
    [SerializeField] private int _damage = 1;
    [SerializeField] private float _lifeTime = 2f;
    [SerializeField] private ParticleSystem _explosionParticles;
    // Start is called before the first frame update
    void Start()
    {
        //Set forward velocity to _velocity
        GetComponent<Rigidbody>().velocity = transform.forward * _velocity;
        Destroy(gameObject, _lifeTime);
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.attachedRigidbody != null && other.attachedRigidbody.CompareTag("Player")){
            return;
        }
        //Try to damage what we jhit
        if(other.attachedRigidbody != null){
            other.attachedRigidbody.GetComponent<Unit>()?.Damage(_damage);
        }

        //Spawn Particles little behind the projectile
        Vector3 spawnPosition = transform.position - transform.forward*0.25f;
        Instantiate(_explosionParticles, transform.position, transform.rotation);
        
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
