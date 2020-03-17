using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*1つの譜面のプレイを行うために必要な情報をひとまとめにしたクラス*/
public class MusicData
{

	
	public string musicName;//曲のファイル名
	public float BPM;
	public float waitTime;//曲の再生と同時に生成されるオブジェクトを遅らせる時間
	public float endTime;//曲開始から終了までの時間
	public List<Note> noteList;
    
}
public class ObaMusicData : MusicData
{
	public string noteData;//譜面の文字列データ
	public string assistDataName;//カラーガイドのデータのファイル名
	public string assistData;//カラーガイドデータの本文
	public int splitLane;//レーン分割数(画面を縦に何分割して使用するか)
}
