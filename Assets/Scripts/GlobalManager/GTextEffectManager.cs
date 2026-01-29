using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GTextEffectManager : SingletonMono<GTextEffectManager>
{
    public GameObject textEffectPrefab;

    private void Start()
    {
    }

    public void ShowTextEffect(int t, Vector3 pos)
    {
        //Debug.Log(pos);
        var Te = Instantiate(textEffectPrefab, pos, Quaternion.identity);
        var textMeshPro = Te.GetComponentInChildren<TextMeshPro>();
        if (t <= 0)
        {
            textMeshPro.color = Color.red;
        }
        else
        {
            textMeshPro.color = Color.green;
        }
        textMeshPro.text = t.ToString();
    }

    public void ShowTextEffect(string t, Vector3 pos)
    {
        var Te = Instantiate(textEffectPrefab, pos, Quaternion.identity);
        var textMeshPro = Te.GetComponent<TextMeshPro>();
        textMeshPro.color = Color.black;
        textMeshPro.text = t;
    }
}