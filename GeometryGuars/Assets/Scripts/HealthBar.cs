using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    //the unit we're displaying
    [SerializeField] private Unit _target;

    [SerializeField] private Image _barImage;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(_target == null){
            _barImage.fillAmount = 0f;
            return;
        }
        //set fill amount to target's health percentage
        //_barImage.fillAmount = _target.HealthPercentage;
       _barImage.fillAmount = Mathf.Lerp(_barImage.fillAmount, _target.HealthPercentage, Time.deltaTime*5f);
    }
}
