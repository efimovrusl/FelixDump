using System.Collections;
using UnityEngine;
using DG.Tweening;

namespace Platforms.Components
{
    
[DisallowMultipleComponent]
public class RotationComponent : MonoBehaviour
{
    private Tween tween;
    
    public void StartCyclicRotation(Quaternion deltaRotation, float durationSeconds)
    {
        tween = transform.DOLocalRotate(deltaRotation.eulerAngles, durationSeconds, RotateMode.LocalAxisAdd)
        .SetEase(Ease.InOutQuad)
        .SetLoops(-1, LoopType.Yoyo);
    }

    public void StopRotation() => tween?.Kill();

}
}
