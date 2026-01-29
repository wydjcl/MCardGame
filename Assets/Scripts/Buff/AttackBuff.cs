using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "力量buff", menuName = "SO/Buff/力量buff")]
public class AttackBuff : BuffSO
{
    public override void Apply(Character target, Buff buff)
    {
        target.attack += buff.power;
    }

    public override void Remove(Character target, Buff buff)
    {
        target.attack -= buff.power;
    }
}