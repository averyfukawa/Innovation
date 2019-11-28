using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Basket : MonoBehaviour
{
    public List<Balloon> balloonsCaught;

    void Start()
    {
        balloonsCaught = new List<Balloon>();
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("caught!");
        Balloon balloon;
        if(other.gameObject.TryGetComponent<Balloon>(out balloon))
        {
            Vector3 difference = other.transform.position - this.transform.position;
            if (difference.y < 0f) return;
            if (difference.x < -0.2f || difference.x > 0.2f) return;
            if (difference.z < -0.2f || difference.z > 0.2f) return;
            balloonsCaught.Add(balloon);
        }
    }
}
