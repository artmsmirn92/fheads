using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using Common.Managers.Advertising;
using Common.Utils;
using mazing.common.Runtime.Exceptions;
using mazing.common.Runtime.Managers.IAP;
using mazing.common.Runtime.Utils;
using YG;
using Zenject;

[System.Serializable]
public class CareerOpponentMain
{
    public EOpponentType oppType;
    public Names.PlayerName oppName;
}

[System.Serializable]
public class CareerOpponentLegend
{
    public EOpponentType oppType;
    public Names.PlayerName_2 oppName_2;

}

[System.Serializable]
public class CareerGame
{
    public EOpponentsNumAndAge oppsNumAndAge;
    public List<CareerOpponentMain> oppsMain;
    public List<CareerOpponentLegend> oppsLegend;
}

public class UniversalFunctions : MonoBehaviour
{
    #region inject
    
    [Inject] private IAdsManager  AdsManager  { get; set; }
    [Inject] private IShopManager ShopManager { get; set; }
    
    #endregion

    #region serialized fields

    public Scripts scr;

    #endregion

    #region engine methods

    private void Start()
    {
        YandexGame.RewardVideoEvent -= OnRewardedVideoShown;
        YandexGame.RewardVideoEvent += OnRewardedVideoShown;
    }

    #endregion

    #region api

    public void RestartLevel()
    {
        YandexGame.RewVideoShow(0);
    }
    
    public void ShowInterstitialAd()
    {
        AdsManager.ShowInterstitialAd();
    }

    public void ShowReviewGameDialogOnGame10()
    {
        int gameNum = PlayerPrefs.GetInt("GameNum");
        if (++gameNum == 10)
            ShopManager.RateGame();
        PlayerPrefs.SetInt("GameNum", gameNum);
    }
    
    #endregion
    
    #region nonpublic methods

    private void RestartLevelForAds()
    {
        if (AdsManager.RewardedAdReady)
            AdsManager.ShowRewardedAd(_OnReward: RestartLevelCore);
        else MazorCommonUtils.ShowAlertDialog("OOPS", "the ad didn't load. Try later...");
    }
    
    private static void OnRewardedVideoShown(int _Id)
    {
        if (_Id == 0)
            RestartLevelCore();
    }

    private static void RestartLevelCore()
    {
        SceneManager.LoadScene(2);
    }
    
    #endregion
}
