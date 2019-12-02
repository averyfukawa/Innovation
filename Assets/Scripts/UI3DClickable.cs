using System;
using System.Collections;
using System.Collections.Generic;
using Audio_Scripts;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI3DClickable : MonoBehaviour
{
    [SerializeField] string switchToScene;
    [SerializeField] GameObject[] switchActiveStateObjects;
    [SerializeField] private SFX sounds;
    [SerializeField] bool disableThisOnClick = false;

    // Start is called before the first frame update
    void Start()
    {
    }

    public void Activate()
    {
        DontDestroyOnLoad(this);

        Debug.Log("The Spatial UI was clicked!");

        if (sounds != null) sounds.Play("SFX/UI Click");

        for (int i = 0; i < switchActiveStateObjects.Length; i++)
        {
            if (switchActiveStateObjects[i] != null)
            {
                if (switchActiveStateObjects[i].activeSelf)
                {
                    switchActiveStateObjects[i].SetActive(false);
                }
                else
                {
                    switchActiveStateObjects[i].SetActive(true);
                }
            }
        }

        if (switchToScene != "") SceneManager.LoadScene(switchToScene);
        if (disableThisOnClick) this.gameObject.SetActive(false);
    }
}
