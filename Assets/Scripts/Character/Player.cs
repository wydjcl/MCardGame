using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : NetworkBehaviour
{
    [SyncVar(hook = nameof(ChangeID))]
    public int playerID;

    [SyncVar]
    public int HP;

    [SyncVar]
    public int MaxHP;

    [SyncVar]
    public int MP;

    [SyncVar]
    public int MaxMP;

    [SyncVar]
    public List<string> cardStringList;

    public List<CardDataSO> cardList;

    [ContextMenu("测试")]
    public void Test()
    {
        if (isOwned)
        {
            Debug.Log("这个是本玩家,ID:" + playerID);
        }
        else
        {
            Debug.Log("这个是其他玩家,ID:" + playerID);
        }
        Debug.Log("我的playerid" + NetworkClient.localPlayer.GetComponent<Player>().playerID);
    }

    public void ChangeID(int oldID, int newID)
    {
        // Debug.Log($"玩家ID变化: {oldID} -> {newID}");
    }

    [ContextMenu("改变本地玩家数据")]
    public void ChangePlayerData()
    {
        PlayerMSG msg = new PlayerMSG
        {
            playerID = playerID,
            HP = 900,
            MaxHP = 999
        };
        GMSGManager.Instance.SendToServer(msg);
    }

    public void Start()
    {
        GNetData.Instance.playerList.Add(this);
    }
}