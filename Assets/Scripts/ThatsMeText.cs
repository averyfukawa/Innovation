using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThatsMeText : MonoBehaviour
{
    public Transform dreamyboi01;
    public GameObject player;
    public Transform textPosition;

    private float _textX;
    private float _textY; // Don't want to change the current Y rotation
    private float _textZ;
    private void Start()
    {
        if (player == null || dreamyboi01 == null) { throw new Exception("Please attach the player or AI to " + gameObject.name); }
        if (textPosition == null) { Debug.Log("TextPosition has not been assigned to " + gameObject.name); }
    }

    private void Update()
    {
        var dreamyboi01Position = dreamyboi01.position;
        textPosition.position = new Vector3(dreamyboi01Position.x, textPosition.position.y,dreamyboi01Position.z);

        var eulerAngles = player.transform.eulerAngles;
        _textX = eulerAngles.x;
        _textY = eulerAngles.y;
        _textZ = eulerAngles.z;

        transform.eulerAngles = new Vector3(_textX - _textX, _textY, _textZ - _textZ);
    }
}
