using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Enemy : Character
{
    // Start is called before the first frame update
    public TextMeshPro HPText;

    public override void Start()
    {
        transform.position = new Vector3(6, 3.2f, 0);
        base.Start();
        UpdateUI();
    }

    public override void UpdateUI()
    {
        base.UpdateUI();
        HPText.text = $"HP:{HP}/{MaxHP}";
    }
}