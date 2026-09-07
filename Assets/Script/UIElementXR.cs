using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class UIElementXR : MonoBehaviour
{
    public UnityEvent OnXRPointerEnter;
    public UnityEvent OnXRPointerExit;
    private Camera xRCamera;

    void Start()
    {
        GetXRCamera();
    }

    private Camera GetXRCamera()
    {
        if (xRCamera == null)
        {
            if (CameraPointerManager.Instance != null)
            {
                xRCamera = CameraPointerManager.Instance.GetComponent<Camera>();
            }
            if (xRCamera == null)
            {
                xRCamera = Camera.main;
            }
        }
        return xRCamera;
    }

    public void OnPointerClickXR()
    {
        PointerEventData pointerEvent = PlacePointer();
        ExecuteEvents.Execute(this.gameObject, pointerEvent, ExecuteEvents.pointerClickHandler);
    }

    public void OnPointerEnterXR()
    {
        if (GazeManager.Instance != null)
        {
            GazeManager.Instance.SetUpGaze(1.5f);
        }
        OnXRPointerEnter?.Invoke();

        PointerEventData pointerEvent = PlacePointer();
        ExecuteEvents.Execute(this.gameObject, pointerEvent, ExecuteEvents.pointerDownHandler);
    }

    public void OnPointerExitXR()
    {
        if (GazeManager.Instance != null)
        {
            GazeManager.Instance.SetUpGaze(2.5f);
        }
        OnXRPointerExit?.Invoke();

        PointerEventData pointerEvent = PlacePointer();
        ExecuteEvents.Execute(this.gameObject, pointerEvent, ExecuteEvents.pointerUpHandler);
    }

    public PointerEventData PlacePointer()
    {
        Camera cam = GetXRCamera();
        Vector3 screenPos = cam != null && CameraPointerManager.Instance != null 
            ? cam.WorldToScreenPoint(CameraPointerManager.Instance.hitPoint) 
            : Vector3.zero;

        var pointer = new PointerEventData(EventSystem.current);
        pointer.position = new Vector2(screenPos.x, screenPos.y);
        return pointer;
    }
}
