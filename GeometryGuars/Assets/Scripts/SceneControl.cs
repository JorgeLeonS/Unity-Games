using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneControl : MonoBehaviour
{
    //Load a scene by name
    public void LoadScene(string sceneName){
        SceneManager.LoadScene(sceneName);
    }

    public void Quit(){
        Debug.Log("Quitting!");
        Application.Quit();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
