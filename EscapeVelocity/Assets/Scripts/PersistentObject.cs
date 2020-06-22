using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistentObject : MonoBehaviour
{

    //static reference to this monobehaviour
    private static PersistentObject _instance;
    // Start is called before the first frame update
    void Start()
    {
        if(_instance != null){
            Destroy(gameObject);
            return;
        }
        _instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
