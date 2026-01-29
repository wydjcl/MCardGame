using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "给予防御", menuName = "SO/卡牌效果/给予防御")]
public class GiveDefenseSO : CardEffectSO
{
    public int defenseValue;
    public bool isGroup;

    public override void ApplyEffect(Character caster, Character target, Card card)
    {
    }
}