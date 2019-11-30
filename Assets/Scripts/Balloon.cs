using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Balloon : MonoBehaviour
{
    public GameObject prefab;
    public TextMeshPro textMesh;
    public Rigidbody rigidBody;
    public float timeOfExistance = 0;
    private float addToTime;
    public Vector3 position;
    public Vector3 wavyPosition;
    public char letter;
    public float waveAmountX = 0f;
    public float waveAmountY = 0f;
    private Vector3 _facingTowards;
    public bool beingHeld = false;
    public bool IsInNet = false;
    public bool hasCorrectLetter = false;

    public void Init(GameObject go, Vector3 pPos, Vector3 facing, TextMeshPro pTextMesh, char pChar)
    {
        rigidBody = this.GetComponent<Rigidbody>();
        prefab = go;
        position = pPos;

        Vector3 difference = position - facing;
        prefab.transform.Rotate(new Vector3(0, Mathf.Rad2Deg * Mathf.Atan2(difference.x, difference.z) , 0));

        _facingTowards = facing;
        addToTime = Random.Range(0, Mathf.PI);

        letter = pChar;
        
        textMesh = pTextMesh;
        textMesh.text = pChar.ToString();

        prefab.transform.position = position + wavyPosition;
    }

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        timeOfExistance += Time.deltaTime;
    }

    public void Move()
    {
        if (beingHeld) return;
        float wavyX = Mathf.Cos(timeOfExistance+addToTime) * waveAmountX;
        float wavyZ = Mathf.Sin(timeOfExistance+addToTime) * waveAmountY;
        wavyPosition = new Vector3(wavyX, /*wavyX*wavyZ*/0, wavyZ);

        //position.y -= 0.008f;
        prefab.transform.position = new Vector3(position.x + wavyPosition.x, prefab.transform.position.y, position.z + wavyPosition.z);
    }

    void OnDestroy()
    {
        Destroy(prefab);
    }
}
