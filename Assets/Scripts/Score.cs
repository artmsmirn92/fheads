using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
	#region serialized fields

    [SerializeField] private Scripts scr;

	public Text enemyScore,  enSc1;
	public Text playerScore, plSc1;

	#endregion

	#region api

	public static int PlayerScore;
	public static int EnemyScore;
	
	public void SetScore()
	{
		playerScore.text = PlayerScore.ToString();
		enemyScore.text  = EnemyScore.ToString();
		plSc1.text       = PlayerScore.ToString();
		enSc1.text       = EnemyScore.ToString();
		System.GC.Collect();
	}

	#endregion

	#region engine methods

	private void Awake ()
	{
		scr.univFunc.ShowReviewGameDialogOnGame10();
		(PlayerScore, EnemyScore) = (0, 0);
		SetScore ();
	}

	#endregion
}
