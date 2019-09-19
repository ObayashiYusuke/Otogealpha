using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;
using UnityEngine.SceneManagement;

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
	[System.NonSerialized] public int inputPushBuffer = 0;//入力を数値化 
	[System.NonSerialized] public static int buttonnumber = 4;

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
	//public static string musicName = "Boss";//"KIKKUNのテーマ";
	public static string noteDataName;//譜面データの名前
	//private float waittime = 0;	//譜面が曲に対して遅れる時間
	//private float barTime;  //1小節の時間
	//private float endtime;  //開始から終了までの時間
	public static float noteMakeTime = 0;//比較用の開始時刻記録用

	//生成したノーツオブジェクトのリスト
	public static List<Note> noteList = new List<Note>();


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
		if ((state == State.beforeMakeObj) && Input.GetKeyDown(KeyCode.Return))
		{//enterが押されたらオブジェクト生成に移行
			state = State.afterMakeObj;
			score = 0; great = 0; fast = 0; late = 0; miss = 0;

			judgeText.text = "";

			musicData = NoteDataParser.NoteDataParse(noteDataName);//譜面の名前から譜面のデータをMusicData型に解析
			Debug.Log("adress is MusicData/" + musicData.musicName);
			Debug.Log      (Resources.Load("MusicData/" + musicData.musicName, typeof(AudioClip)));
			musicSound = (Resources.Load("MusicData/" + musicData.musicName, typeof(AudioClip)) as AudioClip);//解析したMusicData型のデータから曲の名前を抽出し曲設定
			Debug.Log(musicData.musicName.Contains("\r"));
			noteList = noteMaker.GetComponent<NoteObjMaker>().NoteObjMake(musicData);//MusicData型のデータから譜面を生成させ、そのオブジェクトのリストを受け取る
			noteMakeTime = Time.time;
			Debug.Log("noteMakeTime = " + noteMakeTime);
			for (int i = 0; i < noteList.Count; i++)
			{
				noteList[i].noteMove.StartMove();
			}
		}
		else if (state == State.afterMakeObj && Time.time >= (noteMakeTime + (60 / (musicData.BPM / 4))))//1小節分の時間がたったら
		{
			state = State.playing;
			MusicPlay();
		}
		
		else if (state == State.playing && (Time.time - (noteMakeTime + (60 / (musicData.BPM / 4)))) > musicData.playTime)
		{
			state = State.result;
			noteMakeTime = 0;
			timeBar.value = 0;
			timeBar.gameObject.SetActive(false);
			audioSource.Stop();//音楽停止
			resultImageObject.SetActive(true);//リザルト画像表示

			CalcAchievementRate();//achievementrateを算出

			judgeText.enabled = false;

			DestroyList();//リストの消去
		}
		else if (state == State.result && Input.GetKeyDown(KeyCode.Return))
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



	

	/*private void JudgeButton()
	{
		//Debug.Log(noteList[0].transform.position);
		Note note;
		int sub = 0; 

		inputPushBuffer = 0;
		
		//4ボタン
		if (Input.GetKeyDown(KeyCode.S))
		{
			inputPushBuffer += 1;
		}

		if (Input.GetKeyDown(KeyCode.F))
		{
			inputPushBuffer += 2;
		}
		if (Input.GetKeyDown(KeyCode.J))
		{
			inputPushBuffer += 4;
		}
		if (Input.GetKeyDown(KeyCode.L))
		{
			inputPushBuffer += 8;
		}
		
		if (inputPushBuffer != 0)
		{
			nowTime = Time.time - noteMakeTime - (realWait - (60 / (musicData.BPM / 4)));//今の時間を送れた時間分引く

			Debug.Log("push time is " + nowTime);

		}
		while (inputPushBuffer != 0)
		{

			sub = noteList.FindIndex(x => x.justTime <= nowTime + goodJudge && x.justTime >= nowTime - goodJudge
					&& (x.noteType & inputPushBuffer) != 0);
			if (sub != -1)
			{
				note = noteList[sub];
				Debug.Log("note.time is " + note.justTime);

				if (note.justTime <= nowTime + greatJudge && note.justTime >= nowTime - greatJudge)
				{
					score += 100;
					Debug.Log("GREAT");
					great++;
					judgeText.text = "Great";
				}
				else if (note.justTime > nowTime)
				{
					score += 50;
					Debug.Log("FAST");
					fast++;
					judgeText.text = "Fast";
				}
				else
				{
					score += 50;
					Debug.Log("LATE");
					late++;
					judgeText.text = "Late";
				}
				inputPushBuffer = inputPushBuffer & ~note.noteType;//判定したのノーツの入力部分を0マスク
				Destroy(note.gameObject);//破壊
				noteList.RemoveAt(sub);//リストから消去

			}
			else break;//条件に合うノーツがなくなったら脱出
		}

	}*/

	public void MissJudge()
	{
		int i;
		for(i = noteList.Count - 1;i >= 0; i--)
		{
			if(noteList[i].justTime + goodJudge + noteMakeTime + (realWait - (60 / (musicData.BPM / 4))) < Time.time)
			{
				break;
			}
		}
		for(;i>=0;i--)
		{
			Destroy(noteList[i].gameObject);
			noteList.RemoveAt(i);
			miss++;
			judgeText.text = "Miss";
		}
	} 
	

	public void MusicPlay()
	{
		Debug.Log("real bar time = " + (Time.time - noteMakeTime).ToString());
		realWait = Time.time - noteMakeTime;

		Debug.Log("realWait =" + realWait.ToString());
		audioSource.PlayOneShot(musicSound);
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
	void DestroyList()
	{
		for (int i = 0; i < noteList.Count; i++)//リストを消去
		{
			if (noteList[i].gameObject != null)
			{
				Destroy(noteList[i].gameObject);
			}
		}
		noteList.Clear();
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

