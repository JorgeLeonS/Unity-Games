using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#pragma warning disable 649

public class AnimateBackground : MonoBehaviour
{
    [SerializeField] private Vector2 _scrollSpeed;

    private Material _backgroundMaterial;
    // Start is called before the first frame update
    void Start()
    {
        _backgroundMaterial = GetComponent<MeshRenderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        _backgroundMaterial.mainTextureOffset += _scrollSpeed * Time.deltaTime;
    }
}
