using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoVRPlayer : MonoBehaviour
{
    float _totalRotation;
    [SerializeField] float maxLookUp = 90;
    [SerializeField] float maxLookDown = 60;
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
            if (_totalRotation < maxLookUp && _totalRotation > -maxLookDown)
            {
                this.transform.Rotate(Vector3.right, -y);//, Space.World);
                _totalRotation += y;
            }
            else if (_totalRotation > maxLookUp && y < 0)
            {
                this.transform.Rotate(Vector3.right, -y);//, Space.World);
                _totalRotation += y;
            }
            else if (_totalRotation < -maxLookDown && y > 0)
            {
                this.transform.Rotate(Vector3.right, -y);//, Space.World);
                _totalRotation += y;
            }

            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
        }
    }
}
