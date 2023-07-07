using UnityEngine;
using UnityEngine.UI;

public class TimeFreeze : MonoBehaviour
{
    #region serialized fields

    public Scripts    scr;
    public float      freezeTime;
    public float      frTimeScale;
    public Text       text_FreezeTime;
    public GameObject obj_getSlowdownButton;
    public GameObject obj_TimeFreezeButton;
    public Image      sprRend_TimFrButton;
    public Sprite     spr_TimFr_0, spr_TimFr_1;
    public bool       isHandleUnlim;
    public bool timFr;

    #endregion

    #region nonpublic members

    private bool isStopFreezeTime;
    private int  freezeCount;
    private bool isNoFreezes;
    private int  freezeTime_2;
    private int  checkFreezeTime_2;
    private bool isTextDisabled;

    #endregion

    #region api

    public bool IsFreezeActive { get; set; }
    
    public void TimeFreeze_StartOrStop()
    {
        if (IsFreezeActive) TimeFreeze_Stop();
        else                TimeFreeze_Start(); 
    }

    #endregion

    #region engine methods
    
    private void Awake()
    {
        freezeTime = (freezeTime + (float)scr.alPrScr.upgrSlowdown) * frTimeScale;
        timFr = CommonUtilsFheads.Int2Bool(PlayerPrefs.GetInt("UnlimFreeze"));
        if (!timFr) freezeCount = 1;
        if (timFr || isHandleUnlim) EnableUnlimitedFreeze();

        float freezeTime1 = freezeTime / frTimeScale;
        text_FreezeTime.enabled = true;
        text_FreezeTime.text = freezeTime1.ToString("N1") + "c";
    }

    private void Update()
    {
        if (IsFreezeActive)
        {
            freezeTime = !timFr ? freezeTime - Time.deltaTime : freezeTime;

            float freezeTime1 = Time.timeScale > 0f ?
                freezeTime / Time.timeScale : freezeTime / scr.gM.CurrTimeScale;

            freezeTime_2 = Mathf.RoundToInt(freezeTime1 * 10f);

            if (freezeTime_2 != checkFreezeTime_2)
                text_FreezeTime.text = freezeTime1.ToString("N1") + "c";    

            if (freezeTime <= 0)
            {
                text_FreezeTime.text = "0.0c";
                isStopFreezeTime = true;
                IsFreezeActive = false;   

                if (isTextDisabled)
                    GetComponent<TimeFreeze>().enabled = false;

                isTextDisabled = true;
            }
        }

        checkFreezeTime_2 = freezeTime_2;

        if (!isStopFreezeTime) return;
        TimeFreeze_Stop();
        freezeCount--;

        if (freezeCount == 0)
        {
            isNoFreezes = true;
            
            sprRend_TimFrButton.enabled = false;
        }

        isStopFreezeTime = false;
    }


    #endregion
    
    #region nonpublic methods
    
    private void TimeFreeze_Start()
    {
        if (isNoFreezes) 
            return;
        sprRend_TimFrButton.sprite = spr_TimFr_1;
        IsFreezeActive             = true;
        Time.fixedDeltaTime        = 0.01f * frTimeScale;
        Time.timeScale             = frTimeScale;

        scr.levAudScr.timeSlow_In.Play();

        if (scr.levAudScr.timeSlow_Out.isPlaying)
            scr.levAudScr.timeSlow_Out.Stop();
    }

    private void TimeFreeze_Stop()
    {
        sprRend_TimFrButton.sprite = spr_TimFr_0;
        IsFreezeActive = false;
        Time.fixedDeltaTime = 0.01f;
        Time.timeScale = 1f;

        scr.levAudScr.timeSlow_Out.Play();

        if (scr.levAudScr.timeSlow_In.isPlaying)
            scr.levAudScr.timeSlow_In.Stop();
    }

    private void EnableUnlimitedFreeze()
    {
        timFr = true;
        obj_getSlowdownButton.SetActive(false);
    }

    #endregion
}
