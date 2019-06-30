using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;
using UnityEngine.SceneManagement;


public class NoteMaster : MonoBehaviour
{
	public int life = 8;
	public int score = 0;
	public float speed = 10;
	public GameObject Pref;
	public int inputBuffer=0,inputBufferOld =0;//入力を数値化 oldはまだいらなかった
	public int buttonnumber = 4;

	public float greatJudge;
	public float goodJudge;

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
	public Text scoreText;
	public Text lifeText;

	//音楽情報の取得
	public AudioClip musicSound;
	private AudioSource audioSource;
	private float BPM = 1;


	//譜面情報
	private float waittime = 0;	//譜面が曲に対して遅れる時間
	private float barTime;  //1小節の時間
	private float endtime;	//開始から終了までの時間
	private float starttime;//比較用の開始時刻記録用

	//生成したノーツオブジェクトのリスト
	List<Note> noteList = new List<Note>();

	float realWait = 0;


	private IEnumerator TestCoroutine()
	{
		

		fumenAllText = (Resources.Load("Test2", typeof(TextAsset)) as TextAsset).text;//テキストの読み込み

		splitText = fumenAllText.Split(char.Parse("\n"));//テキストを改行ごとに分ける
		rowLength = fumenAllText.Split('\n').Length;

		//ここから情報取得

		BPM = GetBPM();

		if(BPM <= 0)
		{
			Debug.Log("BPM is not found.");
			BPM = 1;
		}
		else if (BPM > 0)
		{
			BPM = float.Parse(Regex.Replace(splitText[textNum], @"[bpm=]", ""));
			Debug.Log("BPM IS " + BPM);
		}
		barTime = 60 / (BPM / 4);
		Debug.Log("barTime is" + barTime);

		waittime = GetWaittime();
		Debug.Log("waittime is " + waittime);

		endtime = GetEndtime();
		Debug.Log("endtime is " + endtime);


		for (int i = 1; i <= 50; i++)
		{
			Debug.Log(i + "小節目の生成");
			MakeOneBar(i,waittime,i);
		}


		starttime = Time.time;

		Debug.Log("real start = " + starttime);
		yield return new WaitForSeconds(barTime);//1小節分待つ
		Debug.Log("real bar time = " + (Time.time - starttime).ToString());
		realWait = Time.time - starttime;
		MusicPlay();

		
		
	}

	public float GetBPM()		//BPMを取得する関数!!!!!!! bpm=oo で記述
	{
		
		if (SearchWord("bpm=") == false)
		{
			return -1;
		}
		if (splitText[textNum].Split('.').Length > 2)//小数点が2個以上ある場合を例外処理
		{
			return -1;
		}
		return float.Parse(Regex.Replace(splitText[textNum], @"[^0-9.]", ""));//BPMをフロート型にして返す

	}

	public float GetWaittime()		//譜面再生までの時間を取得 waittime=oo で記述
	{
		if(SearchWord("waittime=") == false)
		{
			return 0;
		}
		if (splitText[textNum].Split('.').Length > 2)//小数点が2個以上ある場合を例外処理
		{
			return 0;
		}
		return float.Parse(Regex.Replace(splitText[textNum], @"[^0-9.]", ""));//waittimeをフロート型にして返す

	}

	public float GetEndtime()	//開始から終了までの時間を取得する関数 endtime=ooで記述
	{
		if (SearchWord("endtime=") == false)
		{
			return 0;
		}
		if (splitText[textNum].Split('.').Length > 2)//小数点が2個以上ある場合を例外処理
		{
			return 0;
		}
		return float.Parse(Regex.Replace(splitText[textNum], @"[^0-9.]", ""));//endtimeをフロート型にして返す

	}



	public bool MakeOneBar(int skipBar, float wait, int barNumber)
	{
		Vector3 pos,size;
		pos.y = 0;
		size.x = 0.3f;
		size.y = 1;

		Note n;
		string lineData;//各行のデータを入れる
		GameObject obj;



		if(SearchWord("--") == false)
		{
			return false;
		}
		int startline = textNum;
		//Debug.Log("startline is " + startline);

		textNum++;

		if (SearchWord("--") == false)
		{
			return false;
		}
		//Debug.Log("textNum is " + textNum);

		float interval = barTime / (textNum - startline - 1);//(textNum - startline - 1)は行の数

		for (int i = 0; (startline + 1 + i ) < textNum; i++)
		{
			/*Debug.Log("小節の中の" + (i + 1) + "行目");*/
			lineData = (Regex.Replace(splitText[startline + 1 + i], @"[^0-9A-Z]",""));
			/*Debug.Log("lineData is" + lineData);*/
			for(int l = 0;l < buttonnumber; l++)
			{
				if (lineData[l] != '0')
				{
					obj = Instantiate(Pref);
					pos.x = (7 - (skipBar * barTime * speed) - (i * interval * speed) - (wait * speed));
					pos.z = (float)(lineData[l] - '1') / 2 + l;

					obj.transform.position = pos;

					size.z = (float)(lineData[l] - '0') - 0.04f;
					obj.transform.localScale = size;

					n = obj.GetComponent<Note>();
					n.noteType = GetNoteType(l, lineData[l] - '0');
					n.gameObject = obj;
					n.time = waittime + (barNumber * barTime) + (i * interval);
					//Debug.Log("time is " + n.time);

					noteList.Add(n);

				}
			}


			

		}

		return true;

	}

	public int GetNoteType(int left, int size)
	{
		int a = 0;
		for (int i = 1; size >= i; i++)
		{
			a = (a * 2) + 1;
		}
		for(int i = 1; left >= i; i++)
		{
			a *= 2;
		}
		return a;
	}


	public bool SearchWord(string str)
		/*文字列を検索し、最初のその文字列が見つかるまでtextNumを進める
		 見つかればtrue,見つからなければfalse*/
	{
		int rowfrom = textNum;
		if (textNum >= rowLength)//見つからず最後の行まで行ったら失敗
		{
			return false;
		}
		while (Regex.IsMatch(splitText[textNum], str) == false)
		{
			textNum++;
			if (textNum >= rowLength)//見つからず最後の行まで行ったら失敗
			{
				Debug.Log(str + " は見つかりませんでした");
				textNum = rowfrom;
				return false;
			}
		}
		return true;
	}

	// Start is called before the first frame update
	void Start()
    {
		starttime = Time.time;

		StartCoroutine(TestCoroutine());

		audioSource = gameObject.GetComponent<AudioSource>();


	}

	private void FixedUpdate()
	{
		Note note;
		int sub = 0; 
		float nowTime;

		//inputBuffer = inputBufferOld;
		inputBuffer = 0;
		
		if (Input.GetKeyDown(KeyCode.S))
		{
			inputBuffer += 1;
		}

		if (Input.GetKeyDown(KeyCode.F))
		{
			inputBuffer += 2;
		}
		if (Input.GetKeyDown(KeyCode.J))
		{
			inputBuffer += 4;
		}
		if (Input.GetKeyDown(KeyCode.L))
		{
			inputBuffer += 8;
		}

		if(inputBuffer != 0)
		{
			nowTime = Time.time - starttime - (realWait - barTime);//今の時間を送れた時間分引く

			Debug.Log("push time is " + nowTime);

			pushtime += "push time is " + nowTime + "\n";

			sub = noteList.FindIndex(x => x.time <= nowTime + goodJudge && x.time >= nowTime - goodJudge
				&& (x.noteType & inputBuffer) != 0);
			if(sub != -1)
			{
				note = noteList[sub];//前後0.16秒をひろう
				Debug.Log("note.time is " + note.time);

				if (note.time <= nowTime + greatJudge && note.time >= nowTime - greatJudge)
				{
					score += 100;
					Debug.Log("GREAT");
				}
				else if(note.time > nowTime)
				{
					score += 50;
					Debug.Log("FAST");
				}
				else
				{
					score += 50;
					Debug.Log("LATE");
				}
				Destroy(note.gameObject);//破壊

										 noteList.RemoveAt(sub);
			}
			

		}


	}

	// Update is called once per frame
	void Update()
    {
		scoreText.text = "Score : " + score.ToString();
		lifeText.text = "Life : " + life.ToString();


		if((Time.time - starttime) > endtime)
		{
			Finish();
		}

		
	}

	public void MusicPlay()
	{
		audioSource.PlayOneShot(musicSound);
	}
	void Finish()
	{
		SceneManager.LoadScene("Result");
	}

	string pushtime;
	void OnGUI()
	{
		GUI.Label(new Rect(10, 10, 100, 5000), pushtime);

		string notes = "";
		for (int i = 0; i < 10; i++)
		{
			notes += "note time = " + noteList[i].time.ToString() + "\n";
		}
		GUI.Label(new Rect(150, 10, 1000, 5000), notes);
	}
}

