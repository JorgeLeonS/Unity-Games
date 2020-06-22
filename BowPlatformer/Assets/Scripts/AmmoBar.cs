using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AmmoBar : MonoBehaviour
{
    private Player _player;
    private Image _ammo;

    // Start is called before the first frame update
    void Start()
    {
        _player = GetComponentInParent<Player>();
        _ammo = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        _ammo.fillAmount = _player.AmmoPercentage;
    }
    
}
