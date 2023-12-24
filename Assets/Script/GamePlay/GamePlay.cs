using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GamePlay : MonoBehaviour
{
    private InputAction touchPosition;
    public static bool isGameEnd = true;
    public static GamePlay instance;

    public delegate void OrderCheck(Fish fish);
    public static OrderCheck CatchedFish;

    public bool hasShockUsed=false;
    public bool hasBaitUsed = false;
    [SerializeField]private float itemOrderDuration;
    private Coroutine ShockCoroutine;
    private Coroutine BaitCoroutine;
    
    [SerializeField] private int gameTime = 30;
    [SerializeField] private Transform catchEffectTransform;
    private Animator catchEffectAni;
    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
    
        PlayerInputManager.instance.ChangeType(InputType.None);
        touchPosition = PlayerInputManager.inputs.GamePlay.TouchPosition;

        OrderManager.instance.GetOrders();
        MusicControl.instance.PlayBGM(bgmType.Sea1);
        ResultRecord.instance.Init();

        catchEffectAni = catchEffectTransform.GetComponent<Animator>();

    }


    private void OnEnable()
    {
      //  DragControl.dragoff += GetFish;
        GameInformationShow.StopCoro += StopCoro;
        MarkerHit.HitFish += GetFish;
        //PlayerInputManager.inputs.GamePlay.Hit.performed += GetFish;
    }

    

    private void OnDisable()
    {
       // DragControl.dragoff -= GetFish;
      //  CatchDeter.EndDete -= GetFishDete;
        // PlayerInputManager.inputs.GamePlay.Hit.performed -= GetFish;
        GameInformationShow.StopCoro -= StopCoro;
        MarkerHit.HitFish -= GetFish;
    }

    public void GamePreStart()
    {
        StartCoroutine(PreStart());
        isGameEnd = false;
    }

    public void GetFish(Fish fish)
    {
        Debug.Log("GetFish");
        //var _hitPos = (Vector2)Camera.main.ScreenToWorldPoint(touchPosition.ReadValue<Vector2>());
       // var _hitPos = touchPosition.ReadValue<Vector2>();
      //  Debug.Log(aimPoint);
        FishControl.instance.HitFish(fish);
    }




    private IEnumerator PreStart()
    {

        int readytime = 1;

        while (readytime >= 0)
        {
            GameInformationShow.instance.UpdatePreCount(readytime);
            readytime--;
            yield return new WaitForSeconds(1);
        }
        GameInformationShow.instance.HidePreCountUI();
        GameStart();
    }


    private void GameStart()
    {
        PlayerInputManager.instance.ChangeType(InputType.GamePlay);
        FishControl.instance.StartGenFish();
        StartCoroutine(GameCountDown());
       // StartCoroutine(SpawnShark());
    }

    private IEnumerator GameCountDown()
    {
        int readytime = gameTime;
        yield return new WaitForSeconds(1);

        while (readytime >= 0)
        {
            GameInformationShow.instance.UpdateCountDown(readytime);
            //GameInformationShow.instance.UpdatePreCount(readytime);
            readytime--;
            yield return new WaitForSeconds(1);
        }
        GameInformationShow.instance.UpdateCountDown(readytime);
        GameStop();
    }

    private void GameStop()
    {
        PlayerInputManager.instance.ChangeType(InputType.None);
        isGameEnd = true;
        OrderManager.instance.SendOrders();
        ResultRecord.instance.CalcResult();
        StartCoroutine(LoadResultScene());
        MusicControl.instance.SoftStopBGM();
    }

    private IEnumerator LoadResultScene()
    {
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(3);
    }



    public void StartShockCount()
    {
        if (ShockCoroutine != null)
            StopCoroutine(ShockCoroutine);
        ShockCoroutine = StartCoroutine(ShockIE());
    }

    private IEnumerator ShockIE()
    {
        hasShockUsed = true;
        yield return new WaitForSeconds(itemOrderDuration);
        hasShockUsed = false;
        OrderManager.instance.ResetItemCount(ItemType.Shock);
        ShockCoroutine = null;
    }

    public void BaitCount()
    {
        if (BaitCoroutine != null)
            StopCoroutine(BaitCoroutine);
        BaitCoroutine = StartCoroutine(BaitIE());
    }

    private IEnumerator BaitIE()
    {
        hasBaitUsed = true;
        yield return new WaitForSeconds(itemOrderDuration);
        hasBaitUsed = false;
        OrderManager.instance.ResetItemCount(ItemType.Bait);
        BaitCoroutine = null;
    }

    public void StopCoro()
    {
        StopAllCoroutines();
    }
    
    internal void PlayCatchEff(Vector3 worldPos)
    {
        catchEffectTransform.position = worldPos;
        catchEffectAni.SetTrigger("Atk");
    }
}
