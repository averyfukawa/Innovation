using System;
using System.Collections;
using System.Collections.Generic;
using Audio_Scripts;
using UnityEngine;

public class tutorial : MonoBehaviour
{
    [SerializeField] private SFX tutorialSounds;

    [SerializeField] private float waitForTutBegin;
    [SerializeField] private float waitForInactiveTutBegin;
    [SerializeField] private float inactivityWaitTime;
    [SerializeField] private float interval;
    
    private bool _tutorialEnded = false;
    private bool _inactiveTutEnded = false;
    private bool _inactive = false;
    
    void Start()
    {
        if (tutorialSounds == null)
            throw new Exception("Tutorial SFX game object is not set in " + gameObject.name);
        
        if (waitForTutBegin == 0f || waitForInactiveTutBegin == 0f || inactivityWaitTime == 0f || interval == 0f) 
            throw new Exception("Time spacing are not set up completely! Make sure they are more than 0!");
        
        StartCoroutine(BeginTutorial(waitForTutBegin));
    }

    
    void Update()
    {
        if (_tutorialEnded == true && _inactiveTutEnded == false && _inactive == false)
        {
            StartCoroutine(InactiveTutorial(waitForInactiveTutBegin));
        }
        
        if (_tutorialEnded == true && _inactiveTutEnded == true && _inactive == false)
        {
            StartCoroutine(Inactive(inactivityWaitTime));
            _inactive = true;
        }
    }

    // This is the tutorial the player hears the moment they enter the game
    private IEnumerator BeginTutorial(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        tutorialSounds.Play("Conversations/tutorial");
        _tutorialEnded = true;
    }

    // This is the tutorial the player hears if they are inactive for a while after hearing the main tutorial
    private IEnumerator InactiveTutorial(float waitTime)
    {
        _inactiveTutEnded = true;
        yield return new WaitForSeconds(waitTime + 6.6f);
        tutorialSounds.Play("Conversations/tutorial inactive");
    }

    // This is random commands they hear after all tutorials and being inactive for a while
    private IEnumerator Inactive(float waitTime)
    {
        yield return new WaitForSeconds(waitTime + 2.2f);
        tutorialSounds.Play("Conversations/inactive");

        if (_inactive == true)
            StartCoroutine(InactiveInterval(interval));
    }

    // This is the interval with which the above inactive commands happen
    private IEnumerator InactiveInterval(float waitTime)
    {
        yield return new WaitForSeconds(waitTime + 1.9f);
        _inactive = false;
    }
}
