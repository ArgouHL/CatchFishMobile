using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class OrderShow : MonoBehaviour
{
    [SerializeField] private CanvasGroup orderInfosCanvas;
    [SerializeField] private TMP_Text[] orderInfos;
    [SerializeField] private CanvasGroup orderHintsCanvas;
    [SerializeField] private TMP_Text[] orderHints;
    public static OrderShow instance;
    private List<Order> _ordersInGame;
    private void Awake()
    {

        instance = this;
    }

    public void pressGameStart()
    {
        GamePlay.instance.GamePreStart();
        HideOrderInfo();
        ShowOrderHints();
    }

    internal void ShowOrderInfo(List<Order> ordersInGame)
    {
        _ordersInGame = ordersInGame;
        UIHelper.ShowAndClickable(orderInfosCanvas, true);
        for (int i = 0; i < 3; i++)
        {
            orderInfos[i].text = ordersInGame[i].orderInfo + "\n" + "¼úÀy:" + ordersInGame[i].rewardMoney + "ª÷,¸gÅç­È+" + ordersInGame[i].rewardExp;
            orderHints[i].text = ordersInGame[i].orderInfo;
        }
    }

    internal void HideOrderInfo()
    {
        UIHelper.ShowAndClickable(orderInfosCanvas, false);
    }

    internal void ShowOrderHints()
    {
        UIHelper.ShowAndClickable(orderHintsCanvas, true);
    }

    internal void ShowOrderFinish(int index)
    {
        orderHints[index].text = "<s>" + _ordersInGame[index].orderInfo + "</s>";
    }
}
