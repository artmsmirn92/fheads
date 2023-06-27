using System;
using UnityEngine;
using System.Collections.Generic;
using mazing.common.Runtime.Exceptions;


[System.Serializable]
public class Tribunes
{
    public enum Weather { 
        clear,
        rain,
        snow,
        sun,
        rain_and_snow 
    };

    public Weather _weather;
    public Sprite spr_tribunes;
}

public class SkyScript : MonoBehaviour
{
    public Scripts scr;
    [Header("Stadiums and weather:")]
    public List<Tribunes> _tribunes;
    public GameObject bannerGroup1, bannerGroup2;
    public GameObject obj_Splashes;
    public Animator bannersAnim;
    public bool isGoal;
    public bool isRandTribunes;
    public SpriteRenderer[] wallSprs;
    public SpriteRenderer stadiumSprR;
    // public Sprite[] tribunesSpr;
    public Color[] randCol;
    public GameObject obj_Snow;

    private float tim;


    void Awake()
    {
        SetTribunes();
        SetWeather();
    }

    void Start()
    {
        PlayerPrefs.SetInt("MenuTrigger_1", 0);

        bool areTribunesPracrtice = scr.alPrScr.tribunes == 0;
        bannersAnim.enabled = !areTribunesPracrtice;
        bannerGroup1.SetActive(!areTribunesPracrtice);
        bannerGroup2.SetActive(!areTribunesPracrtice);
        obj_Splashes.SetActive(!areTribunesPracrtice);
    }

    void Update()
    {
        if (isGoal && scr.alPrScr.tribunes != 0)
        {
            string call_str = "call";
            int call_int = Animator.StringToHash(call_str);
            bannersAnim.ResetTrigger(call_int);
            bannersAnim.SetTrigger(call_int);
            isGoal = false;
        }
    }
        
    private void SetTribunes()
    {
        stadiumSprR.sprite = _tribunes[scr.alPrScr.tribunes].spr_tribunes;
    }

    public void SetWeather()
    {
        var weather = _tribunes[scr.alPrScr.tribunes]._weather;
        (bool rainActive, bool snowActive) = weather switch {
            Tribunes.Weather.clear         => (false, false),
            Tribunes.Weather.rain          => (true,  false),
            Tribunes.Weather.rain_and_snow => (true,  true),
            Tribunes.Weather.snow          => (false, true),
            Tribunes.Weather.sun           => (false, false),
        };
        scr.rainMan.isRainThisGame = rainActive;
        obj_Snow.SetActive(snowActive);
    }
}
