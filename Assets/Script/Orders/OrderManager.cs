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

    internal void GetOrders()
    {
        System.Random random = new System.Random();
        ordersInGame = new List<Order>();
        var selectedOrders = allOrders.OrderBy(order => random.Next()).Take(3).ToArray();
        foreach (var orderObj in selectedOrders)
        {
            if(orderObj is TimeLimitOrderObj)
            {
                var _orderObj = orderObj as TimeLimitOrderObj;
                var _order = new TimeLimitOrder(_orderObj);
                ordersInGame.Add(_order);
                GamePlay.CatchedFish += _order.AddCount;
            }
            else if(orderObj is CountOrderObj && !(orderObj is TimeLimitOrderObj) && !(orderObj is ReagonLimitOrderObj))
            {
                var _orderObj = orderObj as CountOrderObj;
                var _order = new CountOrder(_orderObj);
                ordersInGame.Add(_order);
                GamePlay.CatchedFish += _order.AddCount;
            }
            else if (orderObj is ItemUseCountOrderObj)
            {
                var _orderObj = orderObj as ItemUseCountOrderObj;
                var _order = new ItemUseCountOrder(_orderObj);
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

    internal void UpdateOrderInfo(Order order)
    {
        int index = ordersInGame.IndexOf(order);
        OrderShow.instance.ShowOrderFinish(index);
    }


    internal void UpdateOrderAfterCatchFish(FishRarity rarity, FishReagon reagon)
    {
        //if (ordersInGame.Any(order => order is ItemUseCountOrder))
        //    StartItemCount();
    }


    internal void ResetItemCount(ItemType itemType)
    {
        foreach (var order in ordersInGame)
        {
            if (order is ItemUseCountOrder)
            {
                var _order = order as ItemUseCountOrder;
                if (_order.itemType == itemType)
                    order.count = 0;
            }
        }
    }

    internal void TimeCount(TimeLimitOrder timeLimitOrder)
    {
        StartCoroutine(timeLimitOrder.TimeCountIE());
    }
}


public class Order
{
    internal string orderName;
    internal string orderInfo;
    internal int rewardMoney;
    internal int rewardExp;
    internal bool isUpdated = false;
    internal int count = 0;
    internal int requiredNum;
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

    internal TypeNeed required;
    internal List<FishRecord> lastFishs;
    public CountOrder(CountOrderObj orderObj)
    {
        GetOrderBase(orderObj);
        requiredNum = orderObj.requirdCount;
        required = orderObj.rule;
        lastFishs = new List<FishRecord>();
    }

    internal virtual void AddCount(Fish fish)
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

    internal virtual void CheckTargetFish(Fish fish)
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


public class TimeLimitOrder : CountOrder
{
    private float limitedTime;
    private Coroutine timeCount;
    public TimeLimitOrder(TimeLimitOrderObj orderObj):base (orderObj)
    {
        GetOrderBase(orderObj);
        requiredNum = orderObj.requirdCount;
        required = orderObj.rule;
        lastFishs = new List<FishRecord>();
        limitedTime = orderObj.timeLimit;
    }

    internal override void AddCount(Fish fish)
    {
        if (isUpdated)
            return;
        CheckTargetFish(fish);
        count++;
        OrderManager.instance.TimeCount(this);
        if (count >= requiredNum)
        {
            OrderManager.instance.UpdateOrderInfo(this);
            isUpdated = true;
        }
    }

    internal IEnumerator TimeCountIE()
    {
        yield return new WaitForSeconds(limitedTime);
        count--;
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

public class ItemUseCountOrder : Order
{
    public ItemType itemType;
    public ItemUseCountOrder(ItemUseCountOrderObj orderObj)
    {
        GetOrderBase(orderObj);
        requiredNum = orderObj.requirdCount;
        itemType = orderObj.itemType;
    }

    internal void AddCount(Fish fish)
    {
        if (isUpdated)
            return;
        if (!((itemType == ItemType.Shock && GamePlay.instance.hasShockUsed) || (itemType == ItemType.Bait && GamePlay.instance.hasBaitUsed)))        
            return;
        
        count++;
        if (count >= requiredNum)
        {
            OrderManager.instance.UpdateOrderInfo(this);
            isUpdated = true;
        }
    }
}

public class ReagonOrder : Order
{

}








