using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Balloon : MonoBehaviour
{
    public GameObject prefab;
    public TextMeshPro textMesh;
    public float timeOfExistance = 0;
    private float addToTime;
    public Vector3 position;
    public Vector3 wavyPosition;
    public char letter;

    public void Init(GameObject go, Vector3 pPos, Vector3 facing, TextMeshPro pTextMesh, char pChar)
    {
        prefab = go;
        position = pPos;

        Vector3 difference = position - facing;
        prefab.transform.Rotate(new Vector3(0, Mathf.Rad2Deg * Mathf.Atan2(difference.x, difference.z) , 0));
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
        float wavyX = Mathf.Sin(timeOfExistance / 3);
        float wavyZ = Mathf.Cos(timeOfExistance) * 0.3f;
        wavyPosition = new Vector3(wavyX, wavyX*wavyZ, wavyZ);

       prefab.transform.position = position + wavyPosition;
    }

    void OnDestroy()
    {
        Destroy(prefab);
    }
}
