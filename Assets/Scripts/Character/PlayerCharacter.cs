using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using DG.Tweening;
using TMPro;

public class PlayerCharacter : Character
{
    [HideInInspector]
    public bool isDrag;

    public Player player;

    [Header("场景块")]
    public GameObject cardPrefab;

    public GameObject CardZone;

    public TextMeshPro text;
    public CardLayout cardLayout;
    public BoxCollider2D boxColl;
    public TextMeshPro HPText;
    public TextMeshPro MPText;
    public TextMeshPro DSText;
    public GameObject arrowIcon;

    [Header("牌组")]
    public List<CardDataSO> cardList;

    public List<Card> drawDeckList = new List<Card>();
    public List<Card> handDeckList = new List<Card>();
    public List<Card> discardDeckList = new List<Card>();
    public List<Card> removeDeckList = new List<Card>();

    [Header("玩家数据")]
    public int MP;

    public int MaxMP;

    public override void Awake()
    {
        base.Awake();
        cardLayout = FindObjectOfType<CardLayout>();
        var zone = GameObject.Find("PlayerZone");
        if (zone != null)
        {
            transform.SetParent(zone.transform, false);
        }
    }

    public override void Start()
    {
        base.Start();
        //InitPlayerCharacter();
    }

    public override void OnStartClient()
    {
        base.OnStartClient();

        HP = player.HP;
        MaxHP = player.MaxHP;
    }

    public void InitPlayerCharacter()
    {
        //Debug.Log("正在初始化角色...");
        CardZone = GameObject.Find("CardZone");
        ReadCardData();
        foreach (var conn in NetworkServer.connections.Values)
        {
            // Debug.Log(conn.identity.name);
            //Debug.Log(conn.identity.netId);
            //Debug.Log(NetworkServer.connections.Values.Count);
        }
        //DrawCard(3);

        if (NetworkClient.connection.identity.GetComponent<Player>() == player)
        {
            arrowIcon.SetActive(true);
        }
        else
        {
            //CardZone.SetActive(false);
            arrowIcon.SetActive(false);
        }

        UpdateUI();
        battleManager.characterList.Add(this);
        // Debug.Log("角色初始化成功");
    }

    public void ReadCardData()
    {
        cardList = NetworkClient.connection.identity.GetComponent<Player>().cardList;
        foreach (var c in cardList)
        {
            var cardP = Instantiate(cardPrefab, CardZone.transform);
            var card = cardP.GetComponent<Card>();
            card.data = c;
            card.player = this;
            card.ReadData();
            card.gameObject.SetActive(false);
            drawDeckList.Add(card);
        }
        ShuffleDrawDeck();
    }

    [ContextMenu("测试")]
    public void Test()
    {
        // DrawCard(3);
        Debug.Log(SPG.transform.position);
    }

    private void SetCardLayout(float delay)
    {
        for (int i = 0; i < handDeckList.Count; i++)
        {
            var currentCard = handDeckList[i];
            CardTransForm cardTransForm = cardLayout.GetCardTransForm(i, handDeckList.Count);
            currentCard.transform.DOScale(Vector3.one, 0.05f).SetDelay(delay).onComplete = () =>
            {
                currentCard.transform.DOMove(cardTransForm.pos, 0.1f).onComplete = () =>
                {
                    currentCard.isAni = false;
                };//
            };
            currentCard.GetComponent<SortingGroup>().sortingOrder = i;
            currentCard.orSortingGroup = i;
            currentCard.UpdatePosRot(cardTransForm.pos, cardTransForm.rotation);
        }
    }

    /// <summary>
    /// 抽牌
    /// </summary>
    /// <param name="amount"></param>
    public void DrawCard(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            if (drawDeckList.Count == 0)
            {
                RecycleCard();
            }
            if (drawDeckList.Count == 0)
            {
                Debug.Log("卡组空空");
                return;
            }
            Card currentCard = drawDeckList[0];
            drawDeckList.RemoveAt(0);
            currentCard.gameObject.SetActive(true);
            currentCard.transform.position = new Vector3(0, 0, 0);
            currentCard.isAni = true;
            handDeckList.Add(currentCard);
            var delay = i * 0.1f;
            SetCardLayout(delay);
        }
    }

    /// <summary>
    /// 洗抽牌堆
    /// </summary>
    public void ShuffleDrawDeck()
    {
        int n = drawDeckList.Count;
        for (int i = 0; i < n; i++)
        {
            int r = i + UnityEngine.Random.Range(0, n - i);
            Card temp = drawDeckList[r];
            drawDeckList[r] = drawDeckList[i];
            drawDeckList[i] = temp;
        }
    }

    /// <summary>
    /// 把弃牌堆洗回抽牌堆
    /// </summary>
    public void RecycleCard()
    {
        drawDeckList.AddRange(discardDeckList);
        discardDeckList.Clear();
        ShuffleDrawDeck();
    }

    public void DiscardCard(Card card)
    {
        discardDeckList.Add(card);
        handDeckList.Remove(card);
        card.gameObject.SetActive(false);
        SetCardLayout(0f);
    }

    /// <summary>
    /// 除外牌
    /// </summary>
    /// <param name="obj"></param>
    public void RemoveCard(Card card)
    {
        removeDeckList.Add(card);
        handDeckList.Remove(card);
        card.gameObject.SetActive(false);
        SetCardLayout(0f);
    }

    /// <summary>
    /// 弃置所有牌
    /// </summary>
    public void DiscardAllCard()
    {
        for (int i = 0; i < handDeckList.Count; i++)
        {
            discardDeckList.Add(handDeckList[i]);
            handDeckList[i].gameObject.SetActive(false);
        }
        handDeckList.Clear();
    }

    public override void SetEffectPos()
    {
        effectPos = SPG.transform.position;
        // boxColl.offset = effectPos;
    }

    public override void UpdateUI()
    {
        base.UpdateUI();
        HPText.text = "HP:" + HP + "/" + MaxHP;
        MPText.text = "MP:" + MP + "/" + MaxMP;
        DSText.text = "DS:" + Defense;
    }

    public override void TurnStart()
    {
        base.TurnStart();
        DrawCard(3);
        var msg = new ChangeMP
        {
            playerID = ID,
            isFull = true,
        };
        GMSGManager.Instance.SendToServer(msg);
        UpdateUI();
    }

    public override void TurnEnd()
    {
        base.TurnEnd();
        DiscardAllCard();
        UpdateUI();
    }
}