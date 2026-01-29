using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "治疗", menuName = "SO/卡牌效果/治疗")]
public class HealEffect : CardEffectSO
{
    public bool isAOE;
    public int ThealAmount = 6;

    public override void ApplyEffect(Character caster, Character target, Card card)
    {
        var msg = new CauseHeal
        {
            targetID = target.ID,
            healAmount = ThealAmount,
            casterID = caster.ID,
            aoe = isAOE,
        };
        GMSGManager.Instance.SendToServer(msg);
    }
}