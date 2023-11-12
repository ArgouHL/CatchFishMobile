using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[DefaultExecutionOrder(-1)]
public class MusicControl : MonoBehaviour
{
    public static MusicControl instance;
    [SerializeField] private MusicScriptableObj[] bGMList;
    Dictionary<bgmType, MusicScriptableObj> bgmDatabase = new Dictionary<bgmType, MusicScriptableObj>() { };
    [SerializeField]
    private Transform bGMPlayersParent;
    private AudioSource[] bGMPlayers;
    [SerializeField]
    public AudioMixer masterMixer;
    private MusicScriptableObj nowBGM;
    public MusicScriptableObj NowBGM { get { return nowBGM; } }
    private int nowBGMIndex = 0;
    internal int nextBar;
    internal double startDspTime = 0;
    [SerializeField] private double offset;
    //private double nextTick = 9999999999999999d; // The next tick in dspTime
    //private double nextBarTick = 9999999999999999d;
    //private double preHalfTick = 0.0F;
    private double sampleRate = 0.0F;
    //private bool ticked = true;
    //private bool barTicked = true;
    //private bool halfBarTicked = true;
    //private bool battleBGMPlaying = false;
    //private bool songstarted = false;
    private Coroutine scheduleBGM;
    //private double timePerTick;
    //private double timePerBar;
    public static bool gameBGMPlaying=false;

    
    public delegate void BeatEvent();
    public static BeatEvent Onbeat;
    public static BeatEvent OnHalfBar;
    public static BeatEvent OnBar;
    public void Awake()
    {
        if (instance != null)
            Destroy(this.gameObject);
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        foreach (var bgm in bGMList)
        {
            bgmDatabase.Add(bgm.type, bgm);
        }
    }

    private void OnEnable()
    {
        //ScreenLatterboxManager.onLatterboxingStart += BGMFadeOut;
        //ScreenLatterboxManager.onLatterboxingOut += BGMFadeIn;
    }

    private void OnDisable()
    {
        //ScreenLatterboxManager.onLatterboxingStart -= BGMFadeOut;
        //ScreenLatterboxManager.onLatterboxingOut -= BGMFadeIn;
    }


    private void Start()
    {
        bGMPlayers = bGMPlayersParent.GetComponentsInChildren<AudioSource>();
    }


    private void Update()
    {
   
        //while (AudioSettings.dspTime >= nextBarTick)
        //{

        //    barTicked = false;
        //    nextBarTick += timePerBar;

        //}
    }

 

  

    public void BGMFadeOut(int index, float duration = 0.5f)
    {
        
        if (bGMPlayers[index].clip == null)
            return;
        var nowVo = bGMPlayers[index].volume;
        LeanTween.value(1f, 0f, duration).setOnUpdate((float val) => BGMFadeUpdate(val, index));
    }

    public void BGMFadeOut(float time = 1,float targetVolume =0.6f)
    {
        float v = 0;
        masterMixer.GetFloat("BGBVolume", out v);
        if (v < 0f)
            return;
        LeanTween.value(1f, targetVolume, time).setOnUpdate((float val) => BGMFadeUpdate(val));
    }

    public void BGMFadeIn(float time = 1)
    {
        Debug.Log("BGMFadeIn");
        float v = 0;
        masterMixer.GetFloat("BgmVolume", out v);
        float val = Mathf.Lerp(1,0, v / -80f);
        LeanTween.value(val, 1f, time).setOnUpdate((float value) => BGMFadeUpdate(value));
    }

    public void SetBGMVolume(float val)
    {
        Debug.Log("SetBGMVolume:" + val);
        BGMFadeUpdate(val);
    }
    private void BGMFadeUpdate(float v)
    {
        masterMixer.SetFloat("BgmVolume", Mathf.Lerp(-80,0,v/1));
    }


    private void BGMFadeUpdate(float v, int i)
    {
        bGMPlayers[i].volume = v;
    }


    public void PlayBGM(bgmType type)
    {

        Debug.Log("PlayBGM" + type);
        StopBGM();

        BGMFadeOut(nowBGMIndex);
        nowBGMIndex += 1;
        nowBGMIndex %= 2;
        startDspTime = AudioSettings.dspTime;

        nowBGM = bgmDatabase[type];


        //timePerTick = 60.0f / nowBGM.bpm;
        //timePerBar = MusicCounter.GetDurationPerBar(nowBGM);

        SetBGMVolume(0.9f);
        bGMPlayers[nowBGMIndex].volume = 1;
        bGMPlayers[nowBGMIndex].clip = nowBGM.bgm;
        bGMPlayers[nowBGMIndex].Play();

        //sampleRate = AudioSettings.outputSampleRate;
        print("setnexttick");

        scheduleBGM = StartCoroutine(NextBGM());
    }



    public void PlayBGMFadeIn(bgmType type)
    {
        Debug.Log("PlayBGM" + type);
        SoftStopBGM();
        BGMFadeOut(nowBGMIndex);
        nowBGMIndex += 1;
        nowBGMIndex %= 2;
        startDspTime = AudioSettings.dspTime;
        BGMFadeIn(nowBGMIndex);
        nowBGM = bgmDatabase[type];


        //timePerTick = 60.0f / nowBGM.bpm;
        //timePerBar = MusicCounter.GetDurationPerBar(nowBGM);
        LeanTween.value(0, 0.9f, 3f).setOnUpdate((float val) => BGMFadeUpdate(val));
        bGMPlayers[nowBGMIndex].volume = 1;
        bGMPlayers[nowBGMIndex].clip = nowBGM.bgm;
        bGMPlayers[nowBGMIndex].Play();

        //sampleRate = AudioSettings.outputSampleRate;
        print("setnexttick");

        scheduleBGM = StartCoroutine(NextBGM());
    }




    private void RePlayBGM()
    {

        startDspTime = AudioSettings.dspTime;
        nowBGMIndex += 1;
        nowBGMIndex %= 2;

        bGMPlayers[nowBGMIndex].volume = 1;
        bGMPlayers[nowBGMIndex].clip = nowBGM.bgm;
        bGMPlayers[nowBGMIndex].Play();
        print("RePlayBattleBGM");
        scheduleBGM = StartCoroutine(NextBGM());
    }



    public void StopBGM(float duration = 0.5f)
    {
        Debug.Log("StopBGM");
        if (scheduleBGM != null)
            StopCoroutine(scheduleBGM);
        foreach (var p in bGMPlayers)
        { p.Stop(); }

    }

    public void SoftStopBGM(float duration = 0.5f)
    {
        Debug.Log("SoftStopBGM");
        if (scheduleBGM != null)
            StopCoroutine(scheduleBGM);
        BGMFadeOut(nowBGMIndex, duration);
        
    }

    public void StopAllBGM(float duration = 0.5f)
    {
        Debug.Log("StopBGM");
        if (scheduleBGM != null)
            StopCoroutine(scheduleBGM);
        BGMFadeOut(nowBGMIndex, duration);
        BGMFadeOut((nowBGMIndex+1)%2, duration);
    }
    private IEnumerator NextBGM()
    {
        double playTime = MusicCounter.GetDuration(nowBGM);

        double currentDspTime = AudioSettings.dspTime;

        
        yield return new WaitUntil(() => AudioSettings.dspTime - currentDspTime+ offset >= playTime);

        RePlayBGM();

    }

   


 

   


    internal float TimePerBar()
    {
        return (float)MusicCounter.GetDurationPerBar(nowBGM);
    }

}

public enum bgmType { title,Lobby,Sea1}
