using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSUP : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

#if UNITY_ANDROID
        Application.targetFrameRate = 120;
#endif
    }


}