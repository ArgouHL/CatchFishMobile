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
    public static FishControl instance;
    int remainHitTime = 0;
    private Fish hittedFish = null;
    [SerializeField] private TMP_Text t;
    [SerializeField] private List<Fish> fishsOnScreen;
    [SerializeField] private FishObject[] tempFishs;

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
        var fishObj = Instantiate(fishPrefab.fishObj, new Vector2(8*way, height), Quaternion.identity);
        var _fish = fishObj.AddComponent<Fish>().Swim(way,3f);
        
        _fish.size = fishPrefab.size;
        _fish.HitTimes = fishPrefab.hitTimes;
        fishsOnScreen.Add(_fish);
    }
    public void FishOutScreen(Fish fish)
    {
        fishsOnScreen.Remove(fish);
    }
    public void HitFish(Vector2 touchPos)
    {
        var _fish = FindClosestFish(touchPos);
        if (_fish == null)
            return;
        if (hittedFish != _fish)
        {
            hittedFish = _fish;
            remainHitTime = _fish.HitTimes;
        }
        remainHitTime--;
        if (remainHitTime <= 0)
        {
            ResultCount.instance.AddCatchedFish();
            GamePlay.CatchedFish.Invoke(_fish);
            _fish.StopMove();         

        }
        UpdateHit();
    }

    private Fish FindClosestFish(Vector2 touchPos)
    {
        Fish nearestFish = null;
        float closestDistance = 999;
        foreach (var f in fishsOnScreen)
        {
            var dis = Vector2.Distance(f.transform.position, (Vector3)touchPos);
            if (dis <= f.size)
                if (dis < closestDistance)
                {
                    nearestFish = f;
                    closestDistance = dis;
                }
        }
        return nearestFish;
    }

    //private Vector3 RandomPos()
    //{
    //    Vector3 _pos = new Vector3(Random.Range(-3f, 3f), Random.Range(-3f, 3f), 0);
    //    return _pos;
    //}

    private void UpdateHit()
    {
        t.text = remainHitTime.ToString();
        t.gameObject.SetActive(remainHitTime > 0);


    }


    private IEnumerator FishSpawn()
    {
        while(!GamePlay.isGameEnd)
        {
            GenerateFish();
            yield return new WaitForSeconds(1.5f);
        }
    }

    public void TestFuncINint(int i)
    {

    }

    
}
