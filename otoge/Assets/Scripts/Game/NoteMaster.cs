using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;
using UnityEngine.SceneManagement;
using System;

/*
 このクラスはゲーム全体の流れや処理を管理するクラス 
	 */
public class NoteMaster : MonoBehaviour
{
	public enum State
	{
		select,beforeMakeObj,afterMakeObj,playing,result
	}

	[System.NonSerialized]public static int score = 0;//それぞれ得点と判定の数を記録
	[System.NonSerialized] public static int great = 0;
	[System.NonSerialized] public static int fast = 0;
	[System.NonSerialized] public static int late = 0;
	[System.NonSerialized] public static int miss = 0;
	public static float achievementRate;
	public static float speed = 16;//譜面の流れる速度

	public GameObject noteMaker;//NoteMakerのオブジェクト

	public static float greatJudge;//それぞれの判定に含まれる時間を記録する
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
	public static float noteMakeTime = 0;//ゲームの開始時刻記録

	//リザルト情報
	public static List<Record> records = new List<Record>();
	public static List<TouchRecord> touchRecords = new List<TouchRecord>();

	//各画像操作用
	GameObject resultImageObject, selectImageObject;
	Image imageComponent, resultComponent;
	public static float realWait = 0;
	[System.NonSerialized]public float nowTime = 0;
	//ボタン操作用
	private GameObject speedUp,speedDown,color1_1,color2_1,color4_1,rageOut1_1,rageOut2_1;

	
	public static State state = State.select;//状態を記録 0:曲選択 1:オブジェクト生成前待機 2:オブジェクト生成後待機 3:プレイ中(曲再生後) 4:リザルト 

	public static MusicData musicData;
	private void Start()
	{
		greatJudge = 0.04f;					//Greatの時間幅
		goodJudge = 0.07f;					//Goodの時間幅(実際には倍となる)
		resultImageObject = GameObject.Find("ResultImage");//結果の画像オブジェクトを取得
		resultImageObject.SetActive(false);

		selectImageObject = GameObject.Find("SelectImage");
		selectImageObject.SetActive(true);


	


		timeBar.gameObject.SetActive(false);

		
		audioSource = gameObject.GetComponent<AudioSource>();
		
		speedUp = GameObject.Find("SpeedUp");
		speedDown = GameObject.Find("SpeedDown");
		color1_1 = GameObject.Find("Color1_1");
		color2_1 = GameObject.Find("Color2_1");
		color4_1 = GameObject.Find("Color4_1");
		rageOut1_1 = GameObject.Find("RageOut1_1");
		rageOut2_1 = GameObject.Find("RageOut2_1");

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
				score = 0; great = 0; fast = 0; late = 0; miss = 0;//評価数のリセット

				judgeText.text = "";


				musicData = noteMaker.GetComponent<NoteObjMaker>().NoteDataParse(noteDataName);//譜面データのファイル名からMusicData型に変換してもらう
				musicSound = (Resources.Load("MusicData/" + musicData.musicName, typeof(AudioClip)) as AudioClip);//解析したMusicData型のデータから曲の名前を抽出し曲設定

				Debug.Log("adress is MusicData/" + musicData.musicName);
				Debug.Log(Resources.Load("MusicData/" + musicData.musicName, typeof(AudioClip)));

				noteMakeTime = Time.time;//現在の時間を譜面が流れ出す時間に設定
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

			ResultReporter.SaveText(noteDataName+"\n"+ScoreData+"\n"+ RecordsToStr.RecordsToString(records),"Record" + dt.ToString("MMdd_HHmm")+".txt");
			ResultReporter.SaveText(TouchRecordToStr.TouchRecordToString(touchRecords),"Touch" + dt.ToString("MMdd_HHmm") + ".txt");
			DestroyNoteList();//リストの消去
			records.Clear();//レコードのリセット
			touchRecords.Clear();

		}
		else if (state == State.result)
		{
			if (Input.GetKeyDown(KeyCode.Return) || TouchCheck())//リザルト画面で画面がタッチされたら
			{
				state = State.select;
				speedText.enabled = true;


				score = 0; great = 0; fast = 0; late = 0; miss = 0;//評価数のリセット

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


	public void GoToGame()//ゲーム開始時の処理　セレクト画面のボタンが押されると呼び出される
	{
		Debug.Log("GoToGame");
		state = State.beforeMakeObj;
		ButtonControl(false);
		selectImageObject.SetActive(false);
		
		timeBar.gameObject.SetActive(true);
		speedText.enabled = false;
	}
	public void GoToTitle()//タイトルボタンが押されると呼び出される
	{
		if (state == State.playing||state == State.afterMakeObj)
		{
			audioSource.Stop();//音楽停止
			judgeText.enabled = false;
			DestroyNoteList();//リストの消去
			records.Clear();//レコードのリセット
			touchRecords.Clear();
		}
		
		speedText.enabled = true;


		score = 0; great = 0; fast = 0; late = 0; miss = 0;

		resultImageObject.SetActive(false);
		judgeText.text = "  Touch\n    To Start!";
		judgeText.enabled = true;

		selectImageObject.SetActive(true);

		ButtonControl(true);//全てのボタンを表示

		state = State.select;

	}
	public bool TouchCheck()//画面がタッチされたらtrue、それ以外ならfalse
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

	public void MusicPlay()//音楽を再生
	{
		Debug.Log("real bar time = " + (Time.time - noteMakeTime).ToString());
		realWait = Time.time - noteMakeTime;

		Debug.Log("realWait =" + realWait.ToString());
		audioSource.PlayOneShot(musicSound);
	}





	public void MissJudge()//ノーツのミスを判定(別クラスにすべきでした)
	{
		int i;
		for(i = musicData.noteList.Count - 1;i >= 0; i--)//goodの判定を過ぎた分iが減る
		{
			if(musicData.noteList[i].justTime + goodJudge + noteMakeTime + (realWait - (60 / (musicData.BPM / 4))) < Time.time)
			{
				break;
			}
		}
		for(;i>=0;i--)//good判定から漏れたものにすべてMissの処理を行う
		{
			Note n = musicData.noteList[i];
			addRecord(n.noteNum, n.justTime, 0f, "miss");
			//Destroy(musicData.noteList[i].gameObject);
			n.gameObject.GetComponent<NoteFadeOut>().DeleteFlagSet();
			musicData.noteList.RemoveAt(i);
			miss++;
			judgeText.text = "Miss";
		}
	} 
	

	
	void CalcAchievementRate()//得点が最高値の何パーセント取れたか計算
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
	public void DestroyNoteList()//ノーツリストの内容をリセット
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
	

	private void ButtonControl(bool b)//ボタンの表示非表示を管理
	{
		
		speedUp.SetActive(b);
		speedDown.SetActive(b);
		color1_1.SetActive(b);
		color2_1.SetActive(b);
		color4_1.SetActive(b);
		rageOut1_1.SetActive(b);
		rageOut2_1.SetActive(b);
	}
	public  void JudgeTextRewrite(string str)//判定の文字を変更
	{
		judgeText.text = str;
	}

	public static void addRecord(int noteNum, float justTime,float hitTime,string judgeGrade)//プレイログを記録するときの情報を1行リストに追加
	{
		records.Add(new Record(noteNum, justTime, hitTime, judgeGrade));
	}
	public static void addTouchRecord(float posx,float posy,float touchTime)//入力ログを記録するときの情報を1行リストに追加
	{
		touchRecords.Add(new TouchRecord(posx,posy,touchTime));
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

