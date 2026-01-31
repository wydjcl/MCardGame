using DG.Tweening;
using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Character : PhysicallyObject
{
    [HideInInspector]
    public BattleManager battleManager;

    [HideInInspector]
    public Vector3 effectPos;

    public GameObject SPG;
    public int ID;

    public int MaxHP;
    public int HP;
    public int ice;
    public int attack;
    public bool isEnemy;

    public virtual void Awake()
    {
        battleManager = FindObjectOfType<BattleManager>();
    }

    public virtual void Start()
    {
        SetEffectPos();
    }

    public virtual void SetEffectPos()
    {
        effectPos = transform.position;
    }

    public virtual int TakeDamage(int i)
    {
        //Debug.Log("敌人坐标_" + transform.position);
        HP -= i;
        GTextEffectManager.Instance.ShowTextEffect(-i, effectPos);
        UpdateUI();
        return i;
    }

    public virtual void CauseDamage(Character target, int i)
    {
        target.TakeDamage(i);
    }

    public virtual void Heal(int i)
    {
        var orHP = HP;
        HP += i;
        if (HP > MaxHP)
        {
            HP = MaxHP;
        }
        GTextEffectManager.Instance.ShowTextEffect(HP - orHP, effectPos);
        UpdateUI();
    }

    public virtual void CauseHeal(Character target, int i)
    {
        target.Heal(i);
    }

    public virtual void UpdateUI()
    {
    }

    [ContextMenu("DoAni")]
    public void DoAni1()
    {
        DoAni(1);
    }

    public virtual void DoAni(int i)
    {
        var orSPG = SPG.transform.localScale;
        Sequence seq = DOTween.Sequence();
        seq.Append(SPG.transform.DOScale(orSPG * 0.75f, 0.5f).SetEase(Ease.Linear));
        seq.Append(SPG.transform.DOScale(orSPG, 0.3f).SetEase(Ease.Linear));
    }

    private void OnDestroy()
    {
        if (SPG != null)
            DOTween.Kill(SPG.transform);
    }
}