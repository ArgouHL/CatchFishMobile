using System;
using System.Collections;
using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public abstract class Fish : MonoBehaviour
{

    public string fishID;
    public int hitTimes;

    public string fishName;
    internal Vector2 realSize;
    [SerializeField] internal float speed;
    internal float accelerate;
    internal bool canbeEat = true;
    internal bool canbeClick = true;
    internal bool feared = false;
    internal float speeduptime;
    internal List<Fish> groupFish = new List<Fish>();

    public FishRarity rarity;
    public int income;
    public int exp;
    public int indexInGroup;

    internal Coroutine swimming, fearing, shocking;
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
       
        Dispawn();
    }

    internal virtual void StopMove()
    {
        StopCoroutine(swimming);
        canbeClick = false;
        Dispawn();

    }

    internal void Dispawn()
    {
        swimming = null;
        if (fearing != null)
            StopCoroutine(fearing);
        if (shocking != null)
            StopCoroutine(shocking);
        FishControl.instance.FishOutScreen(this);
        gameObject.SetActive(false);
        canbeEat = false;
      
    }
    internal void NewFish(FishObject fishPrefab)
    {
        fishID = fishPrefab.fishID;
        hitTimes = fishPrefab.hitTimes;
        //size = fishPrefab.size;
        fishName = fishPrefab.name;
        rarity = fishPrefab.rarity;
        income = fishPrefab.income;
        exp = fishPrefab.exp;
        speed = fishPrefab.speed;
        accelerate = fishPrefab.acceleratedSpeed;
        speeduptime = fishPrefab.speedUpTime;
    }


    static public Vector3 ChangeWay(Vector3 direction, float rotatangle)
    {
        Quaternion rotation = Quaternion.Euler(0, 0, rotatangle);
        Vector3 rotatedDirection = rotation * direction;
        rotatedDirection.Normalize();
        return rotatedDirection;
    }

    internal virtual void Feared(Vector3 sharkPosition)
    {
        if (feared)
            return;
        if (!gameObject.activeInHierarchy)
            return;
        Debug.Log("Feared" + gameObject.name);
        feared = true;
        foreach (var f in groupFish)
        {
            f.Feared(sharkPosition);
        }
        //    int way = sharkPosition.x > transform.position.x ? 1 : -1;
        // GetComponentInChildren<SpriteRenderer>().flipX = way > 0 ? false : true;
        int way = speed > 0 ? 1 : -1;
        fearing = StartCoroutine(GetFearIE(speeduptime, way));
        
    }

    internal IEnumerator GetFearIE(float speeduptime, int way)
    {
        Debug.Log("GetFear");
        float _speed = speed;
        speed = accelerate * way;
        yield return new WaitForSeconds(speeduptime);
        speed = _speed * way;
        fearing = null;
    }




    internal virtual void Eat()
    {
        Debug.Log("eated" + gameObject.name);
        if (swimming != null)
            StopCoroutine(swimming);
        if (fearing != null)
            StopCoroutine(fearing);
        if (shocking != null)
            StopCoroutine(shocking);
        Dispawn();
    }

    internal void GetShock(float time)
    {
        shocking = StartCoroutine(GetShockIE(time));
    }
    private IEnumerator GetShockIE(float time)
    {
        Debug.Log("GetShot");
        float _speed = speed;
        speed = 0;
        yield return new WaitForSeconds(time);
        speed = _speed;
        shocking = null;
    }
}

public abstract class Shark : Fish
{
    [SerializeField] internal SharkFear sharkFear;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private SpriteRenderer sprite;
    private Coroutine chasing;
    internal FishReagon reagon;
    internal override void Feared(Vector3 sharkPos)
    {

    }

    internal override void Eat()
    {

    }
    internal override void StopMove()
    {
        StopCoroutine(chasing);
        StartCoroutine(OutScreen());
        canbeClick = false;
    }

    internal abstract IEnumerator OutScreen();

    internal bool ToTarget(Vector3 targetPos, float dis)
    {
        return ToTarget(targetPos, dis, speed);
    }

    internal bool ToTarget(Vector3 targetPos, float dis, float speed)
    {

        var _pos = transform.position;
        if (Vector3.Distance(targetPos, _pos) < dis)
        { return true; }
        Vector3 fromDirection = transform.up;
        Vector3 toDirection = (targetPos - transform.position).normalized;
        int way = toDirection.x > 0 ? 1 : -1;
        // toDirection.x *= way;
        sprite.flipY = way < 0 ? true : false;
        //var euler = transform.rotation.eulerAngles;
        //if (way > 0) euler.y = 0;
        //else euler.y = 0;
        //transform.rotation = Quaternion.Euler(euler);
        //float angle = Vector2.Angle(new Vector2(fromDirection.x, fromDirection.y), new Vector2(toDirection.x, toDirection.y));

        //// Calculate the step angle based on the desired rotation speed
        //float step = rotationSpeed * Time.deltaTime;


        //float newZRotation = Mathf.Atan2(toDirection.y, toDirection.x) * Mathf.Rad2Deg;

        // Interpolate the rotation with Slerp
        //Quaternion newRotation = Quaternion.Euler(0, 0, newZRotation);
        //newRotation = Quaternion.Slerp(transform.rotation, newRotation, step / angle);
        //transform.rotation = newRotation;

        //var forward = (transform.rotation * (Vector3.right)).normalized;
        transform.rotation = Quaternion.FromToRotation(Vector3.right, toDirection);
        //sprite.flipX = way < 0 ? true : false;
        transform.position += toDirection * speed * Time.deltaTime;

        return false;
    }

    internal void Enter()
    {
        StartCoroutine(EnterIE());
    }
    internal abstract IEnumerator EnterIE();

    internal void ChaseFish()
    {
        chasing = StartCoroutine(ChaseFishIE());
    }
    internal abstract IEnumerator ChaseFishIE();
}
public enum FishRarity { Normal, Rare, SuperRare, Shark, Chest, None }
public enum FishReagon { Pacific, Indian, Atlantic, None }