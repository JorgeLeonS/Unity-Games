using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Unit
{
    [Header("Weapon")]
    [SerializeField] private Projectile _projectilePrefab;
    [SerializeField] private float _RPM = 240f; //RPM = Rounds per minute (Shots in a minute)

    private float _lastFireTime;
    private AudioSource _shootSound;

    // Update is called once per frame
    void Update()
    {
        //Get move input
        Vector3 moveInput = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
        //Normalize to magnitude of 1
        if(moveInput.magnitude > 1f){
            moveInput = moveInput.normalized;
        }   
        //Move
        MoveInDirection(moveInput, Time.deltaTime);
        //Get min input
        Vector3 aimInput = new Vector3(Input.GetAxis("AimHorizontal"), 0f, Input.GetAxis("AimVertical"));
        if(aimInput.magnitude >= 0.1f){
            TryFire(aimInput);
            
        }
    }
    //Attempt to fire in a given direction
    private void TryFire(Vector3 aimInput)
    {
        //Check if we can fire
        float timeBetweenShots = 60f/_RPM;
        //stop if we cant fire yet
        if(Time.timeSinceLevelLoad < _lastFireTime + timeBetweenShots){
            return;
        }
        //Update last fire time
        _lastFireTime = Time.timeSinceLevelLoad;

        //Spawn a projectile
        Vector3 spawnPosition = transform.position;
        Quaternion spawnRotation = Quaternion.LookRotation(aimInput.normalized, Vector3.up);
        //Spawning Projectile at spawnPosition with rotation spawnRotation
        Instantiate(_projectilePrefab, spawnPosition, spawnRotation);

        //Play shoot sound
        _shootSound.Play();
    }

    protected override void Start(){
        base.Start();
        _shootSound = GetComponent<AudioSource>();
    }


}
