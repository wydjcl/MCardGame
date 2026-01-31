using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "SO/PlayerDataSO")]
public class PlayerDataSO : ScriptableObject
{
    public int HP;
    public int MaxHP;
    public int MP;
    public int MaxMP;
}