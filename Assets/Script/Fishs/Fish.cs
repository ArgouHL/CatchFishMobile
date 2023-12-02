using System;
using System.Collections;
using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public abstract class Fish : MonoBehaviour
{
    [SerializeField] private Animator fishAni;
    public string fishID;
    public int hitTimes;
    [SerializeField] internal float speed;
    internal float accelerate;
    internal bool canbeClick = true;
    [SerializeField] internal bool canInteract = false;
    internal bool feared = false;
    internal float speeduptime;
    internal List<Fish> groupFish = new List<Fish>();

    public FishRarity rarity;
    public int indexInGroup;

    internal Coroutine swimming, fearing, shocking;
    internal int randomIndex;
    internal bool isPause = false;
    internal int way = 1;

    public void Swim()
    {
        PlayAni("Stunned", false);
        Swim(way);
    }

    public Fish Swim(float way)
    {
        StartCoroutine(DelayCanInteract());
        swimming = StartCoroutine(SwimToEndPoint(way));
        return this;
    }
    internal IEnumerator DelayCanInteract()
    {
        yield return new WaitForSeconds(1.5f);
        Debug.Log(gameObject.name + "canInteract");
        SetCanInteract(true);
    }
    internal void PauseMove()
    {
        isPause = true;
    }
    internal void ContinueMove()
    {
        isPause = false;
    }

    internal bool IsOutScreen()
    {
        if (transform.position.x * way > 6f)
        {
            SetCanInteract(false);
        }

        bool b = transform.position.x * way > 7.5f ? true:false;
        return b;
    }
    internal virtual IEnumerator SwimToEndPoint(float way)
    {
        while (!IsOutScreen())
        {
            transform.position += new Vector3(speed * -way * Time.deltaTime, 0, 0);
            yield return null;
        }

        Dispawn();
    }

    internal virtual void StopMove()
    {
        if (swimming != null)
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
        canInteract = false;

    }
    internal void NewFish(FishObject fishPrefab)
    {
        fishID = fishPrefab.fishID;
        hitTimes = fishPrefab.hitTimes;
        //size = fishPrefab.size;
        rarity = fishPrefab.rarity;
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
        if (!canInteract)
            return;
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

        float _speed = speed;
        speed = accelerate * way;
        yield return new WaitForSeconds(speeduptime);
        speed = _speed * way;
        fearing = null;
    }




    internal virtual void Eat()
    {
        FishControl.instance.StopDete(this);
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
        if (gameObject.activeInHierarchy)
            shocking = StartCoroutine(GetShockIE(time));
    }
    private IEnumerator GetShockIE(float time)
    {
        PlayAni("Stunned", true);
        Debug.Log("GetShot");
        float _speed = speed;
        speed = 0;
        yield return new WaitForSeconds(time);
        PlayAni("Stunned", false);
        speed = _speed;
        shocking = null;
    }

    internal void SetCanInteract(bool b)
    {
       // Debug.Log("Set CanInteract to " + b);
        canInteract = b;

    }

    public void PlayAni(string key,bool b)
    {
        if (fishAni != null)
        {
            fishAni.SetBool(key, b);
        }
    }
}

public abstract class Shark : Fish
{
    private int hp = 3;
    [SerializeField] internal SharkFear sharkFear;
    [SerializeField] private SpriteRenderer sprite;
    [SerializeField] private int waitTime;
    private Coroutine chasing;
    internal FishReagon reagon;
    internal bool canFear = true;
    Fish _eatingFish;

    internal override void Feared(Vector3 sharkPos)
    {

    }

    internal override void Eat()
    {

    }
    internal override void StopMove()
    {
        Debug.Log(gameObject.name + "Stop");
        if (chasing != null)
            StopCoroutine(chasing);
        StartCoroutine(OutScreen());
        canbeClick = false;
        canFear = false;
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

        sprite.flipY = way < 0 ? true : false;

        transform.rotation = Quaternion.FromToRotation(Vector3.right, toDirection);

        transform.position += toDirection * speed * Time.deltaTime;

        return false;
    }

    internal void Enter()
    {
        StartCoroutine(EnterIE());
    }
    internal IEnumerator EnterIE()
    {
        Vector3 targetPos = new Vector3(0, -3, 5);
        Vector3 spawnPos = RandomSide();
        transform.position = spawnPos;
        Debug.Log("SharKenter");
        while (!ToTarget(targetPos, 0.5f, 2))
        {
            yield return null;
        }
        Debug.Log("SharKentered");
        StartChaseFish();
    }

    private Vector3 RandomSide()
    {
        return new Vector3(9f, UnityEngine.Random.Range(-6f, -5f), 5);
    }

    internal void StartChaseFish()
    {
        Debug.Log("StartChaseFish");
        NextFish();
    }


    internal void NextFish()
    {
        Debug.Log("NextFish");
        var fishs = new List<Fish>(FishControl.instance.FishsOnScreen);
        var fishsOnScreen = fishs.Where(x => x.canInteract).OrderBy(x => Guid.NewGuid()).Take(1).ToArray();
        if (fishsOnScreen.Length <= 0)
        {
            LeanTween.delayedCall(1, () => NextFish());
            return;
        }

        Fish selectedFish = fishsOnScreen[0];
        chasing = StartCoroutine(ChaseTargetFish(selectedFish));
        


    }

    internal IEnumerator ChaseTargetFish(Fish _fish)
    {
        Debug.Log(gameObject.name + "ChaseTargetFish");
        Debug.Log(_fish.canInteract);

        while (_fish.canInteract && !ToTarget(_fish.transform.position, 1.6f))
        {
            yield return null;
        }
        if (_fish.canInteract)
        {
            chasing = StartCoroutine(EatFish(_fish));

        }
        else
        {
            NextFish();
        }

    }

    internal IEnumerator EatFish(Fish _fish)
    {
        _eatingFish = _fish;
        _fish.PauseMove();
        SetCanInteract(true);
        yield return new WaitForSeconds(waitTime);
        SetCanInteract(false);
        _fish.Eat();
        NextFish();
    }


    internal void beHit()
    {
        if (!canInteract)
            return;
        var s = GetComponentInChildren<SpriteRenderer>();
        LeanTween.value(0, 1, 0.5f).setOnUpdate((float val) => s.color = new Color(1, val, val, 1));
        hp--;
        SetCanInteract(false);
        if (_eatingFish != null)
            _eatingFish.ContinueMove();
        StopCoroutine(chasing);

        if (hp <= 0)
        {
            StopMove();
        }
        else
            chasing = StartCoroutine(StunIE());



    }

    internal IEnumerator StunIE()
    {
        //stunani
        SetCanInteract(false);
        yield return new WaitForSeconds(3);
        NextFish();

    }
}




public enum FishRarity { Normal, Rare, SuperRare, Shark, Chest, None }
public enum FishReagon { Pacific, Indian, Atlantic, None }