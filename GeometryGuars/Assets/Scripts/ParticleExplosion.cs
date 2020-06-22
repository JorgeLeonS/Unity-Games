using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleExplosion : MonoBehaviour
{
[SerializeField] private float _lifeTime = 1f;

    // Start is called before the first frame update
    void Start()
    {
        //Destroy after _lifeTime
        Destroy(gameObject, _lifeTime);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
