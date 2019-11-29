using System;
using System.Collections;
using UnityEngine;

namespace Audio_Scripts
{
    public class SFX : MonoBehaviour
    {
        [FMODUnity.EventRef] public string sfxEvent;
        //public string sfxEvent;
        
        private bool _isPlaying = false;

        [SerializeField] private float clipLength;

        private void Start()
        {
            if (sfxEvent == null)
                throw new Exception("SFX Event not set in " + gameObject.name);
        }

        public void Play(string fmodEvent)
        {
            Debug.Log("Audio is playing");
            if (_isPlaying == false)
            {
                sfxEvent = "event:/" + fmodEvent;
                
                FMODUnity.RuntimeManager.PlayOneShot(sfxEvent, transform.position);
                
                _isPlaying = true;
                
                StartCoroutine(WaitForEnd(clipLength));
            }
        }

        private IEnumerator WaitForEnd(float length)
        {
            if (length <= 0) throw new ArgumentOutOfRangeException("Length of clip is not set in " + " " + gameObject.name + nameof(length));
            
            length = clipLength;
            yield return new WaitForSeconds(length);
            _isPlaying = false;
        }
    
    }
}