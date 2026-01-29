using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "弃置此牌", menuName = "SO/卡牌效果/弃置此牌")]
public class DiscardCardEffect : CardEffectSO
{
    public override void ApplyEffect(Character caster, Character target, Card card)
    {
        var p = caster as PlayerCharacter;
        p.DiscardCard(card);
    }
}