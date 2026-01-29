using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CardData_", menuName = "SO/新卡牌")]
public class CardDataSO : ScriptableObject
{
    public string cardName;
    public int cardCost;
    public CardType cardType;
    public Sprite cardSprite;
    public List<CardEffectSO> cardEffects;

    [TextArea]
    public string cardDes;
}