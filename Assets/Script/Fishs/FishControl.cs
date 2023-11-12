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
    int remainHitTime = 0;
    private Fish hittedFish = null;
    public static FishReagon reagon;


    [SerializeField] private TMP_Text t;
    [SerializeField] private List<Fish> fishsOnScreen;
    [SerializeField] internal List<Fish> FishsOnScreen { get { return fishsOnScreen; } }
    [SerializeField] private FishObject[] allFish;

    [SerializeField] private int normalNum, rareNum, superNum;

    private List<FishObject> allFishToSpawn;

    [SerializeField] private GameObject[] sharks;
    [SerializeField] private Shark currentShark;
    [SerializeField] private Dictionary<FishReagon, GameObject> sharkDictionary;
    [SerializeField] Transform fishpool;
    private int lastRanIndex;
    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        Debug.Log(TempData.targetReagon);
        reagon = TempData.targetReagon;
        Instantized();
        GenerateSpawnFish();
    }

    private void GenerateSpawnFish()
    {
        allFishToSpawn = GetGenFishList();

    }

    public void StartGenFish()
    {
        StartCoroutine(FishSpawn());
    }
    private void Instantized()
    {
        fishsOnScreen = new List<Fish>();
        sharkDictionary = new Dictionary<FishReagon, GameObject>();
        foreach (var s in sharks)
        {
            sharkDictionary.Add(s.GetComponent<Shark>().reagon, s);
        }
    }

    public void GenerateFish(int index)
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
        for (int i = 0; i < fishPrefab.numberOfGroup; i++)
        {

            var fishObj = Instantiate(fishPrefab.fishObj, new Vector3(8 * way, _startPos, 5), Quaternion.identity);
            fishObj.transform.localScale = new Vector3(way, 1, 1);
            fishObj.transform.parent = fishpool;
            var _fish = fishObj.GetComponent<Fish>();
            _fish.indexInGroup = i;
            _fish.randomIndex = lastRanIndex;
            _fish.NewFish(fishPrefab);
            _fish.Swim(way);
            //_fish.size = fishPrefab.size;
            _fish.hitTimes = fishPrefab.hitTimes;
            fishsOnScreen.Add(_fish);
        }


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
        TargetMarkCtr.instance.StopTracking();
        remainHitTime = 0;
        UpdateHit();
    }
    public void HitFish(Vector2 touchPos)
    {
        var _fish = FindClosestFish(touchPos);
        if (_fish == null)
            return;
        if (hittedFish != _fish)
        {
            hittedFish = _fish;
            TargetMarkCtr.instance.StartTracking(_fish);
            remainHitTime = _fish.hitTimes > 0 ? _fish.hitTimes : 1;
        }
        remainHitTime--;
        SfxControl.instance.HitPlay(remainHitTime);
        if (remainHitTime <= 0)
        {
            if (_fish is Shark)
            {

                _fish.StopMove();
            }
            else
            {
                ResultCount.instance.AddCatchedFish();
                _fish.StopMove();
            }

            GamePlay.CatchedFish.Invoke(_fish);

            TargetMarkCtr.instance.StopTracking();

        }
        UpdateHit();
    }

    private Fish FindClosestFish(Vector2 touchPos)
    {
        Fish hitFish = null;
        Ray ray = Camera.main.ScreenPointToRay(touchPos);
        RaycastHit2D[] hit2D = Physics2D.GetRayIntersectionAll(ray);
        if (hit2D.Length == 0)
            return null;
        float mindist = 9999999;

        foreach (var hit in hit2D)
        {
            if (hit.collider.CompareTag("Fish"))
            {
                var _dis = Vector3.Distance(touchPos, hit.collider.transform.position);
                if (_dis < mindist)
                {
                    var f = hit.collider.GetComponentInParent<Fish>();
                    if (f.canbeClick)
                    {
                        mindist = _dis;
                        hitFish = f;
                    }

                }
            }
        }

        return hitFish;
    }



    private void UpdateHit()
    {
        t.text = remainHitTime.ToString();
        t.gameObject.SetActive(remainHitTime > 0);


    }


    private IEnumerator FishSpawn()
    {

        for (int i = 0; i < allFishToSpawn.Count; i++)
        {
            if (GamePlay.isGameEnd)
                break;
            GenerateFish(i);
            yield return new WaitForSeconds(1f);
        }

        //while (!GamePlay.isGameEnd)
        //{


        //}
    }

    internal void ShockAllFish(float time)
    {
        foreach (var fish in fishsOnScreen)
        {
            fish.GetShock(time);
        }
    }

    public void SpawnShark()
    {

        currentShark = Instantiate(sharkDictionary[reagon], new Vector3(100, 0, 0), quaternion.identity).GetComponent<Shark>();
        currentShark.Enter();
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
 
        return fishsList.OrderBy(x=>Guid.NewGuid()).ToList();
    }

    private void AddFishs(List<FishObject> fishsList, HashSet<FishObject> fishs, int num)
    {
        if (num <= 0)
            return;

        Dictionary<FishObject, int> weightDict = new Dictionary<FishObject, int>();



        foreach (var fishObj in fishs)
        {
            if (FishUnlockCtr.instance.CheckIfUnlocked(fishObj.fishID) && fishObj.reagon.Contains(reagon))
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
        return allFish.Where(p => (p.reagon.Contains(reagon) && p.rarity.Equals(rarity))).ToHashSet();
    }
}
