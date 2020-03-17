using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*オブジェクトが流れる速度を設定するクラス　セレクト画面でスピードを調整するボタンにインスタンスを持たせる*/
public class SpeedChange : MonoBehaviour
{
	public int changeValue;
    // Start is called before the first frame update
    public void OnClick()
	{
		NoteMaster.speed += (float)changeValue;
	}
}
