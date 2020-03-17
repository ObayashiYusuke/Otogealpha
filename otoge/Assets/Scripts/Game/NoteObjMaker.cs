using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;
/*ノーツオブジェクトを生成するクラス*/
public class NoteObjMaker : MonoBehaviour
{
	public GameObject Pref;//ノーツのプレハブ
	public GameObject leftLine;//レーンの左端のオブジェクト
	public GameObject rightLine;//レーンの右端のオブジェクト
	public GameObject hitLine;//判定ラインのオブジェクト

	public Material[] materials;//オブジェクトに貼り付けるマテリアル

	private string[] splitText; //　改行で分割して配列に入れる
	private int rowLength;//行の総数
	private int rowNum;//現在参照している行の数
	private float barTime;//1小節の時間
	private List<Note> noteList = new List<Note>();//生成したノーツのリスト
	private float width,left,right,hitPos;//オブジェクトの位置や座標を決定するためのレーンの幅、左端、右端、判定ラインの位置
	private ObaMusicData obaMusicData = new ObaMusicData();

	private bool isAssist;//カラーガイドを付加するかどうかのフラグ
	private int noteNum;

	void Start()
	{
		left = leftLine.transform.position.z;
		right = rightLine.transform.position.z;
		hitPos = hitLine.transform.position.x;
		width = right - left;
		Debug.Log("幅は" + width);
	}
	public MusicData NoteDataParse(string noteDataName)
	{
		string noteData;//譜面データの全文

		float BPM;//取得したBPM
		float waitTime;//曲と合わせるために譜面を遅らせる時間
		float endTime;//プレイ終了までの時間
		string musicName;//曲データのファイル名
		string[] splitText;//行ごとに分けた文字列
		int splitLane;
		int rowLength;//データ全体の行数

		Debug.Log("adress is NoteData/" + noteDataName);

		noteData = (Resources.Load("NoteData/" + noteDataName, typeof(TextAsset)) as TextAsset).text;//譜面データの読み込み

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

		if (GetAssistDataName(splitText, rowLength) != null)//譜面データにカラーガイドの記載があったら
		{
			isAssist = true;
			obaMusicData.assistDataName = GetAssistDataName(splitText, rowLength);
			string str = (Resources.Load("AssistData/" + obaMusicData.assistDataName, typeof(TextAsset)) as TextAsset).text;//譜面データの読み込み
			obaMusicData.assistData = Regex.Replace(str, @"[^0-4]", "");
			Debug.Log("assistdata = " + obaMusicData.assistData);

		}
		else
		{
			isAssist = false;
		}
		noteNum = 0;

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

	public static string GetAssistDataName(string[] splitText, int rowLength)
	{
		int rowNum = SearchWord("assistdata=", splitText, rowLength);
		if (rowNum == -1)//曲のアシストデータ名を取得 assistdata=ooで記述
		{
			return null;
		}
		string rmr = Regex.Replace(splitText[rowNum], "\r", "");
		return Regex.Replace(rmr, "assistdata=", "");//assistDataNameを返す

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


	public List<Note> NoteObjMake(ObaMusicData obaMusicData)//実際にオブジェクトを生成する部分
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
	public bool MakeOneBar(int skipBar, float wait, int barNumber,ObaMusicData obaMusicData)//1小節分のオブジェクトを生成する
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
			lineData = (Regex.Replace(splitText[startline + 1 + i], @"[^0-9A-Za-z]", ""));
			Debug.Log("lineData is" + lineData);
			for (int l = 0; l < lineData.Length; l++)
			{
				if ((lineData[l] > '0' && lineData[l] <= '0'+ obaMusicData.splitLane )|| (lineData[l] - 'a' >= 0 && lineData[l] - 'a' + '9' + 1 <= '0' + obaMusicData.splitLane))//数字の範囲の場合 || アルファベットの場合||
				{
					noteNum++;
					obj = Instantiate(Pref);
					pos.x = (hitPos - (skipBar * barTime * NoteMaster.speed) - (i * interval * NoteMaster.speed) - (wait * NoteMaster.speed));//判定ラインから操作されるまでの時間(ここまでの小節分の時間+この小節内でどれくらい待つか+waittime分)

					Debug.Log("pos.x = " + pos.x);

					pos.z = lineData[l] <= '9' ? (float)(lineData[l] - '1') / 2 * laneWidth + left +( l + 0.5f)* laneWidth : (float)(lineData[l] - 'a' + 9) / 2 * laneWidth + left + (l + 0.5f) * laneWidth;//数字:アルファベット 位置とサイズを決定

					obj.transform.position = pos;

					size.z = lineData[l] <= '9'? (lineData[l] - '0')*laneWidth - 0.1f : ((lineData[l] - 'a' + 10) * laneWidth - 0.1f);
					obj.transform.localScale = size;
					Debug.Log("objsize = " + size.z);


					nm = obj.GetComponent<NoteMove>();
					nm.SetSpeed(NoteMaster.speed);
					n = obj.GetComponent<Note>();
					n.noteType = GetNoteType(l, lineData[l] - '0');
					n.noteMove = nm;
					n.justTime = obaMusicData.waitTime + (barNumber * barTime) + (i * interval);
					n.noteNum = noteNum;
					if (isAssist)
					{
						ChangeColor(noteNum, obj);
					}
					noteList.Add(n);

				}else if(lineData[l] != '0')
				{
					break;
				}
			}
		}

		return true;

	}
	private void ChangeColor(int noteNum, GameObject changeColorObject)//オブジェクトのマテリアルを変更するメソッド
	{
		if (noteNum == 0) return;
		if (obaMusicData.assistData.Length >= noteNum){ //アシストデータのそのノーツが何番目かに対応する数字によってマテリアルを変更
			int colorNum = obaMusicData.assistData[noteNum-1] - '1';
			if (colorNum == -1) return;
			changeColorObject.GetComponent<Renderer>().material = materials[colorNum];
		}
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

	public int GetNoteType(int left, int size)//どの位置に判定があるかを記録(キーボード用)
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
