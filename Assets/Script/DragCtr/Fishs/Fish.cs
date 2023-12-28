using System;
using System.Collections;
using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public abstract class Fish : MonoBehaviour
{
    [SerializeField] private Animator fishAni;
    public string fishID;

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
    internal bool isShocking = false;
    internal int way = 1;
    Coroutine escapingCoro;
    Vector3 escapeVector;

    public void Swim()
    {

        PlayAni("shocked", false);
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
    internal void GetOut()
    {
        //  isPause = false;
        canbeClick = false;
        Escape();
    }

    internal bool IsOutScreen()
    {
        if (transform.position.x * way > 6f)
        {
            SetCanInteract(false);
        }

        bool b = transform.position.x * way > 8f ? true : false;
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
        isShocking = true;
        PlayAni("shocked", true);
        Debug.Log("GetShot");
        float _speed = speed;
        speed = 0;
        yield return new WaitForSeconds(time);
        PlayAni("shocked", false);
        speed = _speed;
        shocking = null;
        isShocking = false;
    }

    internal void SetCanInteract(bool b)
    {
        // Debug.Log("Set CanInteract to " + b);
        canInteract = b;

    }

    public void PlayAni(string key, bool b)
    {
        if (fishAni != null)
        {
            fishAni.SetBool(key, b);
        }
    }

    internal void TryEscape(Transform catCentre)
    {
        if (swimming != null)
            StopCoroutine(swimming);
        if (escapingCoro != null)
            return;

        fishAni.SetBool("run", true);
        escapeVector =transform.position - catCentre.position;
        escapeVector.z = 0;
        escapeVector = escapeVector.normalized;
        int way = escapeVector.x > 0 ? 1 : -1;
        transform.localScale = Vector3.one;
        var sp = GetComponentInChildren<SpriteRenderer>();
        sp.flipX = false;
        sp.flipY = way < 0 ? true : false;
        escapingCoro = StartCoroutine(escapingIE(escapeVector));
        transform.rotation = Quaternion.FromToRotation(Vector3.right, escapeVector);

    }


    private IEnumerator escapingIE(Vector3 escapeVector)
    {
        Vector3 pos = transform.position;
        float time = 0;
        while (true)
        {
            transform.position = pos + RunCurve(time) * (escapeVector) * 3;
            FishControl.instance.UpdateLineRender();
            time += Time.deltaTime;
            yield return null;
        }

    }

    public void Escape()
    {
        Debug.Log("Escape");
        if (escapingCoro != null)
            StopCoroutine(escapingCoro);
        StartCoroutine(EscapeIE(escapeVector));
    }

    private IEnumerator EscapeIE(Vector3 escapeVector)
    {
        fishAni.SetBool("run", false);
        while (!IsOutScreen())
        {
            transform.position += escapeVector * Time.deltaTime * speed;
            yield return null;
        }
    }

    private float RunCurve(float x)
    {
        return ((Mathf.Sin(5 * x) + Mathf.Cos(4 * x) - Mathf.Cos(3 * x))) / 3 + 0.5f;
    }

    public void AquaSwim()
    {
        var orgPos = transform.position;
        StartCoroutine(AquaSwimIE(orgPos));

    }
  


  private IEnumerator AquaSwimIE(Vector3 orgPos)
    {
        float lastTargetY = orgPos.y;
        while (true)
        {
            float minY = (lastTargetY - 3) < 0 ? 0 : lastTargetY - 3;
            float maxY = (lastTargetY + 3) > 15 ? 15 : lastTargetY + 3;
            Vector3 targetPos = new Vector3(Random.Range(-6, 6), Random.Range(minY, maxY), 0);
            Vector3 toDirection = (targetPos - transform.position).normalized;
            int way = toDirection.x > 0 ? 1 : -1;

            transform.localScale = new Vector3(2*way, 2, 2);

            while (!ToTarget(targetPos,2))
            { yield return null; }



        }
    }


    internal bool ToTarget(Vector3 targetPos, float speed)
    {

        var _pos = transform.position;
        if (Vector3.Distance(targetPos, _pos) < 0.1f)
        { return true; }
        Vector3 fromDirection = transform.up;
        Vector3 toDirection = (targetPos - transform.position).normalized;
       
      //  transform.rotation = Quaternion.FromToRotation(Vector3.right, toDirection);

        transform.position += toDirection * speed * Time.deltaTime;

        return false;
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
            _eatingFish.GetOut();
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