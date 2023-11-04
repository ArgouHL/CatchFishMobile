using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fish : MonoBehaviour
{

    public string fishID;
    public int hitTimes;

    internal float size;
    public string fishName;

    public FishRarity rarity;
    public FishReagon reagon;
    public int income;
    public int exp;


    Coroutine swimming;
    public Fish Swim(float way, float speed)
    {
        swimming = StartCoroutine(SwimToEndPoint(way, speed));
        return this;
    }

    private IEnumerator SwimToEndPoint(float way, float speed)
    {
        while (transform.position.x * way > -8f)
        {
            transform.position += new Vector3(speed * -way * Time.deltaTime, 0, 0);
            yield return null;
        }
        FishControl.instance.FishOutScreen(this);
        Destroy(gameObject);
    }

    public void StopMove()
    {
        StopCoroutine(swimming);
        FishControl.instance.FishOutScreen(this);
        Destroy(gameObject);
    }

    internal void NewFish(FishObject fishPrefab)
    {
        fishID = fishPrefab.fishID;
        hitTimes = fishPrefab.hitTimes;
        size = fishPrefab.size;
        fishName = fishPrefab.name;
        rarity = fishPrefab.rarity;
        reagon = fishPrefab.reagon;
        income = fishPrefab.income;
        exp = fishPrefab.exp;
    }
}

public enum FishRarity { Normal, Rare, SuperRare, Shark, Chest ,None}
public enum FishReagon { Pacific,Indian, Atlantic ,None}