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
            if (orderObj is CountOrderObj && !(orderObj is TimeLimitOrderObj) && !(orderObj is ReagonLimitOrderObj))
            {
                var _orderObj = orderObj as CountOrderObj;
                var _order = new CountOrder(_orderObj);
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


    public void UpdateOrderAfterCatchFish(FishRarity rarity, FishReagon reagon)
    {
        //if (ordersInGame.Any(order => order is ItemUseCountOrder))
        //    StartItemCount();
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


    private TypeNeed required;
    internal int count = 0;
    private List<FishRecord> lastFishs;
    public CountOrder(CountOrderObj orderObj)
    {
        GetOrderBase(orderObj);
        requiredNum = orderObj.requirdCount;
        required = orderObj.rule;
        lastFishs = new List<FishRecord>();
    }

    internal void AddCount(Fish fish)
    {
        if (isUpdated)
            return;
        CheckTargetFish(fish);
        count++;
        if (count >= requiredNum)
        {
            OrderManager.instance.UpdateOrderInfo(this);
            isUpdated = true;
        }
    }

    internal void CheckTargetFish(Fish fish)
    {       
        if (lastFishs.Count > 0)
        {
            switch (required)
            {
                case TypeNeed.DifferentRarity:
                    bool sameRarity = lastFishs.Any(record => record.rarity == fish.rarity);
                    if (sameRarity)
                    {
                        lastFishs.Clear();
                        count = 0;                        
                    }
                    break;
                case TypeNeed.DifferentFish:
                    bool sameFish = lastFishs.Any(record => record.iD == fish.fishID);
                    if (sameFish)
                    {
                        lastFishs.Clear();
                        count = 0;
                    }                    
                    break;
            }
        }
        lastFishs.Add(new FishRecord(fish));

    }

}

public class FishRecord
{
    internal string iD;
    internal FishRarity rarity;
    public FishRecord(Fish fish)
    {
        iD = fish.fishID;
        rarity = fish.rarity;
    }
}

//public class ItemUseCountOrder : CountOrder
//{


//    //public ItemUseCountOrder(int _requiedNum)
//    //{
//    //    requiredNum = _requiedNum;
//    //}
//}

public class ReagonOrder : Order
{

}


public class TimeLimitOrder : Order
{

}






