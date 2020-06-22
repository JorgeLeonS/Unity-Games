using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootFixer : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    private void LateUpdate()
    {
        transform.localPosition = Vector3.zero;
    }
}
