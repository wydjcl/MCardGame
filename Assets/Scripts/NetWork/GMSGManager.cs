using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GMSGManager : NetworkSingleton<GMSGManager>
{
    public BattleManager battleManager;
    public TurnStateMachine turnStateMachine;

    private void Start()
    {
    }

    public override void OnStartServer()
    {
        //服务端接收到,中转到每个客户端
        RegisterForward<CauseDamage>();
        RegisterForward<CauseHeal>();
        RegisterForward<ChangeMP>();
        RegisterForward<ArgeeEndTurn>();
        RegisterForward<CountPlayerNum>();
        NetworkServer.RegisterHandler<ReadPlayerDataQuest>(ServerSendPlayerData);//服务端接收客户端请求读取玩家数据

        NetworkServer.RegisterHandler<PlayerMSG>(ClientChangePlayerDataMSG);
    }

    public override void OnStartClient()
    {
        //客户端接受到进行处理
        RegisterClientHandler<CauseDamage>(OnCauseDamageMsg);
        RegisterClientHandler<CauseHeal>(OnCauseHeal);
        RegisterClientHandler<ChangeMP>(OnChangeMPMsg);
        RegisterClientHandler<ArgeeEndTurn>(OnArgeeEndTurn);
        RegisterClientHandler<CountPlayerNum>(OnCountPlayerNum);
        RegisterClientHandler<PlayerDataMSG>(OnReadPlayerDataQuest);
    }

    [ContextMenu("测试")]
    public void Text()
    {
        BroadcastMessage("0");
    }

    /// <summary>
    /// 服务端简易通知广播
    /// </summary>
    /// <param name="msg"></param>
    [Server]
    public new void BroadcastMessage(string msg)
    {
        RpcNotifyClients(msg);
    }

    /// <summary>
    /// 服务端调用这个方法的时候，所有客户端都会调用
    /// </summary>
    /// <param name="msg"></param>
    [ClientRpc]
    public void RpcNotifyClients(string msg)
    {
        if (msg == "0")//开始战斗
        {
            GSceneManager.Instance.StartBattleScene();
        }
        if (msg == "下一回合")//开始回合
        {
            turnStateMachine.EnterNextTurnState();
        }
    }

    /// <summary>
    /// 客户端发送消息到服务器
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="msg"></param>
    public void SendToServer<T>(T msg) where T : struct, NetworkMessage
    {
        NetworkClient.Send(msg);
    }

    /// <summary>
    /// 服务器发送消息到所有客户端
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="msg"></param>
    public void SendToAllClient<T>(T msg) where T : struct, NetworkMessage
    {
        NetworkServer.SendToAll(msg);
    }

    /// <summary>
    /// 服务端注册：接收并转发给所有客户端
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public static void RegisterForward<T>() where T : struct, NetworkMessage
    {
        NetworkServer.RegisterHandler<T>((conn, msg) =>
        {
            //  Debug.Log($"[Server] 收到 {typeof(T).Name},来自连接ID{conn.connectionId}"); // 一键转发给所有客户端
            NetworkServer.SendToAll(msg);
        }
        );
    }

    /// <summary>
    /// 客户端注册：接收服务器转发的消息
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="handler"></param>
    public static void RegisterClientHandler<T>(Action<T> handler) where T : struct, NetworkMessage
    { NetworkClient.RegisterHandler<T>(handler); }

    private void OnCountPlayerNum(CountPlayerNum msg)
    {
        GNetData.Instance.playerCount = msg.playerCount;
    }

    private void OnArgeeEndTurn(ArgeeEndTurn msg)
    {
        // Debug.Log($"客户端收到 一个{msg.agreeBool}");
        if (msg.agreeBool)
        {
            turnStateMachine.agreeCount++;

            if (turnStateMachine.agreeCount >= GNetData.Instance.playerCount)
            {
                Debug.Log("全票同意通过到下一回合");
                turnStateMachine.EnterNextTurnState();
                turnStateMachine.agree = false;
                turnStateMachine.agreeCount = 0;
            }
        }
        else
        {
            turnStateMachine.agreeCount--;
        }
        if (turnStateMachine.agree)
        {
            turnStateMachine.turnButtomText.text = ($"已确认{turnStateMachine.agreeCount}/{GNetData.Instance.playerCount}");
        }
        else
        {
            turnStateMachine.turnButtomText.text = ($"结束回合{turnStateMachine.agreeCount}/{GNetData.Instance.playerCount}");
        }
    }

    private void OnChangeMPMsg(ChangeMP msg)
    {
        Character c = null;
        PlayerCharacter player = null;
        foreach (var character in battleManager.characterList)
        {
            if (character.ID == msg.playerID)
            {
                c = character;
                player = c as PlayerCharacter;
            }
        }
        if (msg.isFull)
        {
            player.MP = player.MaxMP;
        }
        else
        {
            player.MP -= msg.deleteAmout;
        }
        player.UpdateUI();
    }

    private void OnCauseDamageMsg(CauseDamage msg)
    {
        // Debug.Log($"客户端收到 伤害数: {msg.damageAmount}, 来源ID{msg.casterID} -> 目标ID{msg.targetID}");
        Character caster = null;
        Character target = null;
        foreach (var character in battleManager.characterList)
        {
            if (character.ID == msg.casterID)
            {
                caster = character;
            }
            if (character.ID == msg.targetID)
            {
                target = character;
            }
        }

        caster.CauseDamage(target, msg.damageAmount);
    }

    private void OnCauseHeal(CauseHeal msg)
    {
        // Debug.Log($"客户端收到 治疗数: {msg.healAmount}, 来源ID{msg.casterID} -> 目标ID{msg.targetID}");
        Character caster = null;
        Character target = null;
        foreach (var character in battleManager.characterList)
        {
            if (character.ID == msg.casterID)
            {
                caster = character;
            }
            if (character.ID == msg.targetID)
            {
                target = character;
            }
        }
        caster.CauseHeal(target, msg.healAmount);
    }

    /// <summary>
    /// 服务器给客户端转发角色信息
    /// </summary>
    /// <param name="msg"></param>
    public void ServerSendPlayerData(NetworkConnectionToClient conn, ReadPlayerDataQuest msg)
    {
        var msg2 = new PlayerDataMSG();
        msg2.playerCount = GNetData.Instance.playerCount;
        msg2.playerIDs = new List<int>();
        foreach (var p in GNetData.Instance.playerList)
        {
            msg2.playerIDs.Add(p.playerID);
        }
        conn.Send(msg2);
    }

    public void OnReadPlayerDataQuest(PlayerDataMSG msg)
    {
        //  Debug.Log("客户端收到 请求读取玩家数据");
        // Debug.Log($"当前玩家数量:{msg.playerCount}");
        foreach (var id in msg.playerIDs)
        {
            // Debug.Log($"玩家ID:{id}");
        }
    }

    public void ClientChangePlayerDataMSG(NetworkConnectionToClient conn, PlayerMSG msg)
    {
        foreach (var p in GNetData.Instance.playerList)
        {
            if (p.playerID == msg.playerID)
            {
                p.HP = msg.HP;
                p.MaxHP = msg.MaxHP;
                p.MP = msg.MP;
                p.MaxMP = msg.MaxMP;
            }
        }
    }
}