using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;

public class NoteObjMaker : MonoBehaviour
{
	public GameObject Pref;
	public GameObject leftLine;
	public GameObject rightLine;
	private string[] splitText; 
	private int rowLength;
	private int rowNum;
	private float barTime;
	private List<Note> noteList = new List<Note>();
	private float width,left,right;
	private ObaMusicData obaMusicData = new ObaMusicData();
	// Start is called before the first frame update

	public MusicData NoteDataParse(string noteDataName)
	{
		string noteData;//譜面データの全文
		float BPM;//取得したBPM
		float waitTime;
		float endTime;
		string musicName;
		string[] splitText;//行ごとに分けた文字列
		int splitLane;
		int rowLength;//データ全体の行数

		Debug.Log("adress is NoteData/" + noteDataName);

		noteData = (Resources.Load("NoteData/" + noteDataName, typeof(TextAsset)) as TextAsset).text;//テキストの読み込み

		splitText = noteData.Split(char.Parse("\n"));//テキストを改行ごとに分ける
		rowLength = noteData.Split('\n').Length;
		BPM = GetBPM(splitText, rowLength);
		waitTime = GetWaittime(splitText, rowLength);
		endTime = GetEndtime(splitText, rowLength);
		musicName = GetMusicName(splitText, rowLength);
		splitLane = GetButtonNumber(splitText, rowLength);
		obaMusicData.noteData = noteData;
		Debug.Log("musicData.noteData=" + obaMusicData.noteData);
		obaMusicData.musicName = musicName;
		Debug.Log("musicData.musicName=" + obaMusicData.musicName);
		obaMusicData.BPM = BPM;
		Debug.Log("musicData.BPM=" + obaMusicData.BPM);
		obaMusicData.waitTime = waitTime;
		Debug.Log("musicData.waitTime=" + obaMusicData.waitTime);
		obaMusicData.endTime = endTime;
		Debug.Log("musicData.endTime=" + obaMusicData.endTime);

		obaMusicData.splitLane = splitLane;
		Debug.Log("musicData.buttonNum=" + obaMusicData.splitLane);
		obaMusicData.noteList = NoteObjMake(obaMusicData);

		return obaMusicData;
	}

	public static float GetBPM(string[] splitText, int rowLength)       //BPMを取得する関数!!!!!!! bpm=oo で記述
	{
		int rowNum = SearchWord("bpm=", splitText, rowLength);
		if (rowNum == -1)
		{
			return -1;
		}
		if (splitText[rowNum].Split('.').Length > 2)//小数点が2個以上ある場合を例外処理
		{
			return -1;
		}
		return float.Parse(Regex.Replace(splitText[rowNum], @"[^0-9.]", ""));//BPMをフロート型にして返す

	}

	public static float GetWaittime(string[] splitText, int rowLength)      //譜面再生までの時間を取得 waittime=oo で記述
	{
		int rowNum = SearchWord("waittime=", splitText, rowLength);
		if (rowNum == -1)
		{
			return 0;
		}
		if (splitText[rowNum].Split('.').Length > 2)//小数点が2個以上ある場合を例外処理
		{
			return 0;
		}
		return float.Parse(Regex.Replace(splitText[rowNum], @"[^0-9.]", ""));//waittimeをフロート型にして返す
	}

	public static float GetEndtime(string[] splitText, int rowLength)   //開始から終了までの時間を取得する関数 endtime=ooで記述
	{
		int rowNum = SearchWord("endtime=", splitText, rowLength);
		if (rowNum == -1)
		{
			return 0;
		}
		if (splitText[rowNum].Split('.').Length > 2)//小数点が2個以上ある場合を例外処理
		{
			return 0;
		}
		return float.Parse(Regex.Replace(splitText[rowNum], @"[^0-9.]", ""));//endtimeをフロート型にして返す

	}

	public static string GetMusicName(string[] splitText, int rowLength)
	{
		int rowNum = SearchWord("musicname=", splitText, rowLength);
		if (rowNum == -1)//曲のファイル名を取得 musicname=ooで記述
		{
			return null;
		}
		string rmr = Regex.Replace(splitText[rowNum], "\r", "");
		return Regex.Replace(rmr, "musicname=", "");//musicNameを返す

	}

	public static int GetButtonNumber(string[] splitText, int rowLength)//ボタン数を取得buttonnumber=で記述
	{
		int rowNum = SearchWord("buttonnumber=", splitText, rowLength);
		if (rowNum == -1)
		{
			return 0;
		}
		if (splitText[rowNum].Split('.').Length > 1)//小数点が2個以上ある場合を例外処理
		{
			return 0;
		}
		return int.Parse(Regex.Replace(splitText[rowNum], @"[^0-9]", ""));//endtimeをフロート型にして返す

	}
	public static int SearchWord(string str, string[] splitText, int rowLength)
	/*文字列を検索し、最初のその文字列が見つかるまでtextNumを進める
	 見つかればtrue,見つからなければfalse*/
	{
		int rowNum = 0;
		while (Regex.IsMatch(splitText[rowNum], str) == false)
		{
			rowNum++;
			if (rowNum >= rowLength)//見つからず最後の行まで行ったら失敗
			{
				return -1;
			}
		}
		return rowNum;
	}

	void Start()
	{
		left = leftLine.transform.position.z;
		right = rightLine.transform.position.z;
		width = right - left;
		Debug.Log("幅は" + width);
	}
	public List<Note> NoteObjMake(ObaMusicData obaMusicData)
	{
		splitText = obaMusicData.noteData.Split(char.Parse("\n"));//テキストを改行ごとに分ける
		rowLength = obaMusicData.noteData.Split('\n').Length;
		barTime = 60 / (obaMusicData.BPM / 4);

		rowNum = 0;

		for (int i = 1; i <= (rowLength / 2); i++)
		{
			if (i == 1) Debug.Log("real start 1 = " + Time.time);

			MakeOneBar(i, obaMusicData.waitTime, i,obaMusicData);
		}
		return noteList;
	}
	public bool MakeOneBar(int skipBar, float wait, int barNumber,ObaMusicData obaMusicData)
	{
		Vector3 pos, size;
		pos.y = 0;
		size.x = 0.3f;
		size.y = 1;

		Note n;
		NoteMove nm;
		string lineData;//各行のデータを入れる
		GameObject obj;

		if (SearchWord("--") == false)
		{
			return false;
		}
		int startline = rowNum;
		//Debug.Log("startline is " + startline);

		rowNum++;

		if (SearchWord("--") == false)
		{
			return false;
		}
		//Debug.Log("textNum is " + textNum);

		float interval = barTime / (rowNum - startline - 1);//(textNum - startline - 1)は行の数

		float laneWidth = width / obaMusicData.splitLane;

		for (int i = 0; (startline + 1 + i) < rowNum; i++)
		{
			/*Debug.Log("小節の中の" + (i + 1) + "行目");*/
			lineData = (Regex.Replace(splitText[startline + 1 + i], @"[^0-9A-Z]", ""));
			Debug.Log("lineData is" + lineData);
			for (int l = 0; l < obaMusicData.splitLane; l++)
			{
				if (lineData[l] != '0')
				{
					obj = Instantiate(Pref);
					pos.x = (7 - (skipBar * barTime * NoteMaster.speed) - (i * interval * NoteMaster.speed) - (wait * NoteMaster.speed));

					Debug.Log("pos.x = " + pos.x);

					pos.z = (float)(lineData[l] - '1') / 2 * laneWidth + left +( l + 0.5f)* laneWidth;

					obj.transform.position = pos;

					size.z = (float)(lineData[l] - '0')*laneWidth - 0.1f;
					obj.transform.localScale = size;

					nm = obj.GetComponent<NoteMove>();
					nm.SetSpeed(NoteMaster.speed);
					n = obj.GetComponent<Note>();
					n.noteType = GetNoteType(l, lineData[l] - '0');
					n.noteMove = nm;
					n.justTime = obaMusicData.waitTime + (barNumber * barTime) + (i * interval);
					noteList.Add(n);

				}
			}
		}

		return true;

	}
	public bool SearchWord(string str)
	/*文字列を検索し、最初のその文字列が見つかるまでtextNumを進める
	 見つかればtrue,見つからなければfalse*/
	{
		int rowfrom = rowNum;
		if (rowNum >= rowLength)//見つからず最後の行まで行ったら失敗
		{
			return false;
		}
		while (Regex.IsMatch(splitText[rowNum], str) == false)
		{
			rowNum++;
			if (rowNum >= rowLength)//見つからず最後の行まで行ったら失敗
			{

				rowNum = rowfrom;
				return false;
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
		for (int i = 1; left >= i; i++)
		{
			a *= 2;
		}
		return a;
	}

}
