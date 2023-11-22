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
    private SharkState state;
    private Coroutine nowStateCoro;
    private bool deterSuccess=false;
    private bool shockSuccess = false;

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
        GameInformationShow.StopCoro += StopCoro;
    }
    private void OnDisable()
    {
        PlayerInputManager.inputs.GamePlay.Hit.performed -= GetShark;
        GameInformationShow.StopCoro -= StopCoro;
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
        if (state != SharkState.swimming)
            return;
        var _fish = TouchFunc.FindClosestShark(touchPos);
        if (_fish == null)
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

   



    private void SharkSwim()
    {
        ChangeState(SharkState.swimming);
        _shark.Swim();
        if (nowStateCoro != null)
            return;
        StartCoroutine(SwimTime());
    }

    private IEnumerator SwimTime()
    {
       float t= UnityEngine.Random.Range(5, 10);
        yield return new WaitForSeconds(5);
        nowStateCoro = null;
        SkillPre();
    }

    private void SkillPre()
    {
        hitTime = 0;
        TargetMarkCtr.instance.StopTracking();
        _shark.StopCoro();
        _shark.Skillpre();
        ChangeState(SharkState.skillPre);
        if(nowStateCoro != null)
            return;
        StartCoroutine(SkillPreDete());
    }


    private IEnumerator SkillPreDete()
    {
        float showUpTime = 0.5f;
        float prefailTime = 0.5f;
        float successTime = 1f;
        float afterfailTime = 0.5f;
        DeteBar.instance.ShowBar(successTime, prefailTime + successTime + afterfailTime);
        //showbar
        yield return new WaitForSeconds(showUpTime);
        DeteBar.instance.StartDete();
        float detetime = 0;
        deterSuccess = false;
        shockSuccess = false;
        while (detetime < prefailTime)
        {            
            DeteBar.instance.UpdateBar(detetime);
            detetime += Time.deltaTime;
            yield return null;
        }
        deterSuccess = true;
        while (detetime < prefailTime+ successTime)
        {
            DeteBar.instance.UpdateBar(detetime);
            detetime += Time.deltaTime;
            yield return null;
        }
        deterSuccess = false;
        while (detetime < prefailTime + successTime+ afterfailTime)
        {
            DeteBar.instance.UpdateBar(detetime);
            detetime += Time.deltaTime;
            yield return null;
        }
        if (shockSuccess)
            Shocked();
        else        
            SharkHide();
        
        DeteBar.instance.StopDete();     
     
        nowStateCoro = null;

    }

    private void Shocked()
    {
        _shark.Shocked();
        StartCoroutine(ShockIE());
    }


    private IEnumerator ShockIE()
    {
        ChangeState(SharkState.swimming);
        yield return new WaitForSeconds(2);
        SharkSwim();
    }

        public void Shock()
    {
        DeteBar.instance.ShockDisable();
        if (deterSuccess)
            shockSuccess = true;           

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


    private void ChangeState(SharkState s)
    {
        Debug.Log(s);
        state = s;
    }


    public void StopCoro()
    {
        StopAllCoroutines();
    }
}

public enum SharkState { swimming,skillPre,hiding}
    
