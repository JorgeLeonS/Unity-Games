using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponCollider : MonoBehaviour
{
    private Unit _unit;
    private Collider _collider;

    // Start is called before the first frame update
    void Start()
    {
        _unit = GetComponentInParent<Unit>();
        _collider = GetComponent<Collider>();
        EnableCollision(false);
    }

    public void EnableCollision(bool enabled){
        _collider.enabled = enabled;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        Unit hitUnit = other.GetComponent<Unit>();
        if(hitUnit == null || hitUnit.Team == _unit.Team) return;

        _unit.MeleeWeaponHit(hitUnit);
    }

}
