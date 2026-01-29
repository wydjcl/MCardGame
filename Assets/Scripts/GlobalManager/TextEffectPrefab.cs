using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TextEffectPrefab : MonoBehaviour
{
    // Start is called before the first frame update
    private void Start()
    {
        // 初始位置
        Vector3 startPos = transform.localPosition; // 目标位置 = 初始位置 + 偏移
        Vector3 targetPos = startPos + new Vector3(0.75f, 0.4f, 0); // 使用 DOJump 做右上角弹跳
        transform.DOJump(targetPos, 0.8f, 2, 0.6f).SetEase(Ease.Linear).OnComplete(() => Destroy(gameObject));
    }
}