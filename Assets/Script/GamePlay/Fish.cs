using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fish : MonoBehaviour
{
    public fishType type;
    public int HitTimes;
    Coroutine swimming;
    internal float size;

    public Fish Swim(float way,float speed)
    {
        swimming=StartCoroutine(SwimToEndPoint(way, speed));
        return this;
    }

    private IEnumerator SwimToEndPoint(float way, float speed)
    {
        while (transform.position.x * way > -8f)
        {
            transform.position += new Vector3(speed*-way * Time.deltaTime, 0, 0);
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
}
