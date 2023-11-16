using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ResultRecord : MonoBehaviour
{
    public static ResultRecord instance;
    int catchedFishCount = 0;
    private List<Fish> catchedFishList;
    internal List<FishObject> allFishObj;
    internal List<Order> orderIngame;
    private Dictionary<string, FishObject> fishDict;
    private List<FishByType> _FishByTypeList;
    internal int income = 0;
    internal int exp = 0;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
        instance = this;
    }


   

    public void AddCatchedFish(Fish fish)
    {
        catchedFishList.Add(fish);
        catchedFishCount++;
        GameInformationShow.instance.UpdateCatchedCount(catchedFishCount);
        var _fishObj = fishDict[fish.fishID];
        if (!_FishByTypeList.Any(x => x.fishObj.fishID == _fishObj.fishID))
        {
            _FishByTypeList.Add(new FishByType(_fishObj));
        }
        else
        {
            _FishByTypeList.Find(x => x.fishObj.fishID == _fishObj.fishID).count++;
        }

    }

    public void Init()
    {
        _FishByTypeList = new List<FishByType>();
        catchedFishList = new List<Fish>();
        catchedFishCount = 0;
        income = 0;
        exp = 0;
    }

    public void GetAllFishObj(FishObject[] input)
    {
        allFishObj = new List<FishObject>(input.ToList<FishObject>());
        NewDict();
    }

    public void GetOrder(List<Order> orders)
    {
        orderIngame = new List<Order>(orders);
    }

    private void NewDict()
    {
        fishDict = new Dictionary<string, FishObject>();
      
        foreach (var f in allFishObj)
        {
            fishDict.Add(f.fishID, f);
        }
    }

    public void CalcResult()
    {

        CalcIncomeAndExp();

    }

    private void CalcIncomeAndExp()
    {
        foreach (var cf in catchedFishList)
        {
            income += fishDict[cf.fishID].income;

        }
        foreach (var fo in orderIngame)
        {
            if (fo.isFinished)
            {
                income += fo.rewardMoney;
                exp += fo.rewardExp;
            }
        }
    }

    public List<FishByType> FishByTypeCount()
    {
        return _FishByTypeList.OrderByDescending(x => x.fishObj.rarity).ToList();
    }

}

public class FishByType
{
    public FishObject fishObj;
    public int count;

    public FishByType(FishObject _fishObj)
    {
        fishObj = _fishObj;
        count = 1;
    }


}