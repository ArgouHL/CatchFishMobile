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

    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        PlayerInputManager.instance.ChangeType(InputType.GamePlay);
        touchPosition = PlayerInputManager.inputs.GamePlay.TouchPosition;

        OrderManager.instance.GetOrders();
        MusicControl.instance.PlayBGM(bgmType.Sea1);    
    }


    private void OnEnable()
    {
        PlayerInputManager.inputs.GamePlay.Hit.performed += GetFish;

    }
    private void OnDisable()
    {
        PlayerInputManager.inputs.GamePlay.Hit.performed -= GetFish;
    }

    public void GamePreStart()
    {
        StartCoroutine(PreStart());
        isGameEnd = false;
    }

    public void GetFish(InputAction.CallbackContext ctx)
    {
        Debug.Log("GetFish");
        //var _hitPos = (Vector2)Camera.main.ScreenToWorldPoint(touchPosition.ReadValue<Vector2>());
        var _hitPos = touchPosition.ReadValue<Vector2>();
        Debug.Log(_hitPos);
        FishControl.instance.HitFish(_hitPos);
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
        FishControl.instance.StartGenFish();
        StartCoroutine(GameCountDown());
        StartCoroutine(SpawnShark());
    }

    private IEnumerator GameCountDown()
    {
        int readytime = 45;
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
        isGameEnd = true;
    }

    private IEnumerator SpawnShark()
    {
        yield return new WaitForSeconds(1);
        FishControl.instance.SpawnShark();
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


    public void BackMenu()
    {
        SceneManager.LoadScene(1);
    }
}
