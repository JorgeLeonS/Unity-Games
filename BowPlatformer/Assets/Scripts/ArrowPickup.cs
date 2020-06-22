using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowPickup : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D other)
    {
        //Check for player
        Player otherPlayer = other.GetComponent<Player>();  
        if(otherPlayer != null && !otherPlayer.IsDead){
            //Try to add an arrow
            bool arrowGrabbed = otherPlayer.AddArrow();
            if(arrowGrabbed){
                Destroy(gameObject);
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
