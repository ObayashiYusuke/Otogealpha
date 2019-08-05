using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;
public class NoteDataParser
{
	
	public static MusicData NoteDataParse(string noteDataName)
	{
		string noteData;//譜面データの全文
		float BPM;//取得したBPM
		float waitTime;
		float endTime;
		string musicName;
		string[] splitText;//行ごとに分けた文字列
		int buttonNumber;
		int rowLength;//データ全体の行数
		MusicData musicData = new MusicData();
		noteData = (Resources.Load("NoteData/" + noteDataName, typeof(TextAsset)) as TextAsset).text;//テキストの読み込み
		splitText = noteData.Split(char.Parse("\n"));//テキストを改行ごとに分ける
		rowLength = noteData.Split('\n').Length;
		BPM = GetBPM(splitText,rowLength);
		waitTime = GetWaittime(splitText,rowLength);
		endTime = GetEndtime(splitText, rowLength);
		musicName = GetMusicName(splitText, rowLength);
		buttonNumber = GetButtonNumber(splitText, rowLength);
		musicData.noteData = noteData;
		Debug.Log("musicData.noteData=" + musicData.noteData);
		musicData.musicName = musicName;
		Debug.Log("musicData.musicName=" + musicData.musicName);
		musicData.BPM = BPM;
		Debug.Log("musicData.BPM=" + musicData.BPM);
		musicData.waitTime = waitTime;
		Debug.Log("musicData.waitTime=" + musicData.waitTime);
		musicData.endTime = endTime;
		Debug.Log("musicData.endTime=" + musicData.endTime);
		musicData.playTime = endTime + waitTime;
		Debug.Log("musicData.playTime=" + musicData.playTime);
		musicData.buttonNum = buttonNumber;
		Debug.Log("musicData.buttonNum=" + musicData.buttonNum);

		return musicData;
	}

	public static float GetBPM(string[] splitText,int rowLength)       //BPMを取得する関数!!!!!!! bpm=oo で記述
	{
		int rowNum = SearchWord("bpm=", splitText,rowLength);
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

	public static float GetWaittime(string[] splitText,int rowLength)      //譜面再生までの時間を取得 waittime=oo で記述
	{
		int rowNum = SearchWord("waittime=", splitText,rowLength);
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
		int rowNum = SearchWord("endtime=", splitText,rowLength);
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
		int rowNum = SearchWord("musicname=",splitText,rowLength);
		if (rowNum == -1)//曲のファイル名を取得 musicname=ooで記述
		{
			return null;
		}
		return Regex.Replace(splitText[rowNum], "musicname=", "");//musicNameを返す

	}

	public static int GetButtonNumber(string[] splitText, int rowLength)//ボタン数を取得buttonnumber=で記述
	{
		int rowNum = SearchWord("buttonnumber=", splitText,rowLength);
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
	public static int SearchWord(string str,string[] splitText,int rowLength)
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
}
