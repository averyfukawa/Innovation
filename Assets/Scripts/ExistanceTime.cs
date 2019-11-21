using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExistanceTime : MonoBehaviour
{
    public float timeOfExistance;

    // Start is called before the first frame update
    void Start()
    {
        timeOfExistance = 0;
    }

    // Update is called once per frame
    void Update()
    {
        timeOfExistance += Time.deltaTime;
    }
}
