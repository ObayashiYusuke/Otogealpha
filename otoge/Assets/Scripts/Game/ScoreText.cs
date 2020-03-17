using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/*画面に表示する文字に持たせるクラス　どの画面の時にどの情報を記録するかを入力することで必要な文字列データを設定する*/
public class ScoreText : MonoBehaviour
{
	public enum ScoreType
	{
		score, great, fast, late, miss,none
	}

	// Start is called before the first frame update

	public NoteMaster.State indicationState;//どの状態のとき表示するか
	public ScoreType type;//どの中身を表示するか
	private Text scoreText;
	private void Start()
	{
		scoreText = this.GetComponent<Text>();
	}
	// Update is called once per frame
	void Update()
    {
		if(NoteMaster.state == indicationState)
		{
			scoreText.enabled = true;
		}
		else
		{
			scoreText.enabled = false;
		}
		 
        if(type == ScoreType.score && indicationState == NoteMaster.State.playing)
		{
			scoreText.text = "Score : " + NoteMaster.score.ToString(); 
		}
		else if(type == ScoreType.score)
		{
			scoreText.text = "Score : " + NoteMaster.score.ToString() + "/" + NoteMaster.achievementRate.ToString() + "%";
		}
		else if (type == ScoreType.great)
		{
			scoreText.text = "Great : " + NoteMaster.great.ToString();
		}
		else if (type == ScoreType.fast)
		{
			scoreText.text = "Fast : " + NoteMaster.fast.ToString();
		}
		else if (type == ScoreType.late)
		{
			scoreText.text = "Late : " + NoteMaster.late.ToString();
		}
		else if (type == ScoreType.miss)
		{
			scoreText.text = "Miss : " + NoteMaster.miss.ToString();
		}
	}
}
