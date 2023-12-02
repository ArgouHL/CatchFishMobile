using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SfxControl : MonoBehaviour
{
    public static SfxControl instance;
    [SerializeField] private AudioSource buttonClick, coin, sad, rain, pass, fail;

    [SerializeField] private AudioSource hitSoundsPlayer;

    [SerializeField] private AudioClip[] hitSounds;
    [SerializeField] private AudioClip catchSound;
    [SerializeField] private AudioMixer mixer;


    private void Awake()
    {
        if (instance == null)
        {
            instance = GetComponent<SfxControl>();
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public void HitPlay(int index)
    {
      //  int index = hitSounds.Length-1 - count;
        hitSoundsPlayer.PlayOneShot(hitSounds[index]);
    }

    public void CatchPlay()
    {
        
        hitSoundsPlayer.PlayOneShot(catchSound);
    }

    //public void PlayClick()
    //{
    //    buttonClick.Play();
    //}

    //public void PlayCoin()
    //{
    //    coin.Play();
    //}
    //public void PlaySad()
    //{
    //    sad.Play();
    //}
    //public void PlayPass()
    //{
    //    pass.Play();
    //}
    //public void PlayFail()
    //{
    //    fail.Play();
    //}
    //public void StartRain()
    //{

    //    RainFadeIn();
    //    rain.Play();
    //}
    //public void StopRain()
    //{
    //    RainFadeOut();
    //}

    //public void RainFadeIn(float time = 2)
    //{


    //    LeanTween.value(0, 0.8f, time).setOnUpdate((float val) => RainFadeUpdate(val));
    //}
    //public void RainFadeOut(float time = 2)
    //{

    //    LeanTween.value(0.8f, 0, time).setOnUpdate((float val) => RainFadeUpdate(val));
    //}
    //private void RainFadeUpdate(float v)
    //{
    //    mixer.SetFloat("RainVolume", Mathf.Lerp(-80, 0, v / 1));
    //}



}


