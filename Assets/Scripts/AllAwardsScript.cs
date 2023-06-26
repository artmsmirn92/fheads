﻿using UnityEngine;
using UnityEngine.UI;


public class AllAwardsScript : MonoBehaviour 
{
    public Scripts scr;
    [Space(5)]
    public Animator anim_AllAwPan;
    public GameObject obj_Ramka;
    public Image im_lang;
    public GameObject allAwPan;
    public GameObject[] awPans;
    public Text aw0CountText;

    [Header("Preview Objects:")]
    public GameObject yesButObj;
    public Text noButTxt;
    public Text prevText;

    private int[] pansActive = new int[3];


    void Awake()
    {
        for (int i = 0; i < pansActive.Length; i++)
            pansActive[i] = 0;

        obj_Ramka.SetActive(true);

        for (int i = 0; i < awPans.Length; i++)
            awPans[i].SetActive(false);
    }

    public void CloseAllAwardsPanel(int awPan)
    {
        //obj_Ramka.SetActive(true);
        int int_close = 0;

        for (int i = 0; i < pansActive.Length; i++)
        {
            if (pansActive[i] == 1)
                int_close++;
        }

        if (int_close > 1)
            awPans[awPan].SetActive(false);
        else
            anim_AllAwPan.SetTrigger(Animator.StringToHash("0"));

        pansActive[awPan] = 0;
    }

    public void CloseAllAwardsPanel_0()
    {
        for (int i = 0; i < awPans.Length; i++)
            awPans[i].SetActive(false);

        allAwPan.SetActive(false);
    }

    /// <summary>
    /// Everyday Reward
    /// </summary>
    public void CallAwardPanel_1()
    {
        allAwPan.SetActive(true);
        awPans[0].SetActive(true);
        pansActive[0] = 1;
        //aw1MainText.alignment = TextAnchor.LowerCenter;
        aw0CountText.enabled = true;
        aw0CountText.text = scr.univFunc.MoneyString (scr.everyDayReward.reward);
    }

    /// <summary>
    /// Preview message.
    /// </summary>
    public void CallAwardPanel_2()
    {
        allAwPan.SetActive(true);
        awPans[1].SetActive(true);
        pansActive[1] = 1;
        if (scr.gM._menues != Menues.menuCareer) 
            return;
        if (scr.alPrScr.moneyCount >= scr.carMng.lg_cost[scr.carMng._lgPrev])
        {
            yesButObj.SetActive(true);
            prevText.text = "Вы хотите открыть эту лигу за " 
                            + scr.univFunc.MoneyString(scr.carMng.lg_cost[scr.carMng._lgPrev]) 
                            + "?";
        }
        else
        {
            yesButObj.SetActive(false);
            prevText.text = "У Вас недостаточно денег, чтобы открыть эту лигу. "
                            + " "
                            + "Вам нужно " 
                            + scr.univFunc.MoneyString(scr.carMng.lg_cost[scr.carMng._lgPrev]);
        }
    }
    
    public void YesButton_Preview()
    {
        switch (scr.gM._menues)
        {
            case Menues.menuPlayers:
                scr.prMng.Unlock();
                scr.prMng.SetSkillsAndSprite();
                scr.prMng.SetShowcase();
                scr.allAw.CloseAllAwardsPanel(1);
                break;
            case Menues.menuCareer:
                scr.carMng.UnlockLeague(scr.carMng._lgPrev);
                scr.allAw.CloseAllAwardsPanel(1);
                break;
        }
    }
}
