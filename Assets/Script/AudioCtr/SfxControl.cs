using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SfxControl : MonoBehaviour
{
    public static SfxControl instance;
    [SerializeField] private AudioClip buttonClick, water, catMeow, award, struggle, openShop,thunder,money,fishJump,normalForce,MagicForce,boxOpen;

    [SerializeField] private AudioSource hitSoundsPlayer;
    [SerializeField] private AudioSource forceSoundsPlayer;
    [SerializeField] private AudioSource stuggleSoundsPlayer;
    [SerializeField] private AudioClip[] hitSounds;
    [SerializeField] private AudioClip catchSound;
    [SerializeField] private AudioMixer mixer;


    private void Awake()
    {
  
            instance = GetComponent<SfxControl>();

    }

    public void HitPlay(int index)
    {
        //  int index = hitSounds.Length-1 - count;
        SfxPlayOneShot(hitSounds[index]);
    }

    public void CatchPlay()
    {
        
        SfxPlayOneShot(catchSound);
    }


    public void ClickPlay()
    {

       SfxPlayOneShot(buttonClick);
    }

    public void SfxPlayOneShot(AudioClip clip)
    {
        if (clip == null)
            return;
        hitSoundsPlayer.PlayOneShot(clip);
    }
    public void WaterPlay()
    {

        SfxPlayOneShot(water);
    }


    public void CatSoundPlay()
    {

        SfxPlayOneShot(catMeow);
    }

    public void AwardPlay()
    {

        SfxPlayOneShot(award);
    }

    public void StrugglePlay()
    {
        stuggleSoundsPlayer.clip = struggle;
        stuggleSoundsPlayer.Play();

    }
    public void StruggleStop()
    {
        stuggleSoundsPlayer.Stop();

    }
    public void OpenShopPlay()
    {

        SfxPlayOneShot(openShop);
    }

  

    public void ThunderPlay()
    {

        SfxPlayOneShot(thunder);
    }

   
    public void FishJumpPlay()
    {

        SfxPlayOneShot(fishJump);
    }

    internal void CoinPlay()
    {
        SfxPlayOneShot(money);
    }

    internal void BoxOpenPlay()
    {
        SfxPlayOneShot(boxOpen);
    }

    public void PlayForcoing()
    {
        forceSoundsPlayer.Stop();
        switch (SkinController.instance.GetSkin(PlayerDataControl.instance.playerData.currentSkin).skinType)
        {
            case SkinType.Normal:
            default:
                forceSoundsPlayer.clip = normalForce;
                break;
            case SkinType.Magic:
                forceSoundsPlayer.clip = MagicForce;
                break;
        }
        forceSoundsPlayer.Play();
    }

    internal void StopForcoing()
    {
        forceSoundsPlayer.Stop();
    }

    void StopAllSound()
    {
        StopForcoing();
        StruggleStop();
    }
}


