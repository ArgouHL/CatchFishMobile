using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
public class VolumeCtr : MonoBehaviour
{

    public static VolumeCtr instance;
    private float totalVolume=10;
    private float bgmVolume = 10;
    private float efxVolume = 10;
    [SerializeField] private AudioMixer masterMixer;
    private CanvasGroup volumeUI;



    public void Awake()
    {
        instance = this;
    }   

    public void totalVolumeChangr(float value)
    {
        totalVolume = value;
        Debug.Log(value);
        //totalVolumeP.text = (int)totalVolume + "%";
        masterMixer.SetFloat("MasterVolume", SetVolume(totalVolume));
    }
    public void BgmVolumeChangr(float value)
    {
        bgmVolume = value;
        //totalVolumeP.text = (int)totalVolume + "%";
        masterMixer.SetFloat("BgmVolume", SetVolume(bgmVolume));
    }
    public void EfxVolumeChangr(float value)
    {
        efxVolume = value;
        
        //totalVolumeP.text = (int)totalVolume + "%";
        masterMixer.SetFloat("EfxVolume", SetVolume(efxVolume));
    }
    private float SetVolume(float v)
    {
        if (v <= 0)
            return -80;
        Debug.Log(Mathf.Log10(v / 10) * 40);
        return Mathf.Log10(v / 10) * 40;
    }


}

public enum mixer { main,bgm,efx}
