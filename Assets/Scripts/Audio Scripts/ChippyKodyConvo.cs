using System;
using System.Collections;
using System.Collections.Generic;
using Audio_Scripts;
using UnityEngine;

public class ChippyKodyConvo : MonoBehaviour
{
    [SerializeField] private SFX conversationSounds;
    [SerializeField] private float delayStart;
    
    void Start()
    {
        if (conversationSounds == null)
            throw new Exception("Chippy Kody conversation is empty at " + gameObject.name);
        
        StartCoroutine(Convo(delayStart));
    }

    private IEnumerator Convo(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        conversationSounds.Play("Conversations/mission");
    }
}
