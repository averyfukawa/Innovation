using System.Collections;
using System.Collections.Generic;
using Audio_Scripts;
using UnityEngine;

public class UI3DHandler : MonoBehaviour
{
    [SerializeField] GameObject controller;

    [SerializeField] private SFX sounds;
    [SerializeField] private Material outline;
    [SerializeField, Range(0.0f, 0.5f)] private float outlineMinWidth = 0.2f;
    [SerializeField, Range(0.0f, 0.5f)] private float outlineMaxWidth = 0.5f;
    
    private bool _hasHovered = false;
    
    // Start is called before the first frame update
    void Start()
    {
        outline.SetFloat("_Outline", outlineMinWidth);
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;

        Vector3 rayDirection = controller.transform.forward;

        Debug.DrawRay(controller.transform.position, rayDirection, Color.red);
        if (Physics.Raycast(controller.transform.position, rayDirection, out hit))
        {
            UI3DClickable clickable;
            if (hit.transform.TryGetComponent<UI3DClickable>(out clickable))
            {
                if (_hasHovered == false)
                {
                    Debug.Log("The Spatial UI was hovered!");
                    sounds.Play("SFX/UI Hover");
                    _hasHovered = true;
                    outline.SetFloat("_Outline", outlineMaxWidth);
                }
            }
        }
        else
        {
            _hasHovered = false;
            outline.SetFloat("_Outline", outlineMinWidth);
        }

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
