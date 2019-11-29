using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Audio_Scripts
{
    public class BKM : MonoBehaviour
    {
        [SerializeField] private int currentMusic;
        [FMODUnity.EventRef, SerializeField] private string background;

        private FMOD.Studio.EventInstance _music;

        private void Start()
        {
            _music = FMODUnity.RuntimeManager.CreateInstance(background);
            _music.setParameterByName("song", currentMusic);
            _music.start();
        }
    }
}