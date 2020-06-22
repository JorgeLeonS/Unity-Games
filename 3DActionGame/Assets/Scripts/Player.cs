using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Unit
{

    
    // Start is called before the first frame update
    void Start()
    {
        //Lock and hide the cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        float horzontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        Vector3 horizontalDirection = Camera.main.transform.right * horzontalInput;
        Vector3 vertialDirection = Camera.main.transform.forward * verticalInput;
        Vector3 input = horizontalDirection + vertialDirection;

        MoveInput = input;

        if(Input.GetButtonDown("Jump")){
            TryJump();
        }

        if(Input.GetButtonDown("Attack")){
            TryAtttack();
        }

    }
}
