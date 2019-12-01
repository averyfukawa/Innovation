using System;
using System.Collections;
using System.Collections.Generic;
using Audio_Scripts;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI3DClickable : MonoBehaviour
{
    [SerializeField] string switchToScene;
    
    [SerializeField] private SFX sounds;
    // Start is called before the first frame update
    void Start()
    {
        if (switchToScene == null)
            throw new Exception("Scene name not set in " + gameObject.name + "!");
        
        if (sounds == null)
            throw new Exception("SFX not set in " + gameObject.name);
    }
    
    public void Activate()
    {
        if(switchToScene != null)
        {
            DontDestroyOnLoad(this);
            
            Debug.Log("The Spatial UI was clicked!");
            sounds.Play("SFX/UI Click");
            
            SceneManager.LoadScene(switchToScene);
        }
    }
}
