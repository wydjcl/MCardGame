using System.Collections.Generic;
using UnityEngine;
using Mirror;
using Unity.VisualScripting;
using System;

public class BattleManager : NetworkBehaviour
{
    public Canvas canvas;
    public GameObject playerCharater;
    public GameObject playerZone;
    public TurnStateMachine turnStateMachine;
    public List<Character> characterList;
    public List<GameObject> enemies;

    private void Start()
    {
        canvas.worldCamera = Camera.main;
        SeverStart();
        ClientQuestReadPlayerData();
        SpawnPlayer();
        SpawnEnemy();
        GMSGManager.Instance.battleManager = this;
    }

    /// <summary>
    /// 服务器启动调用
    /// </summary>
    public void SeverStart()
    {
        if (!isServer)
        {
            return;
        }
        //Debug.Log("服务器调用");
    }

    public void ClientQuestReadPlayerData()
    {
        if (!isClient)
        {
            return;
        }
        var msg = new ReadPlayerDataQuest();
        GMSGManager.Instance.SendToServer(msg);
    }

    public void SpawnPlayer()
    {
        if (GNetData.Instance.playerList.Count == 1)
        {
            Debug.Log("单人游戏");
        }
        else
        {
            Debug.Log("双人游戏战斗场景创建角色中...");
        }
        int i = 0;
        foreach (var player in GNetData.Instance.playerList)
        {
            var p = Instantiate(playerCharater, playerZone.transform);
            var pc = p.GetComponent<PlayerCharacter>();
            pc.player = player;
            pc.ID = i;
            pc.HP = player.HP;
            pc.MaxHP = player.MaxHP;
            pc.MP = player.MP;
            pc.MaxMP = player.MaxMP;
            if (i == 0)
            {
                pc.transform.position = new Vector3(-7.4f, -1.6f, 0);
            }
            if (i == 1)
            {
                pc.transform.position = new Vector3(-4.32f, -1.6f, 0);
            }
            pc.InitPlayerCharacter();
            i++;
        }
        Invoke(nameof(StartTurn), 0.5f);
    }

    private void StartTurn()
    {
        //Debug.Log("battlemanager开始控制回合");
        if (NetworkServer.active)
        {
            GMSGManager.Instance.BroadcastMessage("下一回合");
        }
    }

    public void SpawnEnemy()
    {
        foreach (var enemy in enemies)
        {
            var e = Instantiate(enemy, playerZone.transform);
            var ee = e.GetComponent<Enemy>();
            characterList.Add(ee);
        }
    }

    public PlayerCharacter GetOwnePlayer()
    {
        //Debug.Log(characterList.Count);
        foreach (var character in characterList)
        {
            if (!character.isEnemy)
            {
                var p = character as PlayerCharacter;
                if (p.player != null)
                {
                    if (p.player.isOwned)
                    {
                        return p;
                    }
                }
            }
        }
        Debug.LogWarning("没找到角色!!!");
        return null;
    }

    public void PlayerTurnBegin()
    {
        GetOwnePlayer().TurnStart();
    }

    public void PlayerTurnEnd()
    {
        GetOwnePlayer().TurnEnd();
        GetOwnePlayer().DiscardAllCard();
    }
}