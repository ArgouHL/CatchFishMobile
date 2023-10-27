using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GamePlay : MonoBehaviour
{
    private InputAction touchPosition;
    void Start()
    {
        PlayerInputManager.instance.ChangeType(InputType.GamePlay);
        touchPosition = PlayerInputManager.inputs.GamePlay.TouchPosition;
        StartCoroutine(PreStart());
    }


    private void OnEnable()
    {
        PlayerInputManager.inputs.GamePlay.Hit.performed += GetFish;

    }
    private void OnDisable()
    {
        PlayerInputManager.inputs.GamePlay.Hit.performed -= GetFish;
    }



    public void GetFish(InputAction.CallbackContext ctx)
    {
        Debug.Log("GetFish");
       var _hitPos=(Vector2)Camera.main.ScreenToWorldPoint(touchPosition.ReadValue<Vector2>());
        Debug.Log(_hitPos);
        FishControl.instance.HitFish(_hitPos);
    }

    
    private IEnumerator PreStart()
    {
        yield return FadeInWait();
       int readytime = 3; 
       
        while(readytime >= 0)
        {
            GameInformationShow.instance.UpdatePreCount(readytime);
            readytime--;
            yield return new WaitForSeconds(1);
        }
        GameInformationShow.instance.HidePreCountUI();
        GameStart();
    }

    private IEnumerator FadeInWait()
    {
        yield return new WaitForSeconds(1);
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
        GameInformationShow.instance.UpdateCountDown(readytime);
        while (readytime >= 0)
        {
            //GameInformationShow.instance.UpdatePreCount(readytime);
            readytime--;
            yield return new WaitForSeconds(1);
        }
        GameStop();
    }

    private void GameStop()
    {
        throw new NotImplementedException();
    }
}
