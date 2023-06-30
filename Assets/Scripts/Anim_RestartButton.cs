using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Anim_RestartButton : MonoBehaviour 
{
    [SerializeField]
    private Animator anim_RestBut;
    [SerializeField]
    private Text text_Restart;
    [SerializeField]
    private Text text_WatchVideo;


    public void SetRestartText(int _int)
    {
        switch (_int)
        {
            case 0:
                text_Restart.text = "РЕСТАРТ";
                text_WatchVideo.enabled = true;
                break;
            case 1:
                text_Restart.text = "РЕКЛАМА ЕЩЕ\nНЕ ЗАГРУЗИЛАСЬ";
                text_WatchVideo.enabled = false;
                break;
            case 2:
                text_Restart.text = "РЕКЛАМА \nНЕДОСТУПНА";
                text_WatchVideo.enabled = false;
                break;
        }
    }

    public void Set_Trigger(string _str)
    {
        anim_RestBut.SetTrigger(Animator.StringToHash(_str));
    }
}
