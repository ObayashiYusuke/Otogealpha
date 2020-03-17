using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//各オブジェクトが消えるときの処理を行うクラス GUI上でノーツオブジェクトに付与
public class NoteFadeOut : MonoBehaviour
{
	public float fadeOutSpeed = 2;
	private bool isFadeOut = false;
	private bool isFadeOutFinished = false;
	private bool isDelete = false;
	private Color color;
	private float beforeTime;
    // Start is called before the first frame update
    void Start()
    {
		color = gameObject.GetComponent<Renderer>().material.color;
		StartCoroutine("FadeOut");
    }

    // Update is called once per frame
    void Update()
    {

		if ((isDelete && this.transform.position.x > 10) || isFadeOutFinished)//フェードアウトが終わるかオブジェクトが流れ去ったら
		{
			Destroy(this.gameObject);//オブジェクトを消す
		}

	}

	public void StartFadeOut()//外部からフェードアウトの開始を行う時に呼び出すメソッド
	{
		isFadeOut = true;
	}
	public void DeleteFlagSet()//外部からオブジェクトの削除を行う時に呼び出すメソッド
	{
		isDelete = true;
	}
	private IEnumerator FadeOut()
	{
		while (true)
		{
			if (isFadeOut)//フェードアウトを行うかの信号がtrueになったら
			{
				beforeTime = Time.time;
				yield return null;
				float deltaTime = Time.time - beforeTime;//1フレーム前からの経過時間
				Debug.Log("deltaTime=" + deltaTime);
				color.a -= fadeOutSpeed * deltaTime;//経過時間分透明度を下げる
				gameObject.GetComponent<Renderer>().material.color = color;
				//Debug.Log("透明度=" + gameObject.GetComponent<Renderer>().material.color.a);
				if (color.a <= 0f)//完全に消えたら
				{
					isFadeOutFinished = true;//フェードアウト終了フラグを1に
					yield break;
				}
			}
			else
			{
				yield return null;
			}
			beforeTime = Time.time;
		}
	}
}
