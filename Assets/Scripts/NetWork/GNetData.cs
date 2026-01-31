using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 用来服务器存储共同通信的数据
/// </summary>
public class GNetData : NetworkSingleton<GNetData>
{
    public int playerCount;
    public List<Player> playerList;
}