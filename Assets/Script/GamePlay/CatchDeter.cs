using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System;

public class CatchDeter : MonoBehaviour
{
    public static CatchDeter instance;
    private CanvasGroup ui;
    private Slider deterBar;
    private int targetTimes;
    private int nowTimes;
    Coroutine deteCoro;
    public delegate void DeteEvent(bool success);
    public static DeteEvent EndDete;



    private void OnEnable()
    {
        PlayerInputManager.inputs.Determine.Touch.performed += DeteHit;
    }

    private void OnDisable()
    {
        PlayerInputManager.inputs.Determine.Touch.performed -= DeteHit;
    }
    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        ui = GetComponent<CanvasGroup>();
        deterBar = GetComponentInChildren<Slider>();
    }
    public void StartDete(int hittime, float remainTime)
    {

        if (deteCoro != null)
            return;
        deteCoro = StartCoroutine(deteIE(hittime, remainTime));


    }


    public IEnumerator deteIE(int hittime, float remainTime)
    {
        ui.alpha = 1;
        targetTimes = hittime;
        nowTimes = 0;
        UpdateBar();
        PlayerInputManager.instance.ChangeType(InputType.Determine);
        yield return new WaitForSeconds(remainTime);
        deteCoro = null;
        StopDete(false);
    }

    internal void fishBeEated()
    {
        StopCoroutine(deteCoro);
        deteCoro = null;
        StopDete(false);
    }

    private void StopDete(bool isSuccess)
    {
        ui.alpha = 0;
        EndDete.Invoke(isSuccess);
        Debug.Log("Dete:" + isSuccess);

    }

    public void DeteHit(InputAction.CallbackContext ctx)
    {
        if (deteCoro == null)
            return;
        nowTimes++;
        SfxControl.instance.HitPlay(nowTimes);
        UpdateBar();
        if (nowTimes >= targetTimes)
        {
            StopCoroutine(deteCoro);
            deteCoro = null;
            StopDete(true);

        }
    }


    private void UpdateBar()
    {
        deterBar.maxValue = targetTimes;
        deterBar.value = nowTimes;
    }

    internal void CancelDete()
    {
        if (deteCoro == null)
            return;
        StopCoroutine(deteCoro);
        deteCoro = null;
        StopDete(true);
    }
}
