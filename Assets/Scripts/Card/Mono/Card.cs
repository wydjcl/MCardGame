using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;

public class Card : MonoBehaviour, IPointerClickHandler
{
    [Header("UI层")]
    public GameObject Entry;

    public TextMeshPro costText;
    public TextMeshPro nameText;
    public TextMeshPro desText;
    public SpriteRenderer cardSprite;
    public int orSortingGroup;

    [Header("Pos")]
    public bool isAni = true;

    public Vector3 originalPosition;
    public Quaternion originalRotation;
    public int originaLayerOrder;//原始叠层排序

    [Header("数据层")]
    public CardDataSO data;

    public int cardCost;

    public CardType cardType;

    public BattleManager battleManager;

    public PlayerCharacter player;

    public void OnPointerClick(PointerEventData eventData)
    {
    }

    private void Awake()
    {
    }

    [ContextMenu("读取数据")]
    public void ReadData()
    {
        battleManager = FindObjectOfType<BattleManager>();
        if (data != null)
        {
            nameText.text = data.cardName;
            cardCost = data.cardCost;
            costText.text = cardCost.ToString();

            cardSprite.sprite = data.cardSprite;
            cardType = data.cardType;
            ChangeDes();
        }
    }

    public void UpdatePosRot(Vector3 pos, Quaternion rot)
    {
        originalPosition = pos;
        originalRotation = rot;
        originaLayerOrder = GetComponent<SortingGroup>().sortingOrder;
    }

    public void ResetPos()
    {
        transform.position = originalPosition;
        transform.rotation = originalRotation;
        GetComponent<SortingGroup>().sortingOrder = originaLayerOrder;
    }

    public void Effect(Character caster, Character target, Card card)
    {
        // Debug.Log("执行效果caster:" + caster.gameObject.name + "target:" + target.gameObject.name);
        //player.MP -= card.cardCost;
        var msg = new ChangeMP
        {
            playerID = player.ID,
            deleteAmout = card.cardCost,
            isFull = false,
        };
        GMSGManager.Instance.SendToServer(msg);
        foreach (var effect in data.cardEffects)
        {
            effect.ApplyEffect(caster, target, this);
        }
        player.UpdateUI();
    }

    public string GetDescription(string s)
    {
        string desc = s;
        //desc = desc.Replace("{damage}", player.attack.ToString());
        //desc = desc.Replace("{damage+6}", (player.attack + 6).ToString());
        // desc = desc.Replace("{mana}", manaCost.ToString());
        return desc;
    }

    public void ChangeDes()
    {
        //desText.text = GetDescription(data.cardDes);
    }
}