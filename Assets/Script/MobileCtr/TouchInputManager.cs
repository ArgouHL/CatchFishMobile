using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

[DefaultExecutionOrder(-1)]
public class TouchInputManager : Singleton<TouchInputManager>
{
    public static TouchInputManager instance;



    public delegate void TouchEvent(Vector2 position, float time);
    public static TouchEvent OnStartTouch;
    public static TouchEvent OnEndTouch;
    public static TouchEvent OnStartTouchShop;
    public static TouchEvent OnEndTouchShop;
   


    public delegate void OneTouchEvent(InputAction.CallbackContext ctx);
    public static OneTouchEvent GamePlayOneTouch;


    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }

    }


    //private void OnEnable()
    //{
    //    PlayerInputManager.inputs.Puzzle.Enable();

    //}

    //private void OnDisable()
    //{
    //    PlayerInputManager.inputs.Puzzle.Disable();

    //}


    private void Start()
    {

#if UNITY_ANDROID
#else
        gameObject.SetActive(false);
#endif



    }

    private void OnEnable()
    {
        Debug.Log(PlayerInputManager.inputs == null ? "y":"n"); 
        PlayerInputManager.inputs.Lobby.Touch.started +=StartTouchPrimary;
        PlayerInputManager.inputs.Lobby.Touch.canceled += EndTouchPrimary;
        
        PlayerInputManager.inputs.Shop.Touch.started += ctx => StartTouchShop(ctx);
        PlayerInputManager.inputs.Shop.Touch.canceled += ctx => EndTouchShop(ctx);
        //PlayerInputManager.inputs.CheckItem.Touch.started += ctx => StartTouchCheckItem(ctx);
        //PlayerInputManager.inputs.CheckItem.Touch.canceled += ctx => EndTouchCheckItem(ctx);

    }

    private void OnDisable()
    {
        PlayerInputManager.inputs.Lobby.Touch.started -= StartTouchPrimary;
        PlayerInputManager.inputs.Lobby.Touch.canceled -= EndTouchPrimary;
        //PlayerInputManager.inputs.Puzzle.Touch.started -= StartTouchPrimary;
        //PlayerInputManager.inputs.Puzzle.Touch.canceled -= EndTouchPrimary;
       
        PlayerInputManager.inputs.Shop.Touch.started -= ctx => StartTouchShop(ctx);
        PlayerInputManager.inputs.Shop.Touch.canceled -= ctx => EndTouchShop(ctx);
        //PlayerInputManager.inputs.CheckItem.Touch.started -= ctx => StartTouchCheckItem(ctx);
        //PlayerInputManager.inputs.CheckItem.Touch.canceled -= ctx => EndTouchCheckItem(ctx);

    }



    private void StartTouchPrimary(InputAction.CallbackContext ctx)
    {

        var _pos = Camera.main.ScreenToWorldPoint(PlayerInputManager.inputs.Lobby.TouchPosition.ReadValue<Vector2>());
        _pos.z = Camera.main.nearClipPlane;
        if (OnStartTouch != null)
            OnStartTouch(_pos, (float)ctx.startTime);


    }

    private void EndTouchPrimary(InputAction.CallbackContext ctx)
    {
        var _pos = Camera.main.ScreenToWorldPoint(PlayerInputManager.inputs.Lobby.TouchPosition.ReadValue<Vector2>());
        _pos.z = Camera.main.nearClipPlane;
        if (OnEndTouch != null) OnEndTouch(_pos, (float)ctx.time);
    }

    private void StartTouchShop(InputAction.CallbackContext ctx)
    {
        var _pos = Camera.main.ScreenToWorldPoint(PlayerInputManager.inputs.Shop.TouchPosition.ReadValue<Vector2>());
        _pos.z = Camera.main.nearClipPlane;
        if (OnStartTouchShop != null)
            OnStartTouchShop(_pos, (float)ctx.startTime);
    }

    private void EndTouchShop(InputAction.CallbackContext ctx)
    {
        var _pos = Camera.main.ScreenToWorldPoint(PlayerInputManager.inputs.Shop.TouchPosition.ReadValue<Vector2>());
        _pos.z = Camera.main.nearClipPlane;
        if (OnEndTouchShop != null) OnEndTouchShop(_pos, (float)ctx.time);
    }


    private void StartTouchCheckItem(InputAction.CallbackContext ctx)
    {
        //var _pos = Camera.main.ScreenToWorldPoint(PlayerInputManager.inputs.CheckItem.TouchPosistion.ReadValue<Vector2>());
        //_pos.z = Camera.main.nearClipPlane;
        //if (OnStartTouchCheck != null)
        //    OnStartTouchCheck(_pos, (float)ctx.startTime);
    }

    private void EndTouchCheckItem(InputAction.CallbackContext ctx)
    {
        //var _pos = Camera.main.ScreenToWorldPoint(PlayerInputManager.inputs.CheckItem.TouchPosistion.ReadValue<Vector2>());
        //_pos.z = Camera.main.nearClipPlane;
        //if (OnEndTouchCheck != null) OnEndTouchCheck(_pos, (float)ctx.time);
    }



    public Vector2 PrimaryPos()
    {
        var _pos = Vector2.zero;
        //var _pos = Camera.main.ScreenToWorldPoint(PlayerInputManager.inputs.Puzzle.TouchPosistion.ReadValue<Vector2>());
        //_pos.z = Camera.main.nearClipPlane;
        return _pos;
    }
}
