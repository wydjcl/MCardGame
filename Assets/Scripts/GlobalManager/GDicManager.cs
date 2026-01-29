using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GDicManager : SingletonMono<GDicManager>
{
    public List<CardDataSO> CardDataDic = new List<CardDataSO>();
}