using UnityEngine;
using System.Collections;
using mazing.common.Runtime.Utils;
using UnityEngine.UI;


public class Objects_Menu : MonoBehaviour 
{
    #region constants

    private const string WordExit = "";
    private const string WordBack = "НАЗАД";

    #endregion

    #region serialized fields

    public Scripts scr;

    public Sprite 
        spr_SoundOn,
        spr_SoundOff;

    public Color
        col_Gray,
        col_Orange,
        col_Blue;

    [Header("Gameobject:")]
    public GameObject moneyCount;
    public GameObject sampleButton1;
	
    public GameObject cupAw;
    public GameObject currPrPan;
    public GameObject menuProfile;

    [Header("Other:")]
    public Image soundIm;
    public AudioSource   mainThemeSource;
    public AudioSource   buttonsSource;
    public RectTransform cP;
    public Text[]        text_opndPlayers;
    public string[]      idForTesting;
    public Text          text_ExitButton;
    public GameObject    obj_MainMenu;
    public Animator      anim_MainMenu;
    public GameObject    obj_MenuTournament;
    public Animator      anim_MenuTournament;
    public GameObject    obj_MenuPlayers;
    public Animator      anim_MenuPlayers;
    public GameObject    obj_MenuUpgrades;
    public Animator      anim_MenuUpgrades;
    public Image         controlsTutorialImage;


    #endregion

    #region engine methods

    private void Awake()
    {
        controlsTutorialImage.enabled = !CommonUtils.IsOnMobileWebGl();
        text_ExitButton.text = WordExit;
        scr.upgr.curr_ind = 0;
        scr.upgr.curr_indBall = 0;

#if !UNITY_EDITOR
        bool doNotDestroy = false;

        for (int i = 0; i < idForTesting.Length; i++)
        {
            if (Android_Id() == idForTesting[i])
                doNotDestroy = true;
        }
#endif

        currPrPan.SetActive (true);
        EnableSound(true);
    }
    
    #endregion

    #region api

    public void PixelPerfect()
    {
        bool pixelPerfect = gameObject.GetComponent<Canvas>().pixelPerfect;
        gameObject.GetComponent<Canvas>().pixelPerfect = !pixelPerfect;
    }

    public void EnableSound(bool _IsStart)
    {
        int onInt = PlayerPrefs.GetInt("SoundOn");

        int onInt_1 = onInt == 0 ? 1 : 0;

        if (!_IsStart)
            PlayerPrefs.SetInt("SoundOn", onInt_1);

        onInt = PlayerPrefs.GetInt("SoundOn");
        mainThemeSource.mute = !CommonUtilsFheads.Int2Bool(onInt);
        buttonsSource.mute = !CommonUtilsFheads.Int2Bool(onInt);
        SoundImage(onInt);
    }
    
    public void OpenGameInMarket()
    {
        Application.OpenURL("market://details?id=" + Application.identifier);
        PlayerPrefs.SetInt("Review_Done", 1);
    }
    
    public string Android_Id()
    {
        string android_id;
#if !UNITY_EDITOR
        android_id = "editor";
#else
        AndroidJavaClass up = new AndroidJavaClass ("com.unity3d.player.UnityPlayer");
        AndroidJavaObject currentActivity = up.GetStatic<AndroidJavaObject> ("currentActivity");
        AndroidJavaObject contentResolver = currentActivity.Call<AndroidJavaObject> ("getContentResolver");  
        AndroidJavaClass secure = new AndroidJavaClass ("android.provider.Settings$Secure");
        android_id = secure.CallStatic<string> ("getString", contentResolver, "android_id");
#endif

        return android_id;
    }
    
    public void Menu_Tournaments(bool _IsOn)
    {
        if (_IsOn)
        {
            scr.gM.CurrentMenu = EMenu.MenuCareer;
            anim_MainMenu.SetTrigger(Animator.StringToHash("1"));
            obj_MenuTournament.SetActive(true);
        }
        text_ExitButton.text = _IsOn ? WordBack : WordExit;
        int animationTrigger = Animator.StringToHash(_IsOn ? "0" : "1");
        anim_MenuTournament.SetTrigger(animationTrigger);
    }

    public void Menu_Players(bool _IsOn)
    {
        if (_IsOn)
        {
            scr.gM.CurrentMenu = EMenu.MenuPlayers;
            anim_MainMenu.SetTrigger(Animator.StringToHash("1"));
            obj_MenuPlayers.SetActive(true);
        }
        text_ExitButton.text = _IsOn ? WordBack : WordExit;
        int animationTrigger = Animator.StringToHash(_IsOn ? "0" : "1");
        anim_MenuPlayers.SetTrigger(animationTrigger);
    }

    public void Menu_Upgrades(bool _IsOn)
    {
        if (_IsOn)
        {
            scr.gM.CurrentMenu = EMenu.MenuUpgrades;
            obj_MenuUpgrades.SetActive(true);
        }
        scr.upgr.Ball_Choose(scr.upgr.curr_indBall);
        scr.upgr.Upgrade_Choose(scr.upgr.curr_ind);
        text_ExitButton.text = _IsOn ? WordBack : WordExit;
        int animationTrigger = Animator.StringToHash(_IsOn ? "0" : "1");
        anim_MenuUpgrades.SetTrigger(animationTrigger);
    }

    public void Button_Sound()
    {
        if (!buttonsSource.mute && buttonsSource.enabled)
            buttonsSource.Play();
    }

    #endregion

    #region nonpublic methods

    private void SoundImage(int _IsOn)
    {
        soundIm.sprite = _IsOn == 0 ? spr_SoundOff : spr_SoundOn;
    }

    #endregion
}
