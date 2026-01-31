using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;

public class CardPhysicallyEffect : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler
{
    [HideInInspector]
    public Card card;

    public bool isDrag;

    [HideInInspector]
    public bool canEcecute;

    public GameObject arrowPrefab;
    private GameObject currentArrow;
    public Character target;

    private void Awake()
    {
        card = GetComponent<Card>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        //if (card.player.MP < card.cardCost)
        //{
        //    return;
        //}
        if (card.isAni)
        {
            return;
        }
        card.GetComponent<SortingGroup>().sortingOrder = 50;
        // card.Entry.transform.position = card.originalPosition;
        if (card.cardType == CardType.AttackCard || card.cardType == CardType.PAffectCard)
        {
            currentArrow = Instantiate(arrowPrefab, transform);
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        //if (card.player.MP < card.cardCost)
        //{
        //    return;
        //}
        if (card.isAni)
        {
            return;
        }
        isDrag = true;
        card.Entry.transform.position = card.transform.position;
        if (card.cardType != CardType.AttackCard && card.cardType != CardType.PAffectCard)
        {
            Vector3 screenPos = new(Input.mousePosition.x, Input.mousePosition.y, 10);// 屏幕坐标0-1920
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(screenPos);
            card.transform.position = worldPos;//世界坐标 0-20
            canEcecute = worldPos.y > -1.8f;//拉到1f上就用用掉
            //target = card.battleManager.GetMainPlayer();
        }
        else
        {
            if (eventData.pointerEnter != null)
            {
                if (eventData.pointerEnter.CompareTag("Enemy") && card.cardType == CardType.AttackCard)
                {
                    if (eventData.pointerEnter.GetComponent<Enemy>() != null)
                    {
                        target = eventData.pointerEnter.GetComponent<Enemy>();
                        canEcecute = true;
                    }
                }
                if (eventData.pointerEnter.CompareTag("Player") && card.cardType == CardType.PAffectCard)
                {
                    if (eventData.pointerEnter.GetComponent<PlayerCharacter>() != null)
                    {
                        target = eventData.pointerEnter.GetComponent<PlayerCharacter>();
                        canEcecute = true;
                    }
                }
            }
            else
            {
                target = null;
                canEcecute = false;
            }
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        isDrag = false;
        card.GetComponent<SortingGroup>().sortingOrder = card.orSortingGroup;
        if (currentArrow != null)
        {
            Destroy(currentArrow.gameObject);
        }
        if (canEcecute)
        {
            //card.BoardEffect(card.player, target, card);
            card.Effect(card.player, target, card);
            //card.player.MP -= card.cardCost;
            //card.player.UpdateUI();
            target = null;
        }
        card.ResetPos();
        canEcecute = false;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (card.isAni)//防止抽卡途中扰乱Entry坐标
        {
            return;
        }
        if (!isDrag)
        {
            card.Entry.transform.position = card.originalPosition + Vector3.up * 1.1f;
        }
        //  Debug.Log()
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (card.isAni)//防止抽卡途中扰乱Entry坐标
        {
            return;
        }
        card.Entry.transform.position = card.originalPosition;
    }
}