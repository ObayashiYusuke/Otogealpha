using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;
public class GameMaster : MonoBehaviour
{

	public Text scoreText;//もろもろの表示用テキスト
	public Text greatText;
	public Text fastText;
	public Text lateText;
	public Text missText;
	public Text judgeText;
	public Text resultScoreText;
	public Text resultGreatText;
	public Text resultFastText;
	public Text resultLateText;
	public Text resultMissText;

	public GameObject noteMasterObj;//noteMasterとのつながり
	private NoteMaster noteMastercomp;

	public Button button;
	public GameObject prefab;
	public GameObject obj1,parentObject;
	

	private object[] allNoteData;   //譜面情報を取得しておく配列

	List<MusicData> musicDataList = new List<MusicData>();

	[System.NonSerialized] public static int score = 0;
	[System.NonSerialized] public static int great = 0;
	[System.NonSerialized] public static int fast = 0;
	[System.NonSerialized] public static int late = 0;
	[System.NonSerialized] public static int miss = 0;

	public int state = 0;//0で曲選択画面、1で譜面生成前待機、2で譜面生成から曲再生まで、3でプレイ中、4でリザルト
	// Start is called before the first frame update
	void Start()
    {
		TextAsset textAsset;
		string noteData;
		allNoteData = (Resources.LoadAll("NoteData", typeof(TextAsset)));
		for (int i = 0; i < allNoteData.Length; i++)
		{
			textAsset = (TextAsset)allNoteData[i];
			noteData = textAsset.text;
			Debug.Log("data: " + noteData);
			musicDataList.Add(MakeMusicData(noteData));

			MakeButton(i);

		}

		noteMastercomp = noteMasterObj.GetComponent<NoteMaster>();
		ResultTextSet(false);
		state = 0;
	}

    // Update is called once per frame
    void Update()
    {
		scoreText.text = "Score : " + score.ToString();
		greatText.text = "Great : " + great.ToString();
		fastText.text = "Fast : " + fast.ToString();
		lateText.text = "Late : " + late.ToString();
		missText.text = "Miss : " + miss.ToString();
		resultScoreText.text = "Score : " + score.ToString();
		resultGreatText.text = "Great : " + great.ToString();
		resultFastText.text = "Fast : " + fast.ToString();
		resultLateText.text = "Late : " + late.ToString();
		resultMissText.text = "Miss : " + miss.ToString();
		if(state == 1 && Input.GetKeyDown(KeyCode.Return))
		{
			state = 2;
			//noteMastercomp.MakeNote(MusicData);ここで作成
		}


	}
	public MusicData MakeMusicData(string allNoteText)//譜面の文字列、タイトル、bpm、プレイ時間
	{
		MusicData musicData= new MusicData();//returnするmusicdata
		string[] splitText;//改行で区切ったデータ
		int rowLength;//全部の行数
		string musicTitle;
		float bpm;
		float playTime;
		splitText = allNoteText.Split(char.Parse("\n"));//譜面データのテキストを改行で区切る
		rowLength = allNoteText.Split('\n').Length;
		musicTitle = GetMusicTitle(splitText);
		musicData.musicName = musicTitle;
		bpm = GetBPM(splitText);
		musicData.BPM = bpm;
		musicData.noteData = allNoteText;
		playTime = GetEndTime(splitText);
		musicData.playTime = playTime;
		
		return musicData;
	}

	private void MakeButton(int num)
	{
		parentObject = GameObject.Find("Canvas");
		Vector2 pos;
		pos.x = -200;
		pos.y = num * -100;
		obj1 = Instantiate(prefab) as GameObject;
		obj1.transform.SetParent(parentObject.transform, false);
		obj1.GetComponent<RectTransform>().localPosition = pos;
		Debug.Log("makebutton");
	}

	public string GetMusicTitle(string[] allText)       //タイトルを取得する関数!!!!!!! musictitle= で記述
	{
		int lineNum = SearchWord("musictitle=", allText);
		if (lineNum == -1)
		{
			return null;
		}
		return Regex.Replace(allText[lineNum], "musictitle=", "");//BPMをフロート型にして返す

	}

	public float GetBPM(string[] allText)       //BPMを取得する関数!!!!!!! bpm=oo で記述
	{
		int lineNum;
		lineNum = SearchWord("bpm=", allText);
		if (lineNum == -1)
		{
			return -1;
		}
		if (allText[lineNum].Split('.').Length > 2)//小数点が2個以上ある場合を例外処理
		{
			return -1;
		}
		return float.Parse(Regex.Replace(allText[lineNum], @"[^0-9.]", ""));//BPMをフロート型にして返す
	}

	public float GetEndTime(string[] allText)       //BPMを取得する関数!!!!!!! endtime=oo で記述
	{
		int lineNum;
		lineNum = SearchWord("endtime=", allText);
		if (lineNum == -1)
		{
			return -1;
		}
		if (allText[lineNum].Split('.').Length > 2)//小数点が2個以上ある場合を例外処理
		{
			return -1;
		}
		return float.Parse(Regex.Replace(allText[lineNum], @"[^0-9.]", ""));//BPMをフロート型にして返す
	}

	public int SearchWord(string str,string[] allText)
	/*文字列を検索し、最初のその文字列が見つかるまでtextNumを進める
	検索したい文字列、探す元の文字列(改行区切り),行の数
	 見つかればその行数,見つからなければ-1を返す*/
	{
		int i = 0;
		int rowLength = allText.Length;
		while (Regex.IsMatch(allText[i], str) == false)
		{
			i++;
			if (i >= rowLength)//見つからず最後の行まで行ったら失敗
			{
				return -1;
			}
		}
		return i;
	}


	

	public void ResultTextSet(bool disp)//リザルトテキストの表示非表示
	{
		resultGreatText.enabled = disp;
		resultFastText.enabled = disp;
		resultLateText.enabled = disp;
		resultMissText.enabled = disp;
		resultScoreText.enabled = disp;
	}
	public void PlayTextSet(bool disp)//プレイ中のテキストの表示非表示
	{
		greatText.enabled = disp;//
		fastText.enabled = disp;
		lateText.enabled = disp;
		missText.enabled = disp;
		scoreText.enabled = disp;
	}
}
