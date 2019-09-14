using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreText : MonoBehaviour
{
	public enum ScoreType
	{
		score, great, fast, late, miss
	}

	// Start is called before the first frame update

	public NoteMaster.State indicationState;
	public ScoreType type;
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
		 
        if(type == ScoreType.score)
		{
			scoreText.text = "Score : " + NoteMaster.score.ToString(); 
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
