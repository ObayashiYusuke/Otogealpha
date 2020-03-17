using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*Titleボタンが呼び出す、ゲームの画面をタイトル画面に戻すクラス*/
public class GoToReturn : MonoBehaviour
{
	public void OnClick()
	{

		GameObject master = GameObject.Find("noteMaster");
		NoteMaster noteMaster = master.GetComponent<NoteMaster>();
		noteMaster.GoToTitle();//noteMaster内のタイトル画面に遷移する処理を呼び出す
		Debug.Log("GoToTitle Finished");
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
