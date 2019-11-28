using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vector3Ex : MonoBehaviour
{
    static public Vector3 eulerToVector3(Vector3 euler)
    {
        euler *= Mathf.Deg2Rad;
        Vector3 value;

        value.x = Mathf.Cos(euler.y) + Mathf.Sin(euler.z);
        value.y = Mathf.Cos(euler.z) + Mathf.Sin(euler.x);
        value.z = Mathf.Cos(euler.x) + Mathf.Sin(euler.y);

        value.Normalize();

        return value;
    }

    static public Vector3 scaleVector(Vector3 vec, float scaler)
    {
        vec.x *= scaler;
        vec.y *= scaler;
        vec.z *= scaler;

        return vec;
    }
}
