using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*プレイ中のログを取るために、各ノートに対する判定等の記録する情報を1ノート分まとめたクラス*/
public class Record : MonoBehaviour
{
	public int noteNum;//そのノートが譜面の中で何番目か
	public float justTime;//ゲーム開始から操作されるべき時間ちょうどまでの時間
	public float hitTime;//実際にそのオブジェクトがタッチされた時間
	public string judgeGrade;//その時の判定

	public Record(int noteNum, float justTime, float hitTime,string judgeGrade)
	{
		this.noteNum = noteNum;
		this.justTime = justTime;
		this.hitTime = hitTime;
		this.judgeGrade = judgeGrade;
	}
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
