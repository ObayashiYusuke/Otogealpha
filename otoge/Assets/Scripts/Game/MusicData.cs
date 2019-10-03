using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicData
{

	public string noteData;//譜面の文字列データ
	public string musicName;//曲のファイル名
	public float BPM;
	public float waitTime;//曲の再生と同時に生成されるオブジェクトを遅らせる時間
	public float endTime;//曲開始から終了までの時間
	public List<Note> noteList;
    
}
public class ObaMusicData : MusicData
{
	public int splitLane;
}
