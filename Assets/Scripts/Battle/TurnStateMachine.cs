using Mirror;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TurnStateMachine : MonoBehaviour
{
    public BattleManager battleManager;
    public TurnState currentState = TurnState.None;
    public int agreeCount = 0;
    public bool agree;
    public TextMeshProUGUI turnButtomText;

    private void Start()
    {
        //EnterNextTurnState();
        GMSGManager.Instance.turnStateMachine = this;
        Debug.Log("开始游戏的时候" + GMSGManager.Instance.playerCount);
        turnButtomText.text = "结束回合0/" + GMSGManager.Instance.playerCount;
    }

    [ContextMenu("进入下一阶段")]
    public void EnterNextTurnState()
    {
        currentState = (TurnState)((int)currentState + 1);
        if (((int)currentState) >= 6)
        {
            currentState = TurnState.PlayerTurnStart;
        }
        //Debug.Log("进入" + currentState.ToString());
        if (currentState == TurnState.PlayerTurnStart)
        {
            battleManager.PlayerTurnBegin();
            Debug.Log("进入玩家回合");
            turnButtomText.text = "结束回合0/" + GMSGManager.Instance.playerCount;
            EnterNextTurnState();
            return;
        }
        if (currentState == TurnState.PlayerTurnEnd)
        {
            battleManager.PlayerTurnEnd();
            EnterNextTurnState();
            return;
        }
        if (currentState == TurnState.EnemyTurnStart)
        {
            EnterNextTurnState();
            return;
        }
        if (currentState == TurnState.EnemyTurn)
        {
            //battleManager.EnemyTurn();
            EnterNextTurnState();
            return;
        }
        if (currentState == TurnState.EnemyTurnEnd)
        {
            EnterNextTurnState();
            return;
        }
    }

    public void EndTurn()
    {
        if (currentState == TurnState.PlayerTurn)
        {
            agree = !agree;
            var msg = new ArgeeEndTurn
            {
                agreeBool = agree
            };
            GMSGManager.Instance.SendToServer(msg);
        }
        else
        {
            Debug.Log("不在你的回合");
        }
    }
}