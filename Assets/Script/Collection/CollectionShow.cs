using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Linq;
using System;
using UnityEngine.SceneManagement;

public class CollectionShow : MonoBehaviour
{
    public static CollectionShow instance;
    private FishColletRecord[] recordList1, recordList2, recordList3;
    private List<CollectionFish> fishes1, fishes2, fishes3;
    [SerializeField] private Sprite normalBoarder, rareBoarder, superBoarder;
    [SerializeField] private Sprite unknow;
    [SerializeField] private Transform page1, page2, page3;
    [SerializeField] private RectTransform collectionFish;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        PlayerInputManager.instance.ChangeType(InputType.Shop);
        var recordList = FishData.instance.GetRecord().OrderBy(x => x.fishID).ToArray();
        recordList1 = recordList.Where(x => FishData.instance.GetRegion(x.fishID) == FishReagon.Pacific).OrderBy(x => x.fishID).ToArray();
        recordList2 = recordList.Where(x => FishData.instance.GetRegion(x.fishID) == FishReagon.Indian).OrderBy(x => x.fishID).ToArray();
        recordList3 = recordList.Where(x => FishData.instance.GetRegion(x.fishID) == FishReagon.Atlantic).OrderBy(x => x.fishID).ToArray();

        SpawnCollectionFish(recordList1.Length, page1, ref fishes1);
        SpawnCollectionFish(recordList2.Length, page2, ref fishes2);
        SpawnCollectionFish(recordList3.Length, page3, ref fishes3);
        for (int i = 0; i < recordList1.Length; i++)
        {
            fishes1[i].Show(recordList1[i]);
        }
        for (int i = 0; i < recordList2.Length; i++)
        {
            fishes2[i].Show(recordList2[i]);
        }
        for (int i = 0; i < recordList3.Length; i++)
        {
            fishes3[i].Show(recordList3[i]);
        }
    }

    private void SpawnCollectionFish(int length, Transform page1, ref List<CollectionFish> fishes)
    {
        fishes = new List<CollectionFish>();
        int index = 0;
        Vector2 orgPoint = Vector2.zero;
       
        orgPoint.y = 705;
        while (index < length)
        {
            orgPoint.x = -400;
            for (int j = 0; j < 3; j++)
            {
                var f = Instantiate(collectionFish, page1, false);

                var cf = f.GetComponent<CollectionFish>();
                fishes.Add(cf);
                //    f.SetParent(FishType);  
                f.anchoredPosition = orgPoint;
                index++;

                if (index == length)
                    break;
                orgPoint.x += 400;
               
            }
            orgPoint.y -= 500;
        }

    }

    internal void GetReward()
    {
        PlayerDataControl.instance.playerData.AddCurrency(currencyType.Coin, 300);
        SfxControl.instance.CoinPlay();
        PlayerDataControl.instance.Save();
    }

    public Sprite GetBoarder(FishRarity rarity)
    {
        Sprite boarder = normalBoarder;
        switch (rarity)
        {
            case FishRarity.Normal:
                boarder = normalBoarder;
                break;
            case FishRarity.Rare:
                boarder = rareBoarder;
                break;
            case FishRarity.SuperRare:
                boarder = superBoarder;
                break;
        }
        return boarder;
    }

    public Sprite GetUnknow()
    {
        return unknow;
    }



    public void LoadAqua()
    {
        SceneManager.LoadScene(4);
    }

}
