using UnityEngine;

namespace MyCamera
{
public class CameraManager : MonoBehaviour
{
    [SerializeField] private Transform cameraTransform;
    
    public void MoveCameraTo(Transform focusTransform, 
        Vector3? localPositionOffset = null, Quaternion? localRotation = null)
    {
        localPositionOffset ??= Vector3.zero;
        localRotation ??= Quaternion.identity;

        cameraTransform.position = focusTransform.position + localPositionOffset.Value;
        cameraTransform.localRotation = localRotation.Value;
    }
    
    
}
}
