using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI3DHandler : MonoBehaviour
{
    [SerializeField] GameObject controller;
    Vector3 startOrientation;

    // Start is called before the first frame update
    void Start()
    {
        startOrientation = controller.transform.forward;
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;

        Vector3 rayDirection = controller.transform.forward;

        Debug.DrawRay(controller.transform.position, rayDirection, Color.red);

        if(Input.GetMouseButtonDown(0) && Physics.Raycast(controller.transform.position, rayDirection, out hit))
        {
            UI3DClickable clickable;
            if(hit.transform.TryGetComponent<UI3DClickable>(out clickable))
            {
                clickable.Activate();
            }
        }
    }
}
