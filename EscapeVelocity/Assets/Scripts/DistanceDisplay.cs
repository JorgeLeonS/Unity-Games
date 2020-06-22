using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DistanceDisplay : MonoBehaviour
{
    [SerializeField] private GameManager _gameManager;

    //string diplayed in front of distances
    [SerializeField] private string _prefix;
    //string displayed after distance
    [SerializeField] private string _affix;

    private TextMeshProUGUI _textComponent;

    // Start is called before the first frame update
    void Start()
    {
        _textComponent = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        int roundedDistance = Mathf.RoundToInt(_gameManager.TotalDistance);
        _textComponent.text =_prefix + roundedDistance.ToString() + _affix;
    }
}
