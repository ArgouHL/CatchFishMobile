using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FishData : MonoBehaviour
{
    public static FishData instance;

    private Dictionary<string, FishObject> fishObjectDict;
    [SerializeField] private FishObject[] allFishObjects;


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


    internal object GetRarity(string fishID)
    {
        return fishObjectDict[fishID].rarity;
    }

    internal Sprite GetFishIcon(string fishID)
    {
        return fishObjectDict[fishID].fishIcon;
    }
}
