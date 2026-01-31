using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MSG : MonoBehaviour
{
}

public struct StartMGame
{
}

public struct CountPlayerNum : NetworkMessage
{
    public int playerCount;
}

/// <summary>
/// 是否同意结束回合
/// </summary>
public struct ArgeeEndTurn : NetworkMessage
{
    public bool agreeBool;
}

public struct CauseDamage : NetworkMessage
{
    public int casterID;
    public int targetID;
    public int damageAmount;
    public bool aoe;
}

public struct CauseHeal : NetworkMessage
{
    public int casterID;
    public int targetID;
    public int healAmount;
    public bool aoe;
}

public struct ReadPlayerDataQuest : NetworkMessage
{
}

public struct PlayerDataMSG : NetworkMessage
{
    public int playerCount;
    public List<int> playerIDs;
}

/// <summary>
/// 玩家数据包
/// </summary>
public struct PlayerMSG : NetworkMessage
{
    public int playerID;
    public int HP;
    public int MaxHP;
    public int MP;
    public int MaxMP;
}

/// <summary>
/// 改变法力值
/// </summary>
public struct ChangeMP : NetworkMessage
{
    public int playerID;
    public int deleteAmout;
    public bool isFull;
}