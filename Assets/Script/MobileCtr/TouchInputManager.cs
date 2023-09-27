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


    public delegate void TouchEventScan(Vector2 position, float time);
    public static TouchEventScan OnStartTouchScan;
    public static TouchEventScan OnEndTouchScan;

    public delegate void TouchEvent(Vector2 position, float time);
    public static TouchEvent OnStartTouch;
    public static TouchEvent OnEndTouch;

    public delegate void CheckItemEvent(Vector2 position, float time);
    public static TouchEvent OnStartTouchCheck;
    public static TouchEvent OnEndTouchCheck;

    public delegate void StartOneTouch(InputAction.CallbackContext ctx);
    public static StartOneTouch PuzzleOneTouch;

    public delegate void UIOneTouchEvent(InputAction.CallbackContext ctx);
    public static UIOneTouchEvent UIOneTouch;



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
        //Debug.Log(PlayerInputManager.inputs == null ? "y":"n"); 
        //PlayerInputManager.inputs.Puzzle.Touch.started +=StartTouchPrimary;
        //PlayerInputManager.inputs.Puzzle.Touch.canceled += EndTouchPrimary;
        //PlayerInputManager.inputs.UI.Touch.started += ctx => OneTouchPrimary(ctx);
        //PlayerInputManager.inputs.Player.Touch.started += ctx => StartTouchScan(ctx);
        //PlayerInputManager.inputs.Player.Touch.canceled += ctx => EndTouchScan(ctx);
        //PlayerInputManager.inputs.CheckItem.Touch.started += ctx => StartTouchCheckItem(ctx);
        //PlayerInputManager.inputs.CheckItem.Touch.canceled += ctx => EndTouchCheckItem(ctx);

    }

    private void OnDisable()
    {

        //PlayerInputManager.inputs.Puzzle.Touch.started -= StartTouchPrimary;
        //PlayerInputManager.inputs.Puzzle.Touch.canceled -= EndTouchPrimary;
        //PlayerInputManager.inputs.UI.Touch.started -= ctx => OneTouchPrimary(ctx);
        //PlayerInputManager.inputs.Player.Touch.started -= ctx => StartTouchScan(ctx);
        //PlayerInputManager.inputs.Player.Touch.canceled -= ctx => EndTouchScan(ctx);
        //PlayerInputManager.inputs.CheckItem.Touch.started -= ctx => StartTouchCheckItem(ctx);
        //PlayerInputManager.inputs.CheckItem.Touch.canceled -= ctx => EndTouchCheckItem(ctx);

    }

    private void OneTouchPrimary(InputAction.CallbackContext ctx)
    {
        Debug.Log("UITouch");
        if (UIOneTouch != null) UIOneTouch(ctx);
    }

    private void StartTouchPrimary(InputAction.CallbackContext ctx)
    {

        //var _pos = Camera.main.ScreenToWorldPoint(PlayerInputManager.inputs.Puzzle.TouchPosistion.ReadValue<Vector2>());
        //_pos.z = Camera.main.nearClipPlane;
        //if (OnStartTouch != null)
        //    OnStartTouch(_pos, (float)ctx.startTime);


    }

    private void EndTouchPrimary(InputAction.CallbackContext ctx)
    {
        //var _pos = Camera.main.ScreenToWorldPoint(PlayerInputManager.inputs.Puzzle.TouchPosistion.ReadValue<Vector2>());
        //_pos.z = Camera.main.nearClipPlane;
        //if (OnEndTouch != null) OnEndTouch(_pos, (float)ctx.time);
    }

    private void StartTouchScan(InputAction.CallbackContext ctx)
    {
        //var _pos = Camera.main.ScreenToWorldPoint(PlayerInputManager.inputs.Player.TouchPosistion.ReadValue<Vector2>());
        //_pos.z = Camera.main.nearClipPlane;
        //if (OnStartTouchScan != null)
        //    OnStartTouchScan(_pos, (float)ctx.startTime);
    }

    private void EndTouchScan(InputAction.CallbackContext ctx)
    {
        //var _pos = Camera.main.ScreenToWorldPoint(PlayerInputManager.inputs.Player.TouchPosistion.ReadValue<Vector2>());
        //_pos.z = Camera.main.nearClipPlane;
        //if (OnEndTouchScan != null) OnEndTouchScan(_pos, (float)ctx.time);
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
