using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class ShineEffectCtr : MonoBehaviour
{
    [SerializeField] private Sprite[] effImg;
    private Image img;
    private Coroutine shineCoro;
    public delegate void CollectBtnEvent();
    public CollectBtnEvent GetReward;

    private void Start()
    {
        img = GetComponent<Image>();
        EventTrigger trigger = GetComponent<EventTrigger>();
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerDown;
        entry.callback.AddListener((data) => { Click((PointerEventData)data); });
        trigger.triggers.Add(entry);
    }

    public void Shining()
    {
        shineCoro = StartCoroutine(ShineIE());
    }

    private IEnumerator ShineIE()
    {
        int i = 0;
       
      while(true)
        {
            img.sprite = effImg[i];
            i = (i + 1) % 2;
            yield return new WaitForSeconds(0.5f);
        }    
    }

    public void StopShining()
    {
        if(shineCoro!=null)
        StopCoroutine(shineCoro);
        img.color = new Color(0, 0, 0, 0);
    }


    public void Click(PointerEventData data)
    {
        GetReward?.Invoke();
        GetReward = null;
        StopShining();
        gameObject.SetActive(false);
    }
}
