using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEventReceiver : MonoBehaviour
{

    [SerializeField] private AudioClip _footStepSound;

    private WeaponCollider _weaponCollision;

    // Start is called before the first frame update
    void Start()
    {
        _weaponCollision = GetComponentInChildren<WeaponCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Calles from walk/run animations
    private void PlayStep(){
        if(_footStepSound != null) AudioManager.PlayClip(_footStepSound, transform.position, true, null, 1f, 0.2f);
    }

    private void Grunt(){

    }

    private void MeleeAttackStart(){
        _weaponCollision.EnableCollision(true);
    }

    private void MeleeAttackEnd(){
        _weaponCollision.EnableCollision(false);
    }

    private void Hit(){
        
    }
}
