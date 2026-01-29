using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyNetWorkManager : NetworkManager
{
    public override void OnServerConnect(NetworkConnectionToClient conn)
    {
        base.OnServerConnect(conn);
        Invoke(nameof(SendPlayerCount), 0.7f);
    }

    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    {
        base.OnServerAddPlayer(conn);
        Invoke(nameof(ChangeID), 0.15f);
    }

    public void ChangeID()
    {
        foreach (var conn in NetworkServer.connections.Values)
        {
            conn.identity.GetComponent<Player>().playerID = conn.connectionId;
        }
    }

    public override void OnServerDisconnect(NetworkConnectionToClient conn)
    {
        GMSGManager.Instance.playerList.Remove(conn.identity.GetComponent<Player>());
        base.OnServerDisconnect(conn);
        Invoke(nameof(SendPlayerCount), 0.7f);
    }

    private void SendPlayerCount()
    {
        Debug.Log("有玩家进出，当前连接数：" + NetworkServer.connections.Count);
        var msg = new CountPlayerNum
        {
            playerCount = NetworkServer.connections.Count
        };
        GMSGManager.Instance.SendToServer(msg);
    }

    [ContextMenu("测试")]
    public void Test()
    {
        Debug.Log("玩家连接数：" + GMSGManager.Instance.playerCount);
    }
}