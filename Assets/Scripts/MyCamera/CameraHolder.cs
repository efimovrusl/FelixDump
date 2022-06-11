using System;
using UnityEngine;
using Zenject;

namespace MyCamera
{
public class CameraHolder : MonoBehaviour
{
    [Inject] private CameraManager cameraManager;

    private void FixedUpdate()
    {
        var focusTransform = transform;
        cameraManager.MoveCameraTo(focusTransform, focusTransform.localPosition, focusTransform.localRotation);
    }
}
}
