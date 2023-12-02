using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PPShark : MonoBehaviour
{
    internal bool canBeHit = false;
    private Coroutine nowAction = null;
    private SpriteRenderer spriteRenderer;
    [SerializeField] private float speed = 8;
    internal int swimTimeIndex=1;


    private void Start()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();



    }

    internal void SwimToNext()
    {
        spriteRenderer.color = Color.white;
        if (nowAction != null)
            StopCoroutine(nowAction);
        nowAction = StartCoroutine(SwimIE());
    }

    internal bool ToTarget(Vector3 targetPos, float dis, float speed)
    {

        var _pos = transform.position;
        if (Vector3.Distance(targetPos, _pos) < dis)
        { return true; }
        Vector3 fromDirection = transform.up;
        Vector3 toDirection = (targetPos - transform.position).normalized;
        int way = toDirection.x > 0 ? 1 : -1;

        spriteRenderer.flipY = way < 0 ? true : false;

        transform.rotation = Quaternion.FromToRotation(Vector3.right, toDirection);

        transform.position += toDirection * speed * Time.deltaTime;

        return false;
    }
    private IEnumerator SwimIE()
    {
        //play swim Ani
        spriteRenderer.color = Color.white;
        Debug.Log("SwimIE");
        int times =Random.Range(swimTimeIndex, swimTimeIndex + 2);
        for (int i=0;i< times; i++)
        {
            Vector3 targetPos = new Vector3(Random.Range(-5f, 5f), Random.Range(-6f, 4f), 0);
            while (!ToTarget(targetPos, 0.2f, speed))
            {
                yield return null;
            }
        }
        
        nowAction = StartCoroutine(RestIE());
    }

    private IEnumerator RestIE()
    {
        canBeHit = true;
        yield return new WaitForSeconds(2);
        canBeHit = false;
        SwimToNext();
    }


    internal void BeHit(float duration)
    {
        canBeHit = false;
        if (nowAction != null)
            StopCoroutine(nowAction);
        nowAction = StartCoroutine(BeHitIE(duration));

    }
    private IEnumerator BeHitIE(float duration)
    {
        //be hitted animation
        LeanTween.value(0, 1, duration).setOnUpdate((float val) => spriteRenderer.color = new Color(1, val, val, 1));
        yield return new WaitForSeconds(duration);
       

    }

    internal void Skillpre()
    {

        if (nowAction != null)
            StopCoroutine(nowAction);
        nowAction = StartCoroutine(SkillpreIE());
    }

    private IEnumerator SkillpreIE()
    {
     

        //play pre Ani
        float time = 0;
        while (true)
        {
            float d = Mathf.Sin(time * 10) / 2 + 0.5f;
            //  Debug.Log(d);
            spriteRenderer.color = Color.Lerp(new Color(0.5f, 1, 1, 1), Color.yellow, d);
            time += Time.deltaTime;
            yield return null;
        }


    }

    internal void Shocked()
    {
        if (nowAction != null)
            StopCoroutine(nowAction);
        nowAction = StartCoroutine(ShockedIE());
    }


    private IEnumerator ShockedIE()
    {
        Debug.Log("ShockedIE");

        //play shock Ani
        float time = 0;
        while (true)
        {
            float d = Mathf.Sin(time * 50) / 2 + 0.5f;
            //Debug.Log(d);
            spriteRenderer.color = Color.Lerp(Color.white, Color.yellow, d);
            time += Time.deltaTime;
            yield return null;
        }



    }

    internal void Hide()
    {

        spriteRenderer.color = new Color(1, 1, 1, 0);
        if (nowAction != null)
            StopCoroutine(nowAction);
        nowAction = StartCoroutine(HideIE());
    }

    private IEnumerator HideIE()
    {
        Debug.Log("HideIE");

        yield return new WaitForSeconds(5);


    }

    internal void StopCoro()
    {
        if (nowAction != null)
            StopCoroutine(nowAction);
    }



}
