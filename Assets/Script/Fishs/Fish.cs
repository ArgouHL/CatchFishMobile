using System;
using System.Collections;
using UnityEngine;

public abstract class Fish : MonoBehaviour
{

    public string fishID;
    public int hitTimes;

    public string fishName;
    internal Vector2 realSize;
    internal float speed;
    internal float accelerate;




    public FishRarity rarity;
    public FishReagon reagon;
    public int income;
    public int exp;
    public int indexInGroup;

    Coroutine swimming;
    internal int randomIndex;

    public Fish Swim(float way)
    {
        swimming = StartCoroutine(SwimToEndPoint(way));
        return this;
    }

    internal virtual IEnumerator SwimToEndPoint(float way)
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
        //size = fishPrefab.size;
        fishName = fishPrefab.name;
        rarity = fishPrefab.rarity;
        reagon = fishPrefab.reagon;
        income = fishPrefab.income;
        exp = fishPrefab.exp;
        speed = fishPrefab.speed;
        accelerate = fishPrefab.acceleratedSpeed;

    }


    static public Vector3 ChangeWay(Vector3 direction, float rotatangle)
    {
        Quaternion rotation = Quaternion.Euler(0, 0, rotatangle);
        Vector3 rotatedDirection = rotation * direction;
        rotatedDirection.Normalize();
        return rotatedDirection;
    }

    internal abstract void Feared();

    internal virtual void Eat()
    {
        StopCoroutine(swimming);
        FishControl.instance.FishOutScreen(this);
    }
}

public enum FishRarity { Normal, Rare, SuperRare, Shark, Chest, None }
public enum FishReagon { Pacific, Indian, Atlantic, None }