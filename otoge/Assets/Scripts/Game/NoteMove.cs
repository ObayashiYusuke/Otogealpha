using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*オブジェクトの動きを制御するクラス*/
public class NoteMove : MonoBehaviour
{

	private float dx;//移動速度
	private bool move = true;//移動するかどうかのフラグ

	// Start is called before the first frame update

	public void SetSpeed(float x)//外部から速度を変更するときのメソッド
	{
		dx = x;
	} 

	public void StartMove()//外部から移動を開始するときのメソッド
	{
		move = true;
	}
	public void StopMove()//外部から移動を停止するときのメソッド
	{
		move = false;
	}
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{

		if (move == true)
		{
			this.transform.position += new Vector3(1 * dx * Time.deltaTime, 0, 0);//等速移動
		}
	}




}