using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(menuName = "Music/Creat new music Obj")]
public class MusicScriptableObj : ScriptableObject
{
    public AudioClip bgm;
    public double offset;
    public int bars;

    public double bpm;
    public BeatType beatType;
    public double duration { get { return MusicCounter.GetDuration(this); } }
    public double durationPerBar { get { return MusicCounter.GetDurationPerBar(this); } }
    public int beatPerBar { get { return MusicCounter.GetBeatPerBar(this); } }

    public double Offset()
    {
        var _offset = offset;
#if UNITY_ANDRIOD
       offset+=0.01d;

#endif
        return _offset;
    }

    public bgmType type;


    public enum BeatType { twoFour, threeFour, fourFour, sixEight }
}

public static class MusicCounter
{



    public static double GetDuration(MusicScriptableObj targerBGM)
    {

        return GetDurationPerBar(targerBGM) * targerBGM.bars;
    }

    public static double GetDurationPerBar(MusicScriptableObj targerBGM)
    {
        double _d = 0;

        switch (targerBGM.beatType)
        {
            case MusicScriptableObj.BeatType.twoFour:
                // bpm * 
                _d = 60d / targerBGM.bpm * 2;
                break;
            case MusicScriptableObj.BeatType.threeFour:
                _d = 60d / targerBGM.bpm * 3;
                break;
            case MusicScriptableObj.BeatType.fourFour:
                _d = 60d / targerBGM.bpm * 4;
                break;
            case MusicScriptableObj.BeatType.sixEight:
                _d = 60d / targerBGM.bpm * 6;
                break;

        }
        return _d;
    }


    public static int GetBeatPerBar(MusicScriptableObj targerBGM)
    {
        int _d = 0;

        switch (targerBGM.beatType)
        {
            case MusicScriptableObj.BeatType.twoFour:
                // bpm * 
                _d = 2;
                break;
            case MusicScriptableObj.BeatType.threeFour:
                _d = 3;
                break;
            case MusicScriptableObj.BeatType.fourFour:
                _d = 4;
                break;
            case MusicScriptableObj.BeatType.sixEight:
                _d = 6;
                break;

        }
        return _d;
    }
}
