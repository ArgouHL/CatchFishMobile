using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[DefaultExecutionOrder(-2)]
public class SkinController : MonoBehaviour
{
    public static SkinController instance;
    [SerializeField] private CharaterSkinSet[] sets;
    private Dictionary <string, CharaterSkinSet> skinDict;



    private void Awake()
    {
        if (instance != null)
            Destroy(gameObject);
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
    private void Start()
    {
        Init();
    }

    private void Init()
    {
        NewSkinDict();
    }

    private void NewSkinDict()
    {
        skinDict = sets.ToDictionary(skin => skin.SkinID);
    }


    internal CharaterSkinSet GetSkin(string id)
    {
        if(skinDict.ContainsKey(id))
        {
            return skinDict[id];
        }
        else
            return skinDict["01"];
    }


}
