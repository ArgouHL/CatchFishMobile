using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using TMPro;
using Random = UnityEngine.Random;
using UnityEngine.InputSystem;

public class FishControl : MonoBehaviour
{
    private static System.Random random;
    public static FishControl instance;
    int remainHitTime = 0;
    private Fish hittedFish = null;
    [SerializeField] private TMP_Text t;
    [SerializeField] private List<Fish> fishsOnScreen;
    [SerializeField] private FishObject[] tempFishs;
    [SerializeField] Transform fishpool;
    private int lastRanIndex;
    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        Instantized();

    }



    public void StartGenFish()
    {
        StartCoroutine(FishSpawn());
    }
    private void Instantized()
    {
        fishsOnScreen = new List<Fish>();

    }

    public void GenerateFish()
    {
        var fishPrefab = tempFishs[Random.Range(0, tempFishs.Length)];
        int way = Random.Range(0f, 1f) > 0.5f ? -1 : 1;
        float height = Random.Range(-9f, 4f);
        random = new System.Random(CodeHelper.GetGuidSeed());
        var _index = random.Next(0, fishPrefab.randomRoadCount);
        do
        { _index = random.Next(0, fishPrefab.randomRoadCount);
        } while (_index == lastRanIndex&& fishPrefab.randomRoadCount > 1);

        lastRanIndex = _index;
        for (int i = 0; i < fishPrefab.numberOfGroup; i++)
        {
            var fishObj = Instantiate(fishPrefab.fishObj, new Vector3(8 * way, fishPrefab.startHeight, 5), Quaternion.identity);
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
            remainHitTime = _fish.hitTimes;
        }
        remainHitTime--;
        SfxControl.instance.HitPlay(remainHitTime);
        if (remainHitTime <= 0)
        {
            ResultCount.instance.AddCatchedFish();
            GamePlay.CatchedFish.Invoke(_fish);
            _fish.StopMove();
            TargetMarkCtr.instance.StopTracking();

        }
        UpdateHit();
    }

    private Fish FindClosestFish(Vector2 touchPos)
    {
        Fish hitFish = null;
        Ray ray = Camera.main.ScreenPointToRay(touchPos);
        RaycastHit2D hit2D = Physics2D.GetRayIntersection(ray);
        if (hit2D.collider == null)
            return null;
        hitFish = hit2D.collider.GetComponentInParent<Fish>();

        return hitFish;
    }



    private void UpdateHit()
    {
        t.text = remainHitTime.ToString();
        t.gameObject.SetActive(remainHitTime > 0);


    }


    private IEnumerator FishSpawn()
    {
        while (!GamePlay.isGameEnd)
        {
            GenerateFish();
            yield return new WaitForSeconds(1f);
        }
    }

    public void TestFuncINint(int i)
    {

    }


}
