using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Basket : MonoBehaviour
{
    public Balloon caughtBalloon;
    public bool holdsBalloon = false;
    public GameObject catchPosObject;
    public Vector3 catchPos;
    bool usePreciseCathcing = true;

    void Start()
    {
        if (catchPosObject != null)
        {
            catchPos = catchPosObject.transform.position;
            Destroy(catchPosObject);
        }
    }

    public void ClearBalloon()
    {
        caughtBalloon = null;
        holdsBalloon = false;
        Debug.Log("balloon: " + holdsBalloon);
    }

    void OnTriggerEnter(Collider other)
    {
        Balloon balloon;
        if (other.gameObject.TryGetComponent<Balloon>(out balloon))
        {
            if (holdsBalloon == true)
            {
                Debug.Log("holds a balloon");
                return;
            }
            if (usePreciseCathcing)
            {
                Vector3 difference = other.transform.position - catchPos;
                if (difference.y < 0) return;
            }

            Debug.Log("caught balloon!");
            caughtBalloon = balloon;
            holdsBalloon = true;
        }
    }
}
