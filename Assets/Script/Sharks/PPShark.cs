using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PPShark : MonoBehaviour
{
    internal bool canbeClick = false;
    internal bool canbeShock = false;
    private Coroutine nowAction=null;
    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();

    }

    internal void Swim()
    {
        spriteRenderer.color = Color.white;
        canbeShock = true;
        canbeClick = true;
        if (nowAction != null)
            return;
        nowAction = StartCoroutine(SwimIE());
    }

    private  IEnumerator SwimIE()
    {
        //play swim Ani
        Debug.Log("SwimIE");
        yield return new WaitForSeconds(5);
        nowAction = null;
        Skillpre();
    }

    private void Skillpre()
    {

        if (nowAction != null)
            return;
        nowAction = StartCoroutine(SkillpreIE());
    }

    private IEnumerator SkillpreIE()
    {
        Debug.Log("Skillpre");

        //play pre Ani
        float time = 0;
        while(time<2)
        {
            spriteRenderer.color = Color.Lerp(Color.white, Color.red, time / 2);
            time += Time.deltaTime;
            yield return null;
        }
        spriteRenderer.color = new Color(1, 1, 1, 0);
        nowAction = null;
        Hide();
    }

    internal void Shocked()
    {
        if (!canbeShock)
            return;
        StopCoroutine(nowAction);
        nowAction = StartCoroutine(ShockedIE());
    }


    private IEnumerator ShockedIE()
    {
        Debug.Log("ShockedIE");
        canbeShock = false;
        canbeClick = true;
        //play shock Ani
        float time = 0;
        while (time < 2.5f)
        {
            float d = Mathf.Sin(time*50) / 2 +0.5f;
            Debug.Log(d);
            spriteRenderer.color = Color.Lerp(Color.white, Color.yellow, d);
            time += Time.deltaTime;
            yield return null;
        }
        spriteRenderer.color = Color.white;
        nowAction = null;
        Swim();
    }

    private void Hide()
    {      
        nowAction = StartCoroutine(HideIE());
    }

    private IEnumerator HideIE()
    {
        Debug.Log("HideIE");
        canbeShock = false;
        canbeClick = false;
        yield return new WaitForSeconds(5);
        nowAction = null;
        Swim();
    }
}
