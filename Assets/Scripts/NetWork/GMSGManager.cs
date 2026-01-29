using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GMSGManager : NetworkSingleton<GMSGManager>
{
    public BattleManager battleManager;
    public TurnStateMachine turnStateMachine;
    public int playerCount;
    public List<Player> playerList;

    private void Start()
    {
    }

    public override void OnStartServer()
    {
        //服务端接收到,中转到每个客户端
        RegisterForward<CauseDamage>();
        RegisterForward<CauseHeal>();
        RegisterForward<ArgeeEndTurn>();
        RegisterForward<CountPlayerNum>();
    }

    public override void OnStartClient()
    {
        //客户端接受到进行处理
        RegisterClientHandler<CauseDamage>(OnCauseDamageMsg);
        RegisterClientHandler<CauseHeal>(OnCauseHeal);
        RegisterClientHandler<ArgeeEndTurn>(OnArgeeEndTurn);
        RegisterClientHandler<CountPlayerNum>(OnCountPlayerNum);
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
        playerCount = msg.playerCount;
    }

    private void OnArgeeEndTurn(ArgeeEndTurn msg)
    {
        // Debug.Log($"客户端收到 一个{msg.agreeBool}");
        if (msg.agreeBool)
        {
            turnStateMachine.agreeCount++;

            if (turnStateMachine.agreeCount >= playerCount)
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
            turnStateMachine.turnButtomText.text = ($"已确认{turnStateMachine.agreeCount}/{playerCount}");
        }
        else
        {
            turnStateMachine.turnButtomText.text = ($"结束回合{turnStateMachine.agreeCount}/{playerCount}");
        }
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
}