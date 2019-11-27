using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Audio_Scripts
{
    public class BKM : MonoBehaviour
    {
        [SerializeField] private int currentMusic;

        private FMOD.Studio.EventInstance _music;

        private void Start()
        {
            _music = FMODUnity.RuntimeManager.CreateInstance("event:/BKM");
            _music.start();
        }

        private void Update()
        {
            _music.setParameterByName("song", currentMusic);
        }
    }
}