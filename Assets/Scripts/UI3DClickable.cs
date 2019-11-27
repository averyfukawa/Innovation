using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI3DClickable : MonoBehaviour
{
    [SerializeField] string switchToScene;
    [SerializeField] string audioFile; 

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Activate()
    {
        if(switchToScene != null)
        {
            DontDestroyOnLoad(this);
            SceneManager.LoadScene(switchToScene);
        }
    }
}
