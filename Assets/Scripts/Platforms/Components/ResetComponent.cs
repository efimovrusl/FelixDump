using UnityEngine;

namespace Platforms.Components
{
[DisallowMultipleComponent]
public class ResetComponent : MonoBehaviour
{
    private Rigidbody _rigidbody;
    private RotationComponent _rotatingComponent;

    private void Awake() => Reset();

    public void Reset()
    {
        transform.parent = null;
        if (!TryGetComponent<Rigidbody>(out _))
            _rigidbody = gameObject.AddComponent<Rigidbody>();
        _rigidbody.useGravity = true;
        _rigidbody.velocity = Vector3.zero;
        _rigidbody.isKinematic = true;
        gameObject.transform.localRotation = Quaternion.identity;
        gameObject.transform.localPosition = Vector3.zero;
        gameObject.SetActive(false);
    }
}
}

