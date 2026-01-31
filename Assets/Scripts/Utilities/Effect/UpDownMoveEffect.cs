using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class UpDownMoveEffect : MonoBehaviour
{
    public float dis = 1f;
    public float duration = 1f;

    private void Start()
    {
        Vector3 startPos = transform.position;
        transform.DOMoveY(startPos.y + dis, duration)
            .SetLoops(-1, LoopType.Yoyo)
            .SetEase(Ease.Linear);
    }

    private void OnDestroy()
    {
        DOTween.Kill(transform);
    }
}