using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "普通攻击", menuName = "SO/卡牌效果/普通攻击")]
public class NormalAttackEffect : CardEffectSO
{
    public bool isAOE;
    public int damage = 6;
    public bool isSheldAttack;

    public override void ApplyEffect(Character caster, Character target, Card card)
    {
        //caster.CauseDamage(target, damage);
        var msg = new CauseDamage
        {
            targetID = target.ID,
            damageAmount = damage,
            casterID = caster.ID,
            aoe = isAOE,
        };
        GMSGManager.Instance.SendToServer(msg);
    }
}