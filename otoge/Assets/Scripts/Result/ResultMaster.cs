using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class ResultMaster : MonoBehaviour
{
	// Start is called before the first frame update
	public Text scoretext,greattext,fasttext,latetext,misstext;
	private int score,great,fast,late,miss;
    void Start()
    {
		score = NoteMaster.score;
		great = NoteMaster.great;
		fast = NoteMaster.fast;
		late = NoteMaster.late;
		miss = NoteMaster.miss;
	}

    // Update is called once per frame
    void Update()
    {
		scoretext.text = "score : " + score.ToString();
		greattext.text = "great : " + great.ToString();
		fasttext.text = "fast : " + fast.ToString();
		latetext.text = "late : " + late.ToString();
		misstext.text = "miss : " + miss.ToString() + "\n達成率 : " + (float)((score / 100f)/(great + fast + late + miss) * 100) + "%";
		if (Input.GetKeyDown(KeyCode.Space))
		{
			SceneManager.LoadScene("MusicGame");
		}else if (Input.GetKeyDown(KeyCode.Return))
		{
			SceneManager.LoadScene("MusicSelect");
		}
	}
}
