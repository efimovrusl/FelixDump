using System;
using Managers;
using UnityEngine;
using Zenject;

public class HelixRotator : MonoBehaviour
{
    [Inject] private InputManager inputManager;
    [Inject] private PlayerPrefsManager playerPrefsManager;
    private void OnEnable()
    {
        inputManager.OnTouchSwipe += RotateHelix;
    }

    private void OnDisable()
    {
        inputManager.OnTouchSwipe -= RotateHelix;
    }

    private void RotateHelix(float y)
    {
        transform.Rotate(Vector3.up, -y * playerPrefsManager.Sensitivity);
    }
}
