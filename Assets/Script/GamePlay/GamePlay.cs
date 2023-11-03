using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GamePlay : MonoBehaviour
{
    private InputAction touchPosition;
    public static bool isGameEnd = true;
    public static GamePlay instance;

    public delegate void OrderCheck(Fish fish);
    public static OrderCheck CatchedFish;


    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        PlayerInputManager.instance.ChangeType(InputType.GamePlay);
        touchPosition = PlayerInputManager.inputs.GamePlay.TouchPosition;

        OrderManager.instance.GetOrders();


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
        var _hitPos = (Vector2)Camera.main.ScreenToWorldPoint(touchPosition.ReadValue<Vector2>());
        Debug.Log(_hitPos);
        FishControl.instance.HitFish(_hitPos);
    }


    private IEnumerator PreStart()
    {

        int readytime = 3;

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
}
