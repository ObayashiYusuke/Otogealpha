using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;

public class NoteMaster : MonoBehaviour
{
	public int life = 8;
	public int score = 0;
	public float speed = 10;
	public GameObject Pref;

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


	private float BPM = 1;
	private float barTime;

	private IEnumerator Test1Coloutine()
	{
		Debug.Log("Test1Coloutine開始");
		yield return new WaitForSeconds(0.1f);

	}

	private IEnumerator Test2Coroutine()
	{
		



		Debug.Log("Test2Coloutine開始");

		fumenAllText = (Resources.Load("Test2", typeof(TextAsset)) as TextAsset).text;//テキストの読み込み

		splitText = fumenAllText.Split(char.Parse("\n"));//テキストを改行ごとに分ける
		rowLength = fumenAllText.Split('\n').Length;

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
		for (int i = 1; i <= 5; i++)
		{
			Debug.Log(i + "番目の生成");
			MakeOneBar(i);
		}

		yield return new WaitForSeconds(barTime);

		while(textNum < rowLength)
		{
			MakeOneBar(5);
			yield return new WaitForSeconds(barTime);
		}
		yield return null;
		/*while (a > 0)
		{
			Debug.Log("あと" + a + "個生成");
			var obj = Instantiate(Pref);
			
			pos.x = -10;
			pos.y = 0;
			pos.z = (a % 4); 
			obj.transform.position = pos;
			n = obj.GetComponent<Note>();

			n.noteType = (a % 4 == 0) ? NoteType.POS_S :
						(a % 4 == 1) ? NoteType.POS_F :
						(a % 4 == 2) ? NoteType.POS_J : NoteType.POS_L;

			yield return Test1Coloutine();
			a--;
		}過去の遺産*/


	}

	public float GetBPM()		//BPMを取得する関数!!!!!!!
	{
		/*while (Regex.IsMatch(splitText[textNum], "bpm") == false)//bpmが出でくるまで行を進める
		{
			textNum++;
			if (textNum >= rowLength)//見つからず最後の行まで行ったら-1を返す
			{
				return -1;
			}
		}*/
		if (SearchWord("bpm") == false)
		{
			return -1;
		}
		if (splitText[textNum].Split('.').Length > 2)//小数点が2個以上ある場合を例外処理
		{
			return -1;
		}
		return float.Parse(Regex.Replace(splitText[textNum], @"[^0-9.]", ""));//BPMをフロート型にして返す

	}

	public bool MakeOneBar(int skipBar)
	{
		Vector3 pos,size;
		pos.y = 0;
		size.x = 1;
		size.y = 1;

		Note n;
		string lineData;
		GameObject obj;

		if(SearchWord("--") == false)
		{
			return false;
		}
		int startline = textNum;
		Debug.Log("startline is " + startline);

		textNum++;

		if (SearchWord("--") == false)
		{
			return false;
		}
		Debug.Log("textNum is " + textNum);

		float interval = barTime / (textNum - startline - 1);

		for(int i = 0; (startline + 1 + i ) < textNum; i++)
		{
			Debug.Log("小節の中の" + (i + 1) + "行目");
			lineData = (Regex.Replace(splitText[startline + 1 + i], @"[^0-9A-Z]",""));
			Debug.Log("lineData is" + lineData);
			
			if(lineData[0] != '0')
			{
				obj = Instantiate(Pref);
				pos.x = (7 - (skipBar * barTime * speed) - (i * interval * speed));
				pos.z = (float)(lineData[0] - '1') / 2 + 0;
				
				obj.transform.position = pos;
				
				size.z = (float)(lineData[0] - '0') - 0.04f;
				obj.transform.localScale = size;

				n = obj.GetComponent<Note>();
				n.noteType = (lineData[0] == '1') ? NoteType.POS_S :
							(lineData[0] == '2') ? NoteType.POS_S | NoteType.POS_F :
							(lineData[0] == '3') ? NoteType.POS_S | NoteType.POS_F | NoteType.POS_J :
							 NoteType.POS_S | NoteType.POS_F | NoteType.POS_J | NoteType.POS_L;
			}
			if (lineData[1] != '0')
			{
				obj = Instantiate(Pref);
				pos.x = (7 - (skipBar * barTime * speed) - (i * interval * speed));
				pos.z = (float)(lineData[1] - '1')/ 2 + 1;

				obj.transform.position = pos;

				size.z = (float)(lineData[1] - '0') - 0.04f;
				obj.transform.localScale = size;

				n = obj.GetComponent<Note>();
				n.noteType = (lineData[1] == '1') ? NoteType.POS_F :
							(lineData[1] == '2') ? NoteType.POS_F | NoteType.POS_J :
							 NoteType.POS_F | NoteType.POS_J | NoteType.POS_L;
			}
			if (lineData[2] != '0')
			{
				obj = Instantiate(Pref);
				pos.x = (7 - (skipBar * barTime * speed) - (i * interval * speed));
				pos.z = (float)(lineData[2] - '1') / 2 + 2;

				obj.transform.position = pos;

				size.z = (float)(lineData[2] - '0') - 0.04f;
				obj.transform.localScale = size;

				n = obj.GetComponent<Note>();
				n.noteType = (lineData[2] == '1') ? NoteType.POS_J :
							 NoteType.POS_J | NoteType.POS_L;
			}
			if (lineData[3] != '0')
			{
				obj = Instantiate(Pref);
				pos.x = (7 - (skipBar * barTime * speed) - (i * interval * speed));
				pos.z = (float)(lineData[3] - '1') / 2 + 3;

				obj.transform.position = pos;

				size.z = (float)(lineData[3] - '0') - 0.04f;
				obj.transform.localScale = size;

				n = obj.GetComponent<Note>();
				n.noteType = NoteType.POS_L;
			}


		}

		return true;


	}

	public bool SearchWord(string str)
		/*文字列を検索し、最初のその文字列が見つかるまでtextNumを進める
		 見つかればtrue,見つからなければfalse*/
	{
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
				return false;
			}
		}
		return true;
	}

	// Start is called before the first frame update
	void Start()
    {
		StartCoroutine(Test2Coroutine());

		

	}

    // Update is called once per frame
    void Update()
    {
		scoreText.text = "Score : " + score.ToString();
		lifeText.text = "Life : " + life.ToString();
	}
}
