using UnityEngine;
using System.Collections;

public class InputHandler : MonoBehaviour
{
    [SerializeField] GameObject controller;

    void Update()
    {
        RaycastHit hit;
        Vector3 rayDirection = controller.transform.forward;
        if (Physics.Raycast(controller.transform.position, rayDirection, out hit))
        {
            if (Input.GetMouseButtonDown(0))
            {
                ToyCar toycar = hit.transform.GetComponent<ToyCar>();
                if (toycar)
                    toycar.PlayToyCarAnim();
            }
        }
    }
}