using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PPShark : MonoBehaviour
{

    private Coroutine nowAction=null;
    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();

    }

    internal void Swim()
    {
        spriteRenderer.color = Color.white;
        if(nowAction!=null)
        StopCoroutine(nowAction);
        nowAction = StartCoroutine(SwimIE());
    }


    private  IEnumerator SwimIE()
    {
        //play swim Ani
        spriteRenderer.color = Color.white;
        Debug.Log("SwimIE");
        yield return new WaitForSeconds(5);
        
       
    }

    internal void Skillpre()
    {
        if (nowAction != null)
            StopCoroutine(nowAction);
        nowAction = StartCoroutine(SkillpreIE());
    }

    private IEnumerator SkillpreIE()
    {
        Debug.Log("Skillpre");

        //play pre Ani
        float time = 0;
        while (true)
        {
            float d = Mathf.Sin(time * 10) / 2 + 0.5f;
          //  Debug.Log(d);
            spriteRenderer.color = Color.Lerp(new Color(0.5f,0,0,1), Color.red, d);
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
            float d = Mathf.Sin(time*50) / 2 +0.5f;
            //Debug.Log(d);
            spriteRenderer.color = Color.Lerp(Color.white, Color.yellow, d);
            time += Time.deltaTime;
            yield return null;
        }
       
       
        
    }

    internal void Hide()
    {
        spriteRenderer.color = new Color(1,1,1,0);
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
