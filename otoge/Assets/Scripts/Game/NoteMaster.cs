using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;
using UnityEngine.SceneManagement;
using System;

public class NoteMaster : MonoBehaviour
{
	public enum State
	{
		select,beforeMakeObj,afterMakeObj,playing,result
	}

	[System.NonSerialized]public static int score = 0;
	[System.NonSerialized] public static int great = 0;
	[System.NonSerialized] public static int fast = 0;
	[System.NonSerialized] public static int late = 0;
	[System.NonSerialized] public static int miss = 0;
	public static float achievementRate;
	public static float speed = 10;

	public GameObject noteMaker;//NoteMakerのオブジェクト

	public static float greatJudge;
	public static float goodJudge;

	//　読む込むテキストが書き込まれている.txtファイル
	[SerializeField]
	private TextAsset textAsset;
	//　Resourcesフォルダから直接テキストを読み込む
	private string fumenAllText;
	//　改行で分割して配列に入れる

	private string[] splitText;

	//　テキストの現在行番号
	private int textNum = 0;
	private int rowLength;

	//スコア表示

	public Text judgeText;

	public Text speedText;

	//進行度表示
	public Slider timeBar;

	//音楽情報の取得
	[System.NonSerialized]public AudioClip musicSound;
	private AudioSource audioSource;
	private float BPM = 1;


	//譜面情報

	public static string noteDataName;//譜面データの名前
	public static float noteMakeTime = 0;//比較用の開始時刻記録用

	//リザルト情報
	public static List<Record> records = new List<Record>();

	//各画像操作用
	GameObject resultImageObject, selectImageObject;
	Image imageComponent, resultComponent;
	public static float realWait = 0;
	[System.NonSerialized]public float nowTime = 0;
	//ボタン操作用
	private GameObject basicButton,normalButton, hardButton,veryHardButton,speedUp,speedDown;
	
	public static State state = State.select;//状態を記録 0:曲選択 1:オブジェクト生成前待機 2:オブジェクト生成後待機 3:プレイ中(曲再生後) 4:リザルト 

	public static MusicData musicData;
	private void Start()
	{
		greatJudge = 0.04f;					//Greatの時間幅
		goodJudge = 0.1f;					//Goodの時間幅(実際には倍となる)
		resultImageObject = GameObject.Find("ResultImage");//結果の画像オブジェクトを取得
		resultImageObject.SetActive(false);

		selectImageObject = GameObject.Find("SelectImage");
		selectImageObject.SetActive(true);


	


		timeBar.gameObject.SetActive(false);
		//Debug.Log("object" + allNoteData[0].ToString());
		audioSource = gameObject.GetComponent<AudioSource>();
		basicButton = GameObject.Find("BasicButton");
		normalButton = GameObject.Find("NormalButton");
		veryHardButton = GameObject.Find("VeryHardButton");
		hardButton = GameObject.Find("HardButton");
		speedUp = GameObject.Find("SpeedUp");
		speedDown = GameObject.Find("SpeedDown");
	}

	void Update()
	{
		if (Input.GetKey(KeyCode.Escape))//強制終了
		{
			UnityEngine.Application.Quit();
		}
		
		
		speedText.text = speed.ToString();
		
		

		if (state == State.select)
		{
			
		}
		if ((state == State.beforeMakeObj))
		{//enterが押されたらオブジェクト生成に移行
			if (Input.GetKeyDown(KeyCode.Return) || TouchCheck())
			{
				state = State.afterMakeObj;
				score = 0; great = 0; fast = 0; late = 0; miss = 0;

				judgeText.text = "";


				musicData = noteMaker.GetComponent<NoteObjMaker>().NoteDataParse(noteDataName);//譜面データのファイル名からMusicData型に変換してもらう
				musicSound = (Resources.Load("MusicData/" + musicData.musicName, typeof(AudioClip)) as AudioClip);//解析したMusicData型のデータから曲の名前を抽出し曲設定

				Debug.Log("adress is MusicData/" + musicData.musicName);
				Debug.Log(Resources.Load("MusicData/" + musicData.musicName, typeof(AudioClip)));

				noteMakeTime = Time.time;
				Debug.Log("noteMakeTime = " + noteMakeTime);

			}
		}
		else if (state == State.afterMakeObj && Time.time >= (noteMakeTime + (60 / (musicData.BPM / 4))))//生成してから1小節分の時間がたったら
		{
			state = State.playing;
			MusicPlay();
		}
		
		else if (state == State.playing && (Time.time - (noteMakeTime + (60 / (musicData.BPM / 4)))) > musicData.endTime)
		{
			state = State.result;
			noteMakeTime = 0;
			timeBar.value = 0;
			timeBar.gameObject.SetActive(false);
			audioSource.Stop();//音楽停止
			resultImageObject.SetActive(true);//リザルト画像表示

			CalcAchievementRate();//achievementrateを算出

			judgeText.enabled = false;

			String ScoreData = "score:" + score + " great:" + great + " fast:" + fast + " late:" + late + " miss:" + miss+ " achievementrate:"+achievementRate;

			DateTime dt = DateTime.Now;

			records.Sort((a,b) => a.noteNum - b.noteNum);

			ResultReporter.SaveText(noteDataName+"\n"+ScoreData+"\n"+ RecordsToStr.RecordsToString(records),dt.ToString("MMdd_HHmm")+".txt");
			DestroyNoteList();//リストの消去
			records.Clear();//レコードのリセット

		}
		else if (state == State.result)
		{
			if (Input.GetKeyDown(KeyCode.Return) || TouchCheck())
			{
				state = State.select;
				speedText.enabled = true;


				score = 0; great = 0; fast = 0; late = 0; miss = 0;

				resultImageObject.SetActive(false);
				judgeText.text = "Press Enter\n    To Start!";
				judgeText.enabled = true;

				selectImageObject.SetActive(true);

				ButtonControl(true);//全てのボタンを表示

			}
		}
		if (state == State.playing)//timeBar処理
		{
			timeBar.value = (Time.time - (noteMakeTime + (60 / (musicData.BPM / 4)))) / musicData.endTime;//経過時間/endTime
			MissJudge();
		}
	}


	public void GoToGame()
	{
		Debug.Log("GoToGame");
		state = State.beforeMakeObj;
		ButtonControl(false);
		selectImageObject.SetActive(false);
		
		timeBar.gameObject.SetActive(true);
		speedText.enabled = false;
	}

	public bool TouchCheck()
	{
		if (Input.touchCount > 0)
		{
			Touch touch = Input.GetTouch(0);
			if(touch.phase == TouchPhase.Began)
			{
				return true;
			}
			else
			{
				return false;
			}
		}
		else
		{
			return false;
		}
	}

	public void MusicPlay()
	{
		Debug.Log("real bar time = " + (Time.time - noteMakeTime).ToString());
		realWait = Time.time - noteMakeTime;

		Debug.Log("realWait =" + realWait.ToString());
		audioSource.PlayOneShot(musicSound);
	}





	public void MissJudge()
	{
		int i;
		for(i = musicData.noteList.Count - 1;i >= 0; i--)
		{
			if(musicData.noteList[i].justTime + goodJudge + noteMakeTime + (realWait - (60 / (musicData.BPM / 4))) < Time.time)
			{
				break;
			}
		}
		for(;i>=0;i--)
		{
			Note n = musicData.noteList[i];
			addRecord(n.noteNum, n.justTime, 0f, "miss");
			Destroy(musicData.noteList[i].gameObject);
			musicData.noteList.RemoveAt(i);
			miss++;
			judgeText.text = "Miss";
		}
	} 
	

	
	void CalcAchievementRate()
	{
		if ((great + fast + late + miss) == 0)
		{
			achievementRate = 0;
		}
		else
		{
			achievementRate = score / (great + fast + late + miss);
		}
	}
	public void DestroyNoteList()
	{
		for (int i = 0; i < musicData.noteList.Count; i++)//リストを消去
		{
			if (musicData.noteList[i].gameObject != null)
			{
				Destroy(musicData.noteList[i].gameObject);
			}
		}
		musicData.noteList.Clear();
	}
	

	private void ButtonControl(bool b)
	{
		basicButton.SetActive(b);
		normalButton.SetActive(b);
		hardButton.SetActive(b);
		speedUp.SetActive(b);
		speedDown.SetActive(b);
	}
	public  void JudgeTextRewrite(string str)
	{
		judgeText.text = str;
	}

	public static void addRecord(int noteNum, float justTime,float hitTime,string judgeGrade)
	{
		records.Add(new Record(noteNum, justTime, hitTime, judgeGrade));
	}
	/*string pushtime;
	void OnGUI()
	{
		GUI.Label(new Rect(10, 10, 100, 5000), pushtime);

		string notes = "";
		for (int i = 0; i < noteList.Count; i++)
		{
			notes += "note time = " + noteList[i].time.ToString() + "\n";
		}
		GUI.Label(new Rect(150, 10, 1000, 5000), notes);
	}*/
}

