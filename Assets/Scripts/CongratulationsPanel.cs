﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CongratulationsPanel : MonoBehaviour 
{
    [SerializeField]
	private Scripts scr;
    [Space(5)]
    public Animator anim_CongrPan;
    public GameObject obj_Conffeti;
	public GameObject congrPanel;
	public Rigidbody2D ballRb;
	public Text scoreText;
	public GameObject[] objsToDis;

   
	public void ShopPanel()
	{
        if (TimeManager.resOfGame == 1)
        {
            scr.alPrScr.winsTotal++;

            if (Score.EnemyScore == 0)
                scr.alPrScr.winsNoConcGoals++;
        }
            
		scoreText.text = Score.EnemyScore + ":" + Score.PlayerScore;
		congrPanel.SetActive(true);
        Enemy.gameStop = true;
		scr.gM.MenuResultBack ();

        scr.camSize.SetCameraPositionForCongratulationPanel();
        scr.alPrScr.doCh = true;
	}

	public void DisableSomeObjects()
	{
		foreach (var obj in objsToDis)
			obj.SetActive(false);
		scr.camSize.enabled = false;
	}
}
