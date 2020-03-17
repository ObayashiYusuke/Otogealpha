using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*プレイ中の1つのタッチ入力の情報を記録するのに必要な情報をまとめたクラス*/
public class TouchRecord : MonoBehaviour
{
	public float posx, posy;/*タッチされた位置*/
	public float pushTime;/*タッチされた時間*/
    // Start is called before the first frame update
	public TouchRecord(float x,float y,float time)
	{
		posx = x;
		posy = y;
		pushTime = time;
	}
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
