using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GClientPlayerManager : NetworkSingleton<GClientPlayerManager>
{
    public void ChangeYLSB()
    {
        var p = NetworkClient.localPlayer.GetComponent<Player>();
        PlayerMSG msg = new PlayerMSG
        {
            playerID = p.playerID,
            HP = 30,
            MaxHP = 35,
            MP = 5,
            MaxMP = 5,
        };
        GMSGManager.Instance.SendToServer(msg);
    }

    public void ChangeXEWY()
    {
        var p = NetworkClient.localPlayer.GetComponent<Player>();
        PlayerMSG msg = new PlayerMSG
        {
            playerID = p.playerID,
            HP = 75,
            MaxHP = 80,
            MP = 5,
            MaxMP = 5,
        };
        GMSGManager.Instance.SendToServer(msg);
    }
}