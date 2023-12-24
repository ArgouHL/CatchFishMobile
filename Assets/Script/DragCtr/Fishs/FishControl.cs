using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using TMPro;

using UnityEngine.InputSystem;
using System.Linq;

public class FishControl : MonoBehaviour
{
    private static System.Random random;
    public static FishControl instance;

    private Fish hittedFish = null;
    public static FishReagon reagon;

    [SerializeField] private TMP_Text t;
    [SerializeField] private LinkedList<Fish> fishsOnScreen;
    [SerializeField] internal LinkedList<Fish> FishsOnScreen { get { return fishsOnScreen; } }
   

    [SerializeField] private int normalNum, rareNum, superNum;

    [SerializeField] private List<FishObject> allFishToSpawn;


    [SerializeField] private Dictionary<FishReagon, GameObject> sharkDictionary;
    [SerializeField] Transform fishpool;
    private int lastRanIndex;
    private Fish nowFish;

    [SerializeField] private float fishSpawnPeriod = 2;
   // private List<List<Fish>> SpawnedFish;

    private void Awake()
    {
        instance = this;
    }

    private void OnEnable()
    {
        CatchDeter.EndDete += GetFishDete;
        GameInformationShow.StopCoro += StopCoro;
        
    }
    private void OnDisable()
    {
        CatchDeter.EndDete -= GetFishDete;
        GameInformationShow.StopCoro -= StopCoro;
        
    }
    private void Start()
    {
        Debug.Log(TempData.instance.targetReagon);
        reagon = TempData.instance.targetReagon;
        Instantized();
        GenerateSpawnedFish();
    }



    private void GetFishDete(bool success)
    {
        PlayerInputManager.instance.ChangeType(InputType.GamePlay);
        TargetMarkCtr.instance.StopTracking();
        if (success)
        {
            ResultRecord.instance.AddCatchedFish(nowFish);
            CatCtr.instance.Atk();
            SfxControl.instance.CatchPlay();
            nowFish.StopMove();
            GamePlay.CatchedFish.Invoke(nowFish);
            GamePlay.instance.PlayCatchEff(nowFish.transform.position);
        }
        else
        {
            nowFish.ContinueMove();
        }
    }

    private void GenerateSpawnedFish()
    {
        allFishToSpawn = GetGenFishList();
        //for (int i = 0; i < allFishToSpawn.Count; i++)
        //{
        //    SpawnedFish.Add(GenerateFish(i));
        //}
    }

    public void StartGenFish()
    {

        StartCoroutine(FishSpawn());
    }
    private void Instantized()
    {
      //  SpawnedFish = new List<List<Fish>>();
        ResultRecord.instance.GetAllFishObj(FishData.instance.allFishObjects);
        fishsOnScreen = new LinkedList<Fish>();



    }

    public List<Fish> GenerateFish(int index)
    {

        var fishPrefab = allFishToSpawn[index];
        int way = UnityEngine.Random.Range(0f, 1f) > 0.5f ? -1 : 1;
        float height = UnityEngine.Random.Range(-9f, 4f);
        random = new System.Random(CodeHelper.GetGuidSeed());
        var _index = random.Next(0, fishPrefab.randomRoadCount);
        do
        {
            _index = random.Next(0, fishPrefab.randomRoadCount);
        } while (_index == lastRanIndex && fishPrefab.randomRoadCount > 1);

        lastRanIndex = _index;
        float _startPos = fishPrefab.startHeights[UnityEngine.Random.Range(0, fishPrefab.startHeights.Length)];
        List<Fish> fishs = new List<Fish>();
        for (int i = 0; i < fishPrefab.numberOfGroup; i++)
        {

            var fishObj = Instantiate(fishPrefab.fishObj, new Vector3(8.5f * -way, _startPos, 5), Quaternion.identity);
            fishObj.transform.localScale = new Vector3(way, 1, 1);
            fishObj.transform.parent = fishpool;
            var _fish = fishObj.GetComponent<Fish>();
            fishs.Add(_fish);

            _fish.randomIndex = lastRanIndex;
            _fish.NewFish(fishPrefab);
            _fish.way = way;
            // _fish.Swim(way);
            //_fish.size = fishPrefab.size;
         
           
            _fish.indexInGroup = i;
           
        }

        if (fishPrefab.numberOfGroup > 1)
        {
            foreach (var f in fishs)
            {
                f.groupFish = fishs;
            }
        }
        return fishs;
    }

    internal void StopDete(Fish fish)
    {
        if (fish != nowFish)
            return;
        TargetMarkCtr.instance.StopTracking();
        CatchDeter.instance.CancelDete();
    }

    private void GetFishType(FishObject fishPrefab, out bool success, out Type type)
    {
        Type fishType = Type.GetType("Fish" + fishPrefab.fishID);
        if (fishType == null)
        {
            success = false;
            type = null;
            Debug.Log("no fish Type");
        }
        else
        {
            success = true;
            type = fishType;
            Debug.Log("Get Fish Type");
        }
    }

    public void FishOutScreen(Fish fish)
    {
       fishsOnScreen.Remove(fish);
        if (hittedFish != fish)
            return;
        hittedFish = null;
     
     
      
    }
    public void HitFish(Fish _fish)
    {
       // var _fish = TouchFunc.FindClosestFish(aimPoint);
        if (_fish == null)
            return;
        if (hittedFish != _fish)
        {
            hittedFish = _fish;
           
        }
        if (!_fish.canInteract)
            return;

        if (_fish is Shark)
        {
            var _shark = _fish as Shark;
            _shark.beHit();
            Debug.Log("isShark");
            CatCtr.instance.Atk();
        }
        else 
        {
            SfxControl.instance.HitPlay(0);
            nowFish = _fish;
            CatchDeter.instance.StartDete(_fish.fishID);
            nowFish.PauseMove();
            TargetMarkCtr.instance.StartTracking(nowFish.transform);
        }
        
        // UpdateHit();
    }





    private IEnumerator FishSpawn()
    {
        Debug.Log("FishSpawn");
        for (int i = 0; i < allFishToSpawn.Count; i++)
        {
            if (GamePlay.isGameEnd)
                break;
            var fs = GenerateFish(i);
            foreach (var f in fs)
            {
               f.Swim();
                fishsOnScreen.AddLast(f);
            }
            yield return new WaitForSeconds(fishSpawnPeriod);
        }


    }

    internal void ShockAllFish(float time)
    {
        foreach (var fish in fishsOnScreen)
        {
            fish.GetShock(time);
        }
    }




    private List<FishObject> GetGenFishList()
    {
        var normalFishs = GetFishWithRarity(FishRarity.Normal);
        var rareFishs = GetFishWithRarity(FishRarity.Rare);
        var superFishs = GetFishWithRarity(FishRarity.SuperRare);
        List<FishObject> fishsList = new List<FishObject>();

        AddFishs(fishsList, normalFishs, normalNum);
        AddFishs(fishsList, rareFishs, rareNum);
        AddFishs(fishsList, superFishs, superNum);

        return fishsList.OrderBy(x => Guid.NewGuid()).ToList();
    }

    private void AddFishs(List<FishObject> fishsList, HashSet<FishObject> fishs, int num)
    {
        if (num <= 0)
            return;

        Dictionary<FishObject, int> weightDict = new Dictionary<FishObject, int>();



        foreach (var fishObj in fishs)
        {
            if (FishUnlockCtr.instance.CheckIfUnlocked(fishObj.fishID) && fishObj.reagon==reagon)
            {
                // Debug.Log(fishObj.name);
                weightDict.Add(fishObj, fishObj.weights);
            }
        }

        int totalWeight = 0;
        foreach (var weight in weightDict.Values)
        {
            totalWeight += weight;
        }
        //  Debug.Log("totalWeight" + totalWeight);

        for (int i = 0; i < num; i++)
        {
            var rand1 = new System.Random();
            int ranNum = rand1.Next(0, totalWeight + 1);
            //  Debug.Log("Ran" + ranNum);
            int counter = 0;
            foreach (var temp in weightDict)
            {
                counter += temp.Value;
                //     Debug.Log("counter" + counter);
                if (ranNum <= counter)
                {
                    // Debug.Log(ranNum + "," + temp.Key);
                    fishsList.Add(temp.Key);
                    break;
                }
            }
            if (ranNum > counter)
                Debug.LogError("no" + ranNum);
        }



    }

    private HashSet<FishObject> GetFishWithRarity(FishRarity rarity)
    {
        return FishData.instance.allFishObjects.Where(p => (p.reagon==reagon && p.rarity.Equals(rarity))).ToHashSet();
    }


    public void StopCoro()
    {
        StopAllCoroutines();
    }
}
