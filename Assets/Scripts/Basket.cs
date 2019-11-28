using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Basket : MonoBehaviour
{
    public Balloon caughtBalloon;
    public GameObject catchPosObject;
    public Vector3 catchPos;

    void Start()
    {
        if (catchPosObject != null)
        {
            catchPos = catchPosObject.transform.position;
            Destroy(catchPosObject);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        Balloon balloon;
        float catchRange = 1.0f;
        if (caughtBalloon == null && other.gameObject.TryGetComponent<Balloon>(out balloon))
        {
            Vector3 difference = other.transform.position - catchPos;
            Debug.Log("difference: " + other.transform.position.ToString() + " with " + catchPos);
            Debug.Log("full differenc: " + difference.ToString());
            if (difference.y < 0) return;

            Debug.Log("catch");
            caughtBalloon = balloon;
        }
    }
}
