using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using System;

public class OrderManager : MonoBehaviour
{
    public static OrderManager instance;

    [SerializeField] private OrderObj[] allOrders;
    public List<Order> ordersInGame;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this.gameObject);
            return;
        }
        DontDestroyOnLoad(this.gameObject);
        instance = this;
    }

    public void GetOrders()
    {
        System.Random random = new System.Random();
        ordersInGame = new List<Order>();
        var selectedOrders = allOrders.OrderBy(order => random.Next()).Take(3).ToArray();
        foreach (var orderObj in selectedOrders)
        {
            if (orderObj is SimpleOrderObj)
            {
                var _order= new SimpleCountOrder((SimpleOrderObj)orderObj);
                ordersInGame.Add(_order);
                GamePlay.CatchedFish += _order.AddCount;
            }     
        }
        ShowOrderInfo(ordersInGame);
    }

    private void ShowOrderInfo(List<Order> ordersInGame)
    {
        OrderShow.instance.ShowOrderInfo(ordersInGame);
    }

    public void UpdateOrderInfo(Order order)
    {
        int index = ordersInGame.IndexOf(order);
        OrderShow.instance.ShowOrderFinish(index);
    }


    public void UpdateOrderAfterCatchFish(fishRarity rarity, fishReagon reagon)
    {
        if (ordersInGame.Any(order => order is ItemUseCountOrder))
            StartItemCount();
    }

    




    private void StartItemCount()
    {

    }
}


public class Order
{
    internal string orderName;
    internal string orderInfo;
    internal int rewardMoney;
    internal int rewardExp;
    internal bool isUpdated = false;
    internal void GetOrderBase(OrderObj orderObj)
    {
        orderName = orderObj.orderName;
        orderInfo = orderObj.orderInfo;
        rewardMoney = orderObj.rewardMoney;
        rewardExp = orderObj.rewardExp;
    }
}



public class CountOrder : Order
{
    internal int requiredNum;
    internal int count = 0;
    internal void AddCount(Fish fish)
    {
        if (isUpdated)
            return;
        if (!IsTargetFish())
            return;
        count++;

        if (count >= requiredNum)
        {
            OrderManager.instance.UpdateOrderInfo(this);
            isUpdated = true;
        }
    }

    internal virtual bool IsTargetFish()
    {
        return true;
    }

}

public class SimpleCountOrder : CountOrder
{
   
   
   
    public SimpleCountOrder(SimpleOrderObj orderObj)
    {
        GetOrderBase(orderObj);
        requiredNum = orderObj.requirdCount;
    }
    public void AddCount()
    {
      
    }

}

public class ItemUseCountOrder : CountOrder
{
  
   
    public ItemUseCountOrder(int _requiedNum)
    {
        requiredNum = _requiedNum;
    }
}

public class ReagonOrder : Order
{

}


public class TimeLimitOrder : Order
{

}




public enum orderType { fishCount, }

