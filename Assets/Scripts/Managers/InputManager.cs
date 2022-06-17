using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace Managers
{
public class InputManager : MonoBehaviour
{
    private TouchControls touchControls;

    public delegate void OnTouchSwipeEvent(float angle);
    public event OnTouchSwipeEvent OnTouchSwipe;

    private void Awake()
    {
        touchControls = new TouchControls();
    }
    
    private void OnEnable()
    {
        touchControls.Enable();
    }
    
    private void OnDisable()
    {
        touchControls.Disable();
    }

    private void Start()
    {
        touchControls.Touch.TouchPress.started += StartTouch;
        touchControls.Touch.TouchPress.canceled += EndTouch;
    }
    
    private void StartTouch(InputAction.CallbackContext ctx)
    {
        StartCoroutine(_TouchHandler());
    }

    private void EndTouch(InputAction.CallbackContext ctx)
    {
    }

    private IEnumerator _TouchHandler()
    {
        while (touchControls.Touch.TouchPress.inProgress)
        {
            OnTouchSwipe?.Invoke(GetTouchScreenDelta().x);
            yield return null;
        }
    }

    private Vector2 GetTouchScreenPosition() => touchControls.Touch.TouchPosition.ReadValue<Vector2>();
    
    private Vector2 GetTouchScreenDelta() => touchControls.Touch.TouchDelta.ReadValue<Vector2>();
    
    }
}

