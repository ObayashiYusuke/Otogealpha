using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;

public class NoteObjMaker : MonoBehaviour
{
	public GameObject Pref;
	private string[] splitText; 
	private int rowLength;
	private int rowNum;
	private float barTime;
	private List<Note> noteList = new List<Note>();
	// Start is called before the first frame update
	public List<Note> NoteObjMake(MusicData musicData)
	{
		splitText = musicData.noteData.Split(char.Parse("\n"));//テキストを改行ごとに分ける
		rowLength = musicData.noteData.Split('\n').Length;
		barTime = 60 / (musicData.BPM / 4);

		rowNum = 0;

		for (int i = 1; i <= (rowLength / 2); i++)
		{
			Debug.Log(i + "小節目生成");
			if (i == 1) Debug.Log("real start 1 = " + Time.time);

			MakeOneBar(i, musicData.waitTime, i,musicData);
		}
		return noteList;
	}
	public bool MakeOneBar(int skipBar, float wait, int barNumber,MusicData musicData)
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

		for (int i = 0; (startline + 1 + i) < rowNum; i++)
		{
			/*Debug.Log("小節の中の" + (i + 1) + "行目");*/
			lineData = (Regex.Replace(splitText[startline + 1 + i], @"[^0-9A-Z]", ""));
			Debug.Log("lineData is" + lineData);
			for (int l = 0; l < musicData.buttonNum; l++)
			{
				if (lineData[l] != '0')
				{
					obj = Instantiate(Pref);
					pos.x = (7 - (skipBar * barTime * NoteMaster.speed) - (i * interval * NoteMaster.speed) - (wait * NoteMaster.speed));

					Debug.Log("pos.x = " + pos.x);

					pos.z = (float)(lineData[l] - '1') / 2 + l;

					obj.transform.position = pos;

					size.z = (float)(lineData[l] - '0') - 0.1f;
					obj.transform.localScale = size;

					nm = obj.GetComponent<NoteMove>();
					nm.SetSpeed(NoteMaster.speed);
					n = obj.GetComponent<Note>();
					n.noteType = GetNoteType(l, lineData[l] - '0');
					n.noteMove = nm;
					n.time = musicData.waitTime + (barNumber * barTime) + (i * interval);
					n.SetGoodTime(NoteMaster.goodJudge + 0.1f);
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
