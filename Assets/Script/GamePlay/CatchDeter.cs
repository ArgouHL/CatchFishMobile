using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System;
using TMPro;

public class CatchDeter : MonoBehaviour
{
    public static CatchDeter instance;
    private CanvasGroup ui;
    private Slider deterBar;
    // private int targetTimes;
    private int nowTimes;
    Coroutine deteCoro;
    public delegate void DeteEvent(bool success);
    public static DeteEvent EndDete;
    float deterValue = 100;
    [SerializeField] private TMP_Text timer;

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
    public void StartDete(string fishID)
    {

        if (deteCoro != null)
            return;
        deteCoro = StartCoroutine(deteIE(fishID));


    }


    public IEnumerator deteIE(string fishID)
    {
        PlayerInputManager.instance.ChangeType(InputType.Determine);
        if (!FishData.instance.GetFishDeteData(fishID, out float deterTime, out float deterSpeed))
            Debug.Log("No Fish Data");
        ui.alpha = 1;
        float time = 0;
        deterValue = 100;
        CheckTime(ref deterTime, deterSpeed);
        deterBar.maxValue = deterValue;
        deterBar.value = deterValue;
        do
        {


            deterValue -= deterSpeed * Time.deltaTime;
            UpdateBar(deterValue, time, deterTime);
            time += Time.deltaTime;
            yield return null;
        }
        while (time < deterTime);
        deteCoro = null;
        StopDete(true);
    }

    private void CheckTime(ref float deterTime, float deterSpeed)
    {
        if(deterSpeed* deterTime<100)
        {
            deterTime = 100 / deterSpeed +0.5f;
        }
    }

    private void CheckDeterValue(float v)
    {
        if (v <= 0)
        {
            CancelDete();
        }
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

        Debug.Log("Dete:" + isSuccess);
        ShowIsSuccess(isSuccess);
        EndDete.Invoke(isSuccess);

    }

    private void ShowIsSuccess(bool isSuccess)
    {

    }

    public void DeteHit(InputAction.CallbackContext ctx)
    {
        if (deterValue + 10 > 100)
        {
            deterValue = 100;
        }
        else
            deterValue += 10    ;
    }


    private void UpdateBar(float deterValue, float time, float deterTime)
    {
        deterBar.value = deterValue;
        timer.text = (deterTime - time).ToString("F");
        CheckDeterValue(deterValue);
    }

    internal void CancelDete()
    {
        if (deteCoro == null)
            return;
        StopCoroutine(deteCoro);
        deteCoro = null;
        StopDete(false);
    }
}
