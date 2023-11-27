using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEditor;
using System;

[DefaultExecutionOrder(-1)]
public class FitScreenScale : MonoBehaviour
{
	private CinemachineVirtualCamera vCam;	
	private PolygonCollider2D polygonCollider2D;

	[Range(0,1)]
	[SerializeField] private int widthOrHeight;
	void Start()
	{
		Initialized();
		FitScreen();
	}

    private void Initialized()
    {
		vCam = GetComponent<CinemachineVirtualCamera>();
		polygonCollider2D = (PolygonCollider2D)GetComponent<CinemachineConfiner>().m_BoundingShape2D;
	}

   

	private void FitScreen()
    {
		Vector2 screenRatio;
		screenRatio = new Vector2(Screen.currentResolution.width, Screen.currentResolution.height);
#if UNITY_EDITOR
		screenRatio = new Vector2(Camera.main.scaledPixelWidth, Camera.main.scaledPixelHeight);

#endif
		float ratio;
		float _widthHeight;
		ratio = (float)screenRatio.y / screenRatio.x;
		if (widthOrHeight==0)
		{
		
			_widthHeight = Mathf.Abs(polygonCollider2D.points[0].x - polygonCollider2D.points[3].x);
			
		}
		else
		{
			
			_widthHeight = Mathf.Abs(polygonCollider2D.points[0].y - polygonCollider2D.points[1].y)/ ratio;

		}
		Debug.Log(screenRatio);
		Debug.Log(_widthHeight);
		vCam.m_Lens.OrthographicSize = (float)_widthHeight * ratio/2;

	}

	[ContextMenu("FitScreen")]
	public void FitScreen_1()
    {
		Initialized();
		FitScreen();

	}

	//public static EditorWindow GetMainGameView()
	//{
	//	var assembly = typeof(EditorWindow).Assembly;
	//	var type = assembly.GetType("UnityEditor.GameView");
	//	var gameview = EditorWindow.GetWindow(type);
	//	return gameview;
	//}
}
