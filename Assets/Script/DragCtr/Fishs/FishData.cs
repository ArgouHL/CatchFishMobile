using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
[DefaultExecutionOrder(-2)]
public class FishData : MonoBehaviour
{
    public static FishData instance;

    private Dictionary<string, FishObject> fishObjectDict;
    public FishObject[] allFishObjects;


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

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        fishObjectDict = allFishObjects.ToDictionary(fishObject => fishObject.fishID);

    }


    internal FishRarity GetRarity(string fishID)
    {
        return fishObjectDict[fishID].rarity;
    }

    internal FishReagon GetRegion(string fishID)
    {
        return fishObjectDict[fishID].reagon;
    }

    internal Sprite GetFishIcon(string fishID)
    {
        return fishObjectDict[fishID].fishIcon;
    }

    internal bool GetFishDeteData(string fishID,out float deteTime,out float deteSpeed)
    {
        deteTime = 2;
        deteSpeed = 0.5f;
        if (!fishObjectDict.ContainsKey(fishID))
            return false;
        deteTime= fishObjectDict[fishID].deterTime;
        deteSpeed= fishObjectDict[fishID].deterSpeed;
        return true;
    }

    public void AddRecord(string fishID,int count)
    {
        var dict = PlayerDataControl.instance.playerData.fishCollection;
        if(dict.ContainsKey(fishID))
        {
            dict[fishID].count+= count;
        }
        else
        {
            dict.Add(fishID, new FishColletRecord(fishID,count));
        }
    }

    public List<FishColletRecord> GetRecord()
    {
        List<FishColletRecord> records = new List<FishColletRecord>();
        var dict = PlayerDataControl.instance.playerData.fishCollection;
        
        foreach (var f in allFishObjects)
        {
            if (dict.ContainsKey(f.fishID))
            {
                records.Add(dict[f.fishID]);
            }
            else
            {
                records.Add(new FishColletRecord(f.fishID, 0));
            }
        }
        return records;
    }

    public FishObject GetFishObject(string id)
    {
        return fishObjectDict[id];
    }
}

[Serializable]
public class FishColletRecord
{
    public string fishID;
    public int count;
    public bool hasGetReward;
    public FishColletRecord(string iD,int _count)
    {
        fishID = iD;
        count = _count;
        hasGetReward = false;
    }

    public void GetReward()
    {
        hasGetReward = true;
    }


}
