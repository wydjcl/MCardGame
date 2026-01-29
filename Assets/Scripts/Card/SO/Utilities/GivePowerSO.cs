using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "给予力量Buff", menuName = "SO/卡牌效果/给予力量Buff")]
public class GivePowerSO : CardEffectSO
{
    public Buff powerBuff;

    public override void ApplyEffect(Character caster, Character target, Card card)
    {
        // target.AddBuff(powerBuff);
    }
}