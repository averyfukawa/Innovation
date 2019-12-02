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
    [SerializeField] string animationName;
    [SerializeField] private SFX soundFX;
    [SerializeField] string soundEvent;
    [SerializeField] bool disableThisOnClick = false;

    private FMOD.Studio.Bus _masterBus;

    private void Start()
    {
        _masterBus = FMODUnity.RuntimeManager.GetBus("Bus:/");
        FMODUnity.RuntimeManager.LoadBank("Master Bank");
    }

    public void Activate()
    {
        Debug.Log("The Spatial UI was clicked!");

        if (soundFX != null)
        {
            soundFX.Play(soundEvent);
        }

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

        if (animationName != "")
        {
            Animator ani;
            if (this.TryGetComponent<Animator>(out ani))
            {
                ani.enabled = true;
                ani.Play(animationName, 0, 0);
            }
        }

        if (switchToScene != "")
        {
            if(soundFX != null)
            {
                DontDestroyOnLoad(this);
            }
            SceneManager.LoadScene(switchToScene);
            _masterBus.stopAllEvents(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        }
        if (disableThisOnClick) this.gameObject.SetActive(false);
    }
}
