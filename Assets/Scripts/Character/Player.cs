using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : NetworkBehaviour
{
    [SyncVar(hook = nameof(ChangeID))]
    public int playerID;

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
        Debug.Log($"玩家ID变化: {oldID} -> {newID}");
    }

    private void Start()
    {
        GMSGManager.Instance.playerList.Add(this);
    }
}