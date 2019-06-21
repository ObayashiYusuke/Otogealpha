using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;

public class NoteMaster : MonoBehaviour
{
	public int a = 4;//テスト生成個数
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

	private float BPM = 1;

	private IEnumerator Test1Coloutine()
	{
		Debug.Log("Test1Coloutine開始");
		yield return new WaitForSeconds(0.1f);

	}

	private IEnumerator Test2Coroutine()
	{
		Vector3 pos;

		Note n;



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
		yield return new WaitForSeconds(1f);

	}

	public float GetBPM()
	{
		while (Regex.IsMatch(splitText[textNum], "bpm") == false)//bpmが出でくるまで行を進める
		{
			textNum++;
			if (textNum >= rowLength)//見つからず最後の行まで行ったら-1を返す
			{
				return -1;
			}
		}
		if (splitText[textNum].Split('.').Length > 2)
		{
			return -1;
		}
		return float.Parse(Regex.Replace(splitText[textNum], @"[^0-9.]", ""));//BPMをフロート型にして返す

	}

	// Start is called before the first frame update
	void Start()
    {
		StartCoroutine(Test2Coroutine());

		

	}

    // Update is called once per frame
    void Update()
    {
        
    }
}
