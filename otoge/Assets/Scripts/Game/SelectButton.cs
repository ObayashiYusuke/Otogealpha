using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*セレクト画面のゲーム画面に移行するボタンがインスタンスを持つクラス　押されたらゲーム画面に移行する*/
public class SelectButton : MonoBehaviour
{
	public string noteDataName;//譜面データの名前


    public void OnClick()//このボタンが押されたら
	{
		NoteMaster.noteDataName = noteDataName;//譜面データの名前を選ばれた内容に設定する
		
		GameObject master = GameObject.Find("noteMaster");
		NoteMaster noteMaster = master.GetComponent<NoteMaster>();
		noteMaster.GoToGame();//ゲーム開始画面に遷移する
	}
}
