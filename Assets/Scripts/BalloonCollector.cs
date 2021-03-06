﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Audio_Scripts;

public class BalloonCollector : MonoBehaviour
{
    public List<Balloon> collectedBalloons;
    public ParticleSystem particles;
    public SFX sfx;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        Balloon balloon;
        if (other.gameObject.TryGetComponent<Balloon>(out balloon))
        {
            if(balloon.hasCorrectLetter && !balloon.IsInNet && balloon.beingHeld)
            {
                collectedBalloons.Add(balloon);
                particles.Play();
            }
            else if(!balloon.hasCorrectLetter && !balloon.IsInNet && balloon.beingHeld)
            {
                sfx.Play("Conversations/Wrong");
            }
        }
    }
}
