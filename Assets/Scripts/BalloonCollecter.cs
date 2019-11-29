using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalloonCollecter : MonoBehaviour
{
    public List<Balloon> collectedBalloons;

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
            if(balloon.hasCorrectLetter && !balloon.beingHeld)
            {
                collectedBalloons.Add(balloon);
            }
        }
    }
}
