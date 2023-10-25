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

}
