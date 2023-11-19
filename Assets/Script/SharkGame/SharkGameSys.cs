using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SharkGameSys : MonoBehaviour
{
    [SerializeField] private int gameTime = 60;
    [SerializeField] private SharkGameSet pacific, indian, atlantic;
    private SharkGameSet _setting;
    private PPShark _shark;
    int hitTime = 0;
    int remainHP = 0;
    private InputAction touchPosition;

    void Start()
    {
        PlayerInputManager.instance.ChangeType(InputType.GamePlay);
        touchPosition = PlayerInputManager.inputs.GamePlay.TouchPosition;
        Init();
        //MusicControl.instance.PlayBGM(bgmType.Sea1);
        GamePreStart();
    }

    private void Init()
    {
        switch (TempData.instance.targetReagon)
        {
            case FishReagon.Pacific:
                _setting = pacific;
                break;
            case FishReagon.Indian:
                _setting = indian;
                break;
            case FishReagon.Atlantic:
                _setting = atlantic;
                break;
        }

        _shark= Instantiate(_setting.shark).GetComponent<PPShark>();
        SetHp(_setting.hp);
        

    }

  

    private void OnEnable()
    {
        PlayerInputManager.inputs.GamePlay.Hit.performed += GetShark;

    }
    private void OnDisable()
    {
        PlayerInputManager.inputs.GamePlay.Hit.performed -= GetShark;
    }

    public void GamePreStart()
    {
        StartCoroutine(PreStart());
        //  isGameEnd = false;
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
        _shark.Swim();
        StartCoroutine(GameCountDown());
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
        //GameStop();
    }
    public void GetShark(InputAction.CallbackContext ctx)
    {
        Debug.Log("GetShark");
        //var _hitPos = (Vector2)Camera.main.ScreenToWorldPoint(touchPosition.ReadValue<Vector2>());
        var _hitPos = touchPosition.ReadValue<Vector2>();
        Debug.Log(_hitPos);
        HitShark(_hitPos);
    }

    public void HitShark(Vector2 touchPos)
    {
        var _fish = TouchFunc.FindClosestShark(touchPos);
        if (_fish == null)
            return;
        if (!_fish.canbeClick)
            return;
        TargetMarkCtr.instance.StartTracking(_fish.gameObject.transform);
        hitTime++;

        SfxControl.instance.HitPlay(10 - hitTime);
        // SfxControl.instance.HitPlay(remainHitTime);
        if (hitTime >= 10)
        {

            hitTime = 0;
            ChangeHp(-1);
            SfxControl.instance.CatchPlay();



        }
        // UpdateHit();
    }

    private void SetHp(int hp)
    {
        remainHP = hp;
        GameInformationShow.instance.UpdateHpCount(hp);
    }

    private void ChangeHp(int change)
    {
        remainHP += change;
        GameInformationShow.instance.UpdateHpCount(remainHP);
        if(remainHP<=0)
        {
            Debug.Log("win");
        }
    }

    public void Shock()
    {
        _shark.Shocked();
    }

}
