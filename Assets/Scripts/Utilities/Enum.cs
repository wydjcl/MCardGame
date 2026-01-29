public enum CardType
{
    PetCard,//宠物卡
    AttackCard,//攻击卡
    AbilityCard,//效果卡
    PAffectCard,//宠物效果卡
    SpecialCard,
    None
}

public enum TurnState
{
    PlayerTurnStart,
    PlayerTurn,
    PlayerTurnEnd,
    EnemyTurnStart,
    EnemyTurn,
    EnemyTurnEnd,
    None
}

public enum EquipmentType
{
    /// <summary>
    /// 头盔
    /// </summary>
    Helmet,

    /// <summary>
    /// 胸甲
    /// </summary>
    Cuirass,

    /// <summary>
    /// 武器
    /// </summary>
    Weapon,

    /// <summary>
    /// 鞋子
    /// </summary>
    Shoe
}

public enum RoomNodeType
{
    Start,
    SmallEnemy,
    BigEnemy,
    Shop,
    Event,
    Bar,
    Treasure,
    Boss,
    None
}

public enum RoomStage
{
    Visited,//已访问
    CanVisit,//未访问但是可以访问
    CantVisit//不可访问
}

/// <summary>
/// 游戏当前所在阶段或者说场景
/// </summary>
public enum GameState
{
    Map,
    Bar,
    Battle,
    Shop,
    Treasure,
    Delete,
    Event,
    CardBag,
}

public enum Quality
{
    Ordinary,
    Excellent,
    Rare,
    Epic,
    Legendary,
    Mythical
}