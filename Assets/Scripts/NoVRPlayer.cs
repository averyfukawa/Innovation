using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoVRPlayer : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        float x = Input.GetAxis("Mouse X");
        float y = Input.GetAxis("Mouse Y");

        if (!Input.GetKey(KeyCode.Space))
        {
            this.transform.Rotate(Vector3.up, x, Space.World);
            this.transform.Rotate(Vector3.right, -y);//, Space.World);

            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
        }
    }
}
