using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*各オブジェクトの持つ情報をまとめたクラス*/
public class Note : MonoBehaviour
{

	public int noteType;//ノートの種類
	public float justTime;//ゲーム開始から操作されるまでの時間
	public int noteNum;//その譜面の中で何番目のノーツか
	public NoteMove noteMove;//オブジェクトの動きを制御するインスタンス
}
