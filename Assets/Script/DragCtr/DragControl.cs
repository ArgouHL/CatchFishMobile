using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Policy;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class DragControl : MonoBehaviour
{
    public static DragControl instance;

    [SerializeField] private RectTransform startP, endP;
    //  [SerializeField] private LineRenderer dragLine;
    private Coroutine dragCoro;
    private Vector2 screenRect;
    [SerializeField] private Image startImage, aimImage, aimTarget;
    [SerializeField] private RectTransform targetLine, chargeBar;

    //[SerializeField] private Image[] backPoints;
    public delegate void DragEvent(float angle, float maxDis);
    public static DragEvent dragoff;
    [SerializeField] private Transform catTransform;
    private float draggedTime;
    [SerializeField] private float MaxDraggedTime = 3;
    [SerializeField] private ParticleSystem charging;
    private float wayAngle;
    private float disOnWorld;
    private Coroutine effCoro;
    private float coolDownTime = 0.5f;
    private bool coolingDown = false;
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
        screenRect = new Vector2(Screen.currentResolution.width, Screen.currentResolution.height);

#if UNITY_EDITOR
        screenRect = new Vector2(Camera.main.scaledPixelWidth, Camera.main.scaledPixelHeight);

#endif
    }
    private void StartDrag(InputAction.CallbackContext ctx)
    {
        //if (coolingDown)
        //    return;
        if (MarkerHit.instance.tracking)
            return;
        // Debug.Log(PlayerInputManager.inputs.GamePlay.TouchPosition.ReadValue<Vector2>());
        draggedTime = 0;
        Debug.Log("StartDrag");
        if (dragCoro != null)
            return;
        dragCoro = StartCoroutine(DragShowIE());
        CatCtr.instance.PreAtk();
        SfxControl.instance.PlayForcoing();
        coolingDown = true;
    }
    private void EndDrag(InputAction.CallbackContext ctx)
    {
        if (effCoro != null)
        {
            StopCoroutine(effCoro);
            effCoro = null;
        }
        else
            charging.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        //  Debug.Log("EndDrag");
        if (dragCoro == null)
            return;

        StopCoroutine(dragCoro);
        dragCoro = null;
        // dragLine.positionCount = 0;
        HideAim();
        SfxControl.instance.StopForcoing();
        if (disOnWorld < 2f)
        {
            CatCtr.instance.BackIdle();
            coolingDown = false;
            return;
        }

        dragoff.Invoke(wayAngle, disOnWorld);
        CatCtr.instance.Atk();


    }


    private IEnumerator DragShowIE()
    {
        effCoro = StartCoroutine(effIE());
        var sp = PlayerInputManager.inputs.GamePlay.TouchPosition.ReadValue<Vector2>();
        startP.anchoredPosition = sp;
        // dragLine.positionCount = 2;
        // dragLine.SetPosition(0, startP.position);

        ShowAim();
        while (true)
        {
            draggedTime += Time.deltaTime;
            var ep = PlayerInputManager.inputs.GamePlay.TouchPosition.ReadValue<Vector2>();
            endP.anchoredPosition = ep;
            //     dragLine.SetPosition(1, endP.position);
            Aiming(sp, ep);

            // Debug.Log("Dragging");
            yield return null;
        }
    }

    private void ShowAim()
    {
        print(screenRect);
        startImage.rectTransform.position = RectTransformUtility.WorldToScreenPoint(Camera.main, catTransform.position);
        startImage.color = Color.white;
        chargeBar.GetComponent<Image>().color = Color.white;
        //  aimTarget.color = new Color(1, 0.5f, 0.5f, 0.5f);
        //  aimImage.color = Color.white;
        foreach (var p in targetLine.GetComponentsInChildren<Image>())
        {
            p.color = Color.white;
        }
        //foreach (var p in backPoints)
        //{
        //    p.color = Color.white;
        //}
    }


    private void HideAim()
    {
        var transparent = new Color(1, 1, 1, 0);
        startImage.color = transparent;
        //   aimImage.color = transparent;
        //  aimTarget.color = transparent;
        chargeBar.GetComponent<Image>().color = transparent;
        foreach (var p in targetLine.GetComponentsInChildren<Image>())
        {
            p.color = transparent;
        }
        //foreach (var p in points)
        //{
        //    p.color = transparent;
        //}
        //foreach (var p in backPoints)
        //{
        //    p.color = transparent;
        //}
    }

    private void Aiming(Vector2 sp, Vector2 ep)
    {
        var maxDis = Mathf.Lerp(0, 2500, draggedTime / MaxDraggedTime);

        var _wayVector = sp - ep;
        //500>-500
        float angle = Mathf.Lerp(-80, 80, (_wayVector.x + 400) / 800);

        //Debug.Log(_wayVector);
        //_wayVector.x *= 0.2f;
        //_wayVector = _wayVector.normalized;






        // float angle = Mathf.Atan2(_wayVector.x, _wayVector.y)*Mathf.Rad2Deg;
        Debug.Log(angle);

        wayAngle = angle;
        var aimRotate = Quaternion.AngleAxis(wayAngle + 180, -Vector3.forward);
        targetLine.rotation = aimRotate;
        chargeBar.rotation = aimRotate;
        var vector = _wayVector * maxDis;
        chargeBar.sizeDelta = new Vector2(chargeBar.sizeDelta.x, maxDis);

        disOnWorld = maxDis * (Camera.main.orthographicSize * 2) / screenRect.y;

        //   aimTarget.rectTransform.anchoredPosition = vector + startImage.rectTransform.anchoredPosition;

        //

        //if (aimPos.x > 660)
        //    aimPos.x = 660;
        //else if (aimPos.x < -660)
        //    aimPos.x = -660;
        //if (aimPos.y < -1460)
        //    aimPos.y = -1460;

        //aimTarget.rectTransform.anchoredPosition = aimPos;


        //aimImage.rectTransform.anchoredPosition = Vector2.ClampMagnitude((aimPos - startImage.rectTransform.anchoredPosition), maxDis) + startImage.rectTransform.anchoredPosition;


        //Vector2 back = (aimPos - startImage.rectTransform.anchoredPosition) * -0.5f + startImage.rectTransform.anchoredPosition;
        //for (int i = 0; i < points.Length; i++)
        //{
        //    points[i].rectTransform.anchoredPosition = Vector2.Lerp(aimImage.rectTransform.anchoredPosition, startImage.rectTransform.anchoredPosition, ((float)i + 1) / (float)points.Length);
        //}
        //for (int i = 0; i < backPoints.Length; i++)
        //{
        //    backPoints[i].rectTransform.anchoredPosition = Vector2.Lerp(back, startImage.rectTransform.anchoredPosition, ((float)i + 1) / (float)backPoints.Length);
        //}
    }



    private IEnumerator effIE()
    {
        yield return new WaitForSeconds(0.15f);
        charging.Play();
        effCoro = null;
    }

    public void CoolDownReset()
    {
        coolingDown = false; ;
    }
    public void CoolDown()
    {
        LeanTween.delayedCall(coolDownTime, () => coolingDown = false);
    }
}
