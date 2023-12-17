using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SwipeControl : MonoBehaviour
{
    public static SwipeControl instance;

    [SerializeField]
    private float minDisLR = .2f, minDisUpDown = .2f;
    [SerializeField]
    private float maxTime = 1f;
    [SerializeField,Range(0,1)]
    private float dirThreshold = .9f;


    private Vector2 startPos,endPos;
    private float startTime,endTime;


    public delegate void SwipeEvent();
    public static SwipeEvent SwipeUp, SwipeDown, SwipeLeft, SwipeRight;

    public delegate void SwipeShopPageEvent();
    public static SwipeShopPageEvent SwipeRightPage, SwipeLeftPage;

    //public delegate void SwipeCheckEvent();
    //public static SwipeCheckEvent SwipeLeftCheck, SwipeRightCheck;

    private void Awake()
    {

        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = GetComponent<SwipeControl>();
            DontDestroyOnLoad(gameObject);
        }



    }

    private void OnEnable()
    {
        TouchInputManager.OnStartTouch += SwipeStart;
        TouchInputManager.OnEndTouch += SwipeEnd;
        TouchInputManager.OnStartTouchShop += SwipeStartShop;
        TouchInputManager.OnEndTouchShop += SwipeEndShop;
        //TouchInputManager.OnStartTouchCheck += SwipeStartCheck;
        //TouchInputManager.OnEndTouchCheck += SwipeEndCheck;

    }

    private void OnDisable()
    {
        TouchInputManager.OnStartTouch -= SwipeStart;
        TouchInputManager.OnEndTouch -= SwipeEnd;
        TouchInputManager.OnStartTouchShop -= SwipeStartShop;
        TouchInputManager.OnEndTouchShop -= SwipeEndShop;
        //TouchInputManager.OnStartTouchScan -= SwipeStartScan;
        //TouchInputManager.OnEndTouchScan -= SwipeEndScan;
        //TouchInputManager.OnStartTouchCheck -= SwipeStartCheck;
        //TouchInputManager.OnEndTouchCheck -= SwipeEndCheck;
    }
      

    private void SwipeStartCheck(Vector2 position, float time)
    {
        startPos = position;
        startTime = time;
    }

    //private void SwipeEndCheck(Vector2 position, float time)
    //{
    //    endPos = position;
    //    endTime = time;
    //    DetectSwipeCheck();

    //}


    private void SwipeStartShop(Vector2 position, float time)
    {
        startPos = position;
        startTime = time;
    }

    private void SwipeEndShop(Vector2 position, float time)
    {
        endPos = position;
        endTime = time;
        DetectSwipeShop();
    }

    private void SwipeStart(Vector2 position, float time)
    {
        startPos = position;
        startTime = time;
    }

    private void SwipeEnd(Vector2 position, float time)
    {
        endPos = position;
        endTime = time;
        DetectSwipe();
    }

    private void DetectSwipe()
    {
        Debug.Log("swipeAnyWay");
        if (Vector3.Distance(startPos,endPos)>= minDisUpDown && (endTime-startTime)<= maxTime)
        {
            Debug.Log("swipe");
            Debug.DrawLine(startPos, endPos, Color.green, 5f);
            var direction = endPos - startPos;
            var direction2D = ((Vector2)direction).normalized;
            SwipeDirection(direction2D);
        }
    }

    private void DetectSwipeShop()
    {
        
        if (Vector3.Distance(startPos, endPos) >= minDisLR && (endTime - startTime) <= maxTime)
        {
            Debug.Log("swipe");
            Debug.DrawLine(startPos, endPos, Color.green, 5f);
            var direction = endPos - startPos;
            var direction2D = ((Vector2)direction).normalized;
            SwipeDirectionShop(direction2D);
        }
    }

    //private void DetectSwipeCheck()
    //{
    //    Debug.Log("swipeSwipeCheck");
    //    Debug.DrawLine(startPos, endPos, Color.green, 5f);
    //    if (Vector3.Distance(startPos, endPos) >= minDisLR && (endTime - startTime) <= maxTime)
    //    {
    //        Debug.Log("swipe");

    //        var direction = endPos - startPos;
    //        var direction2D = ((Vector2)direction).normalized;
    //        SwipeDirectionCheck(direction2D);


    //    }
    //}


    private void SwipeDirection(Vector2 direction)
    {
        if(Vector2.Dot(Vector2.up,direction)>dirThreshold)
        {
            Debug.Log("swipeUp");
            SwipeUp?.Invoke();
        }
        if (Vector2.Dot(Vector2.down, direction) > dirThreshold)
        {
            Debug.Log("swipeDown");
            SwipeDown?.Invoke();
        }
        if (Vector2.Dot(Vector2.left, direction) > dirThreshold)
        {
            Debug.Log("swipeleft");
            SwipeLeft?.Invoke();
        }
        if (Vector2.Dot(Vector2.right, direction) > dirThreshold)
        {
            Debug.Log("swiperight");
            SwipeRight?.Invoke();
        }
       
    }


    private void SwipeDirectionShop(Vector2 direction)
    {
        if (Vector2.Dot(Vector2.left, direction) > dirThreshold)
        {
            Debug.Log("swipeleft");
            SwipeLeftPage?.Invoke();
        }
        if (Vector2.Dot(Vector2.right, direction) > dirThreshold)
        {
            Debug.Log("swiperight");
            SwipeRightPage?.Invoke();
            
        }

    }


    //private void SwipeDirectionCheck(Vector2 direction)
    //{
    //    if (Vector2.Dot(Vector2.left, direction) > dirThreshold)
    //    {
    //        Debug.Log("swipeCheckCheckleft");
    //        SwipeLeftCheck?.Invoke();
    //    }
    //    if (Vector2.Dot(Vector2.right, direction) > dirThreshold)
    //    {
    //        Debug.Log("swipeCheckright");
    //        SwipeRightCheck?.Invoke();
    //    }

    //}
}

