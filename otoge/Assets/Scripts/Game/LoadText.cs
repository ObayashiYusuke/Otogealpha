using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadText : MonoBehaviour
{


	//　読む込むテキストが書き込まれている.txtファイル
	[SerializeField]
	private TextAsset textAsset;
	//　Resourcesフォルダから直接テキストを読み込む
	private string loadText2;
	//　改行で分割して配列に入れる

	private string[] splitText2;
	
	//　現在表示中テキスト2番号
	private int textNum2;
	// Start is called before the first frame update
	void Start()
    {

		loadText2 = (Resources.Load("Test2", typeof(TextAsset)) as TextAsset).text;

		splitText2 = loadText2.Split(char.Parse("\n"));

		textNum2 = 0;
		Debug.Log("????");
		for(textNum2 = 0; textNum2 < splitText2.Length; textNum2++)
		{
			Debug.Log((textNum2 + 1) + "行目" + splitText2[textNum2]);
		}

	}

    // Update is called once per frame
    void Update()
    {
        
    }
}
