using UnityEngine;
using UnityEngine.SceneManagement;
using Lean.Common;
using mazing.common.Runtime.Exceptions;
using YG;

public class GameManager : MonoBehaviour 
{
	#region serialized fields

	public Scripts scr;
	public bool
		pauseInLevel,
		gamePaused;
	
	#endregion

	#region nonpublic members

	private bool m_FirstCallProfPl;
	private int  m_Timer;
	
	private bool IsLevelScene => SceneManager.GetActiveScene().buildIndex == 2;

	#endregion

	#region engine methods

	private void Awake()
	{
		switch (SceneManager.GetActiveScene().buildIndex) 
		{
			case 1:
				PlayerPrefs.SetInt("CanRestart", 2);
				scr.allAw.allAwPan.SetActive(false);
				scr.alPrScr.game       = 0;
				scr.alPrScr.doCh       = true;
				scr.buf.isPractice     = false;
				scr.alPrScr.isRandGame = 0;
				Time.timeScale         = 1f;
				CurrentMenu            = EMenu.MainMenu;
				DestroyImmediate (GameObject.Find ("ChampList"));
				DestroyImmediate (GameObject.Find ("ChampListImage"));
				break;
			case 2:
				break;
		}
		scr.alPrScr.doCh = true;
	}

	private void Update()
	{
		if (SceneManager.GetActiveScene().buildIndex == 1)
			UpdateMenu();
		else if (SceneManager.GetActiveScene().buildIndex == 2)
			UpdateLevel();
	}

	#endregion

	#region api
	
	public EMenu CurrentMenu   { get; set; } = EMenu.MainMenu;
	public float CurrTimeScale { get; set; }

	public void StartGame()
	{
		Time.timeScale     = 1f;
		scr.pMov.startGame = true;
		scr.objLev.startPanelAnim.gameObject.SetActive(false);
	}
	
	public void DependentMenuBack() 
	{
        switch (CurrentMenu)
        {
            case EMenu.MainMenu:                                 break;
            case EMenu.MenuCareer:   scr.objM.Menu_Tournaments(false); break;
            case EMenu.MenuPlayers:  scr.objM.Menu_Players(false);     break;
            case EMenu.MenuUpgrades: scr.objM.Menu_Upgrades(false);    break;
            default:                 throw new SwitchCaseNotImplementedException(CurrentMenu);
        }

        CurrentMenu = EMenu.MainMenu;
	}

    public void MenuProfilePlayers()
    {
	    if (!m_FirstCallProfPl)
        {
	        scr.objM.cP.anchoredPosition = Vector2.up * scr.objM.cP.anchoredPosition.y;
            m_FirstCallProfPl = true;
        }
	    scr.objM.currPrPan.SetActive(false);
        scr.prMng.SetOpenedPlayersCountryText(false);
        CurrentMenu = EMenu.MenuPlayers;
    }

	public void GoToMenu()
	{
        scr.buf.is1stPractice = false;
        SceneManager.LoadScene("Menu");
        scr.alPrScr.doCh = true;
	}

	public void WinGame1()
	{
		if (!IsLevelScene) 
			return;
		scr.tM.time0 = 2;
		Score.PlayerScore  = 6;
		Score.EnemyScore = 0;
		scr.scoreScr.SetScore();
	}

	public void LooseGame()
	{
		if (!IsLevelScene) 
			return;
		Score.PlayerScore  = 0;
		Score.EnemyScore = 3;
		scr.tM.time0 = 2;
		scr.scoreScr.SetScore();
	}

	public void TieGame()
	{
		if (!IsLevelScene) 
			return;
		Score.PlayerScore  = 1;
		Score.EnemyScore = 1;
		scr.tM.time0 = 2;
		scr.scoreScr.SetScore();
	}

	public void SetStadium()
	{
        scr.alPrScr.stadium = scr.alPrScr.isRandGame == 1 ? 
            Mathf.FloorToInt(Random.value * (18 - 0.01f)) : 
            CommonUtilsFheads.Stadium(scr.alPrScr.game);
        scr.alPrScr.tribunes = scr.alPrScr.isRandGame == 0 ? 
            scr.alPrScr.lg : Mathf.FloorToInt(1f + (5 - 0.1f) * Random.value);
        scr.alPrScr.doCh = true;
	}
        
	public static void LoadSimpleLevel()
	{
		SceneManager.LoadScene(SceneNames.Level);
	}
        
	public void LoadRandomLevel()
	{
        scr.alPrScr.isRandGame = 1;
        SetStadium();
        scr.buf.SetRandomData();
		SceneManager.LoadScene(2);
	}
	
	public void Pause() 
	{
        System.GC.Collect();
        pauseInLevel = true;
        scr.objLev.pauseMenuAnim.gameObject.SetActive(true);
        scr.objLev.pauseMenuAnim.ResetTrigger(Animator.StringToHash("back"));
        scr.objLev.pauseMenuAnim.SetTrigger (Animator.StringToHash("call"));
        CurrTimeScale = Time.timeScale;
        Time.timeScale = 0f;
        Rigidbodies_TimeScale(0);
	}
	
	public void PauseBack()
	{
		pauseInLevel = false;
        scr.objLev.pauseMenuAnim.SetTrigger(Animator.StringToHash("back"));
        Time.timeScale = CurrTimeScale;
		scr.objLev.pauseMenuAnim.gameObject.SetActive(false);
        Rigidbodies_TimeScale(1);
	}
	
	public void MenuResult()
	{
		gamePaused            = true;
        scr.enAlg.enabled     = false;
        scr.enAlg_1.enabled   = false;
        scr.pMov.enabled      = false;
        scr.bonObjMan.enabled = false;
		scr.objLev.resultMenuAnim.gameObject.SetActive(true);
	}

	public void MenuResultBack()
	{
		scr.objLev.resultMenuAnim.SetBool ("call", false);
		Time.timeScale = 1f;
		scr.objLev.resultMenuAnim.gameObject.SetActive(false);
	}
	
	public void Rigidbodies_TimeScale_0()
	{
		foreach (var t in scr.objLev.allRbs)
			t.constraints = RigidbodyConstraints2D.FreezeAll;
	}

	#endregion

	








    

	

	#region nonpbublic methods
	
	private void UpdateMenu()
	{
#if UNITY_EDITOR
		if(LeanInput.GetPressed(KeyCode.R))
			SceneManager.LoadScene("Menu");
#endif

		if (LeanInput.GetUp (KeyCode.Escape))
		{
			switch (CurrentMenu)
			{
				case EMenu.MainMenu:
					Application.Quit();
					break;
				case EMenu.MenuCareer:
					DependentMenuBack();
					break;
				case EMenu.MenuPlayers:
					DependentMenuBack();
					break;
			}
		}
	}
		
	private void UpdateLevel()
	{
		if (LeanInput.GetUp (KeyCode.Escape)) 
		{
			if (scr.tM.isBetweenTimes && Time.timeScale <= 0.1f)
			{
				scr.tM.CallBackBetweenTimesPanel();
				scr.levAudScr.Button_Sound();
			}
			else
			{
				if (!gamePaused && scr.pMov.startGame)
				{
					scr.levAudScr.Button_Sound();

					if (!pauseInLevel)
					{
						Pause();
						scr.objLev.mainCanvas.enabled = true;
					}   
					else
					{
						switch (scr.objLev.quitPanel.activeSelf)
						{
							case true:
								PauseQuitBack();

								break;
							case false:
								PauseBack();
								scr.objLev.mainCanvas.enabled = false;
								break;
						}
					} 
				}
			}
		}

		if (LeanInput.GetUp(KeyCode.Space))
		{
			if (!scr.pMov.startGame || scr.tM.isBetweenTimes)
				StartGame();
		}
	}

	private void Rigidbodies_TimeScale(int _TScale)
	{
		foreach (var t in scr.objLev.allRbs)
		{
			t.bodyType = _TScale == 0 ?
				RigidbodyType2D.Kinematic
				: RigidbodyType2D.Dynamic;
		}
	}
    
	private void PauseQuitBack()
	{
		scr.objLev.quitPanel.SetActive(false);
		scr.objLev.pauseMenuAnim.gameObject.SetActive(true);
		scr.objLev.pauseMenuAnim.SetTrigger("call");
	}

	#endregion
	
    /*public void LoadPractice()
    {
        scr.buf.isPractice = true;
        scr.buf.SetData();
        SceneManager.LoadScene("Level");
        scr.objM.buttonsSource.Play();
    }*/
}



