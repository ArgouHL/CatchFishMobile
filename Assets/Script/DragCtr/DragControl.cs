using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class DragControl : MonoBehaviour
{
    public static DragControl instance;

  [SerializeField] private RectTransform startP, endP;
    [SerializeField] private LineRenderer dragLine;
    private Coroutine dragCoro;
    private Vector2 ScreenCenter;
    [SerializeField] private Image startImage, aimImage;
    [SerializeField] private Image[] points;

    public delegate void DragEvent(Vector3 v3);
    public static DragEvent dragoff;


    private void Awake()
    {
        instance = this;
        PlayerInputManager.instance.ChangeType(InputType.GamePlay);
    }

    private void OnEnable()
    {
        PlayerInputManager.inputs.GamePlay.Touch.started += StartDrag;
        PlayerInputManager.inputs.GamePlay.Touch.canceled += EndDrag;

    }
    private void OnDisable()
    {
        PlayerInputManager.inputs.GamePlay.Touch.started -= StartDrag;
        PlayerInputManager.inputs.GamePlay.Touch.canceled -= EndDrag;
    }
    private void Start()
    {
        ScreenCenter = new Vector2(Screen.currentResolution.width / 2, Screen.currentResolution.height / 2);
    }
    private void StartDrag(InputAction.CallbackContext ctx)
    {
       // Debug.Log(PlayerInputManager.inputs.GamePlay.TouchPosition.ReadValue<Vector2>());
        Debug.Log("StartDrag");
        if (dragCoro != null)
            return;
        dragCoro = StartCoroutine(DragShowIE());

    }
    private void EndDrag(InputAction.CallbackContext ctx)
    {
        Debug.Log("EndDrag");
        if (dragCoro == null)
            return;
        StopCoroutine(dragCoro);
        dragCoro = null;
        dragLine.positionCount = 0;
        HideAim();
        dragoff.Invoke(aimImage.transform.position);
    }


    private IEnumerator DragShowIE()
    {

        var sp= PlayerInputManager.inputs.GamePlay.TouchPosition.ReadValue<Vector2>();
        startP.anchoredPosition = sp;  
        dragLine.positionCount = 2;
        dragLine.SetPosition(0, startP.position);

        ShowAim();
        while (true)
        {
            var ep = PlayerInputManager.inputs.GamePlay.TouchPosition.ReadValue<Vector2>();
            endP.anchoredPosition = ep;
            dragLine.SetPosition(1, endP.position);
            Aiming(sp, ep);

            Debug.Log("Dragging");
            yield return null;
        }
    }

    private void ShowAim()
    {
        startImage.color = Color.white;
        aimImage.color = Color.white;
        foreach(var p in points)
        {
            p.color = Color.white;
        }
    }


    private void HideAim()
    {
        var transparent = new Color(1, 1, 1, 0);
        startImage.color = transparent;
        aimImage.color = transparent;
        foreach (var p in points)
        {
            p.color = transparent;
        }
    }

    private void Aiming(Vector2 sp, Vector2 ep)
    {

        aimImage.rectTransform.anchoredPosition=(sp- ep).normalized*Vector2.Distance(sp , ep)*1.3f + startImage.rectTransform.anchoredPosition;
        for(int i=0; i < points.Length; i++)
        {
            points[i].rectTransform.anchoredPosition = Vector2.Lerp(aimImage.rectTransform.anchoredPosition, startImage.rectTransform.anchoredPosition, ((float)i + 1) / (float)points.Length);
        }

    }




}
