using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffSO : ScriptableObject
{
    public virtual void Apply(Character target, Buff buff)
    { }

    public virtual void Remove(Character target, Buff buff)
    { }

    public virtual void OnTurnStart(Character target, Buff buff)
    { }

    public virtual void OnTurnEnd(Character target, Buff buff)
    { }
}

public class Buff
{
    public BuffSO SO;
    public int power;
}