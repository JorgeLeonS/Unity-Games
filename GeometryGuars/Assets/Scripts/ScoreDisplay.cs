using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreDisplay : MonoBehaviour
{
    //Prefix and affix strings
    [SerializeField] private string _prefix;
    [SerializeField] private string _affix;

    private TextMeshProUGUI _textComponent;
    private int _currentScore;
    // Start is called before the first frame update
    void Start()
    {
        _textComponent = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        //interpolate score
        _currentScore = Mathf.CeilToInt(Mathf.Lerp(_currentScore, GameManager.Instance.Score, Time.deltaTime*10f));
        //update score text
        _textComponent.text = _prefix + _currentScore + _affix;
    }
}
