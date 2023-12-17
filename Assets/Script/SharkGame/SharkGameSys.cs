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
    private int nowHP;
    private int maxHP;
    private InputAction touchPosition;
    private SharkState state;
    private Coroutine SkillPreCoro;

    private float hpThreshold;
    [SerializeField] private float deteTime = 1f;
    
    int hpThresholdIndex = 0;

    void Start()
    {
        PlayerInputManager.instance.ChangeType(InputType.None);
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

        _shark = Instantiate(_setting.shark).GetComponent<PPShark>();
        SetHp(_setting.hp);
    }

    private void OnEnable()
    {
     //   DragControl.dragoff += GetShark;
        PlayerInputManager.inputs.Determine.Touch.performed += DeteHit;
        //  PlayerInputManager.inputs.GamePlay.Touch.performed += GetShark;
        GameInformationShow.StopCoro += StopCoro;
        // PlayerInputManager.inputs.Determine.Touch.performed += DeteHit;
    }

   

    private void OnDisable()
    {
       // DragControl.dragoff -= GetShark;
        GameInformationShow.StopCoro -= StopCoro;
        PlayerInputManager.inputs.Determine.Touch.performed -= DeteHit;
        // PlayerInputManager.inputs.Determine.Touch.performed -= DeteHit;
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
        PlayerInputManager.instance.ChangeType(InputType.GamePlay);
        SharkSwim();
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
    public void GetShark(Vector3 aimPoint)
    {
        Debug.Log("GetShark");
        HitShark(aimPoint);
    }

    public void HitShark(Vector3 aimPoint)
    {
        Debug.Log("HitShark");
        var _fish = TouchFunc.FindClosestShark(aimPoint);
        if (_fish == null)
            return;
        Debug.Log("Found");
        ChangeHp(1);

    }

    private void SetHp(int hp)
    {
        nowHP = hp;
        maxHP = hp;
        hpThreshold = _setting.hpThresholds[0];
        GameInformationShow.instance.UpdateHpCount(hp);
    }

    private void ChangeHp(int change)
    {
        if (!_shark.canBeHit)
            return;
        nowHP -= change;
        GameInformationShow.instance.UpdateHpCount(nowHP);
        float duration = 1;
        _shark.BeHit(duration);
        if (nowHP <= 0)
        {
            Debug.Log("win");
        }
        else if ((float)nowHP / (float)maxHP <= hpThreshold)
        {
            PlayerInputManager.instance.ChangeType(InputType.None);
            _shark.swimTimeIndex++;
            Debug.Log((float)nowHP / (float)maxHP);
            Debug.Log(hpThreshold);
            if (hpThresholdIndex < _setting.hpThresholds.Length - 1)
                hpThresholdIndex++;
            hpThreshold = _setting.hpThresholds[hpThresholdIndex];

            SkillPre();
        }
        else
        {
            LeanTween.delayedCall(duration, () => SharkSwim());
        }      
       
    }





    private void SharkSwim()
    {
      //  ChangeState(SharkState.swimming);
        _shark.SwimToNext();
  
    }


    private void SkillPre()
    {
        Debug.Log("SkillPre");
        _shark.StopCoro();
        _shark.Skillpre();
       // ChangeState(SharkState.skillPre);
        if (SkillPreCoro != null)
            return;
        SkillPreCoro= StartCoroutine(SkillPreDete());
    }


    private IEnumerator SkillPreDete()
    {
     
        DeteBar.instance.ShowBar(deteTime);
        float showUpTime = 1.5f;
        yield return new WaitForSeconds(showUpTime);
        PlayerInputManager.instance.ChangeType(InputType.Determine);
        
        //showbar
      
     
        float time = 0;

        while (time < deteTime)
        {
            DeteBar.instance.UpdateBar(time);
            time += Time.deltaTime;
            yield return null;
        }
      

            SharkHide();

        DeteBar.instance.StopDete(false);

        SkillPreCoro = null;
        LeanTween.delayedCall(2, () => PlayerInputManager.instance.ChangeType(InputType.GamePlay));
        
    }


    private void DeteHit(InputAction.CallbackContext obj)
    {
        if (SkillPreCoro != null)
            StopCoroutine(SkillPreCoro);
        SkillPreCoro = null;
        Shocked();
        PlayerInputManager.instance.ChangeType(InputType.None);
        DeteBar.instance.StopDete(true);
        LeanTween.delayedCall(2, () => PlayerInputManager.instance.ChangeType(InputType.GamePlay));

    }


    private void Shocked()
    {
        _shark.Shocked();
        StartCoroutine(ShockIE());
    }


    private IEnumerator ShockIE()
    {
     
        yield return new WaitForSeconds(2);
        SharkSwim();
    }



    private void SharkHide()
    {
        StartCoroutine(SharkHideIE());
    }


    private IEnumerator SharkHideIE()
    {
        _shark.Hide();
        yield return new WaitForSeconds(3);
        SharkSwim();
    }




    public void StopCoro()
    {
        StopAllCoroutines();
    }
}

public enum SharkState { swimming, skillPre, hiding }

