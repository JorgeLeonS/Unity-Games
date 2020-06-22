using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    [SerializeField] private float _destroyHeight = -6f;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       if(transform.position.y < _destroyHeight){
           //Destroy the asteroid from the scene
           Destroy(gameObject);
       } 
    }
}
