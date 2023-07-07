using UnityEngine;
using System.Collections;
using RMAZOR.Helpers;
using UnityEngine.UI;

public class TopPanelManager : MonoBehaviour 
{
    public Scripts scr;

    public Text moneyText;
    private int maxMoneyCount = 100000000;
    private int moneyCountPrev;


    void Awake()
    {
        moneyText.text = FhUtils.MoneyString(scr.alPrScr.moneyCount);
    }

    void Update()
    {
        if (scr.alPrScr.moneyCount > maxMoneyCount)
        {
            scr.alPrScr.moneyCount = maxMoneyCount;
            scr.alPrScr.setMoney = true;
        }

        if (scr.alPrScr.moneyCount != moneyCountPrev)
            moneyText.text = FhUtils.MoneyString(scr.alPrScr.moneyCount);

        moneyCountPrev = scr.alPrScr.moneyCount;
    }
}
