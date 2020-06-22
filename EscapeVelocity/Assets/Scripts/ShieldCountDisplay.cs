using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShieldCountDisplay : MonoBehaviour
{
    //PLayer itself

    [SerializeField] private Player _player;

    //Attached TMPro text
    private TextMeshProUGUI _textComponent;

    // Start is called before the first frame update
    void Start()
    {
        _textComponent = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {

        //Check if player exists
        if(_player == null || _player.gameObject == null){
            _textComponent.text = "0";
            return;
        }else{
            //set the current ShieldCount
            _textComponent.text = _player.ShieldCount.ToString();
        }
    }
}
