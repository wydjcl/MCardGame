using System.Collections.Generic;
using UnityEngine;
using Mirror;
using Unity.VisualScripting;
using System;

public class BattleManager : MonoBehaviour
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

        SpawnPlayer();
        SpawnEnemy();
        GMSGManager.Instance.battleManager = this;
    }

    public void SpawnPlayer()
    {
        //if (!NetworkServer.active)
        //{
        //    return;
        //}

        //int ii = -1;
        //foreach (var conn in NetworkServer.connections.Values)
        //{
        //    var p = Instantiate(playerCharater, playerZone.transform);
        //    var pc = p.GetComponent<PlayerCharacter>();
        //    pc.player = conn.identity.GetComponent<Player>();
        //    //pc.SPG.transform.position = new Vector3(i * 5.5f, 0, 0);
        //    //pc.cardList = conn.identity.GetComponent<Player>().cardList;

        //    //pc.datas= GGameManager.Instance.GetPlayerData(conn).datas;
        //    NetworkServer.Spawn(p, conn);//绑定到conn上
        //    ii += 2;
        //    //pc.InitPlayerCharacter();//客户端不执行,在spawn后在start里执行
        //}
        //Debug.Log("battlemanager创建角色完成");

        //for (int i = 0; i < GMSGManager.Instance.playerCount; i++)
        //{
        //    var p = Instantiate(playerCharater, playerZone.transform);
        //    var pc = p.GetComponent<PlayerCharacter>();
        //    pc.player
        //}
        Debug.Log("battlemanager");
        int i = 0;
        foreach (var p in GMSGManager.Instance.playerList)
        {
            var pg = Instantiate(playerCharater, playerZone.transform);
            var pc = pg.GetComponent<PlayerCharacter>();
            characterList.Add(pc);
            if (p.isOwned)
            {
                pc.player = p;
            }
            pc.ID = i;
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
        GetOwnePlayer().DrawCard(3);
    }

    public void PlayerTurnEnd()
    {
        GetOwnePlayer().DiscardAllCard();
    }
}