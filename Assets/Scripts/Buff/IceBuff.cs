using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "冻结buff", menuName = "SO/Buff/冻结buff")]
public class IceBuff : BuffSO
{
    public override void Apply(Character target, Buff buff)
    {
        target.ice += buff.power;
    }

    public override void Remove(Character target, Buff buff)
    {
        target.ice -= buff.power;
    }

    public override void OnTurnEnd(Character target, Buff buff)
    {
        buff.power -= 1;
        target.ice -= 1;
    }
}