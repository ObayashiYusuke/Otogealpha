using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteMaster : MonoBehaviour
{
	public int a = 3;
	public GameObject Pref;

	private IEnumerator Test1Coloutine()
	{
		Debug.Log("Test1Coloutine開始");
		yield return new WaitForSeconds(1f);
		Debug.Log("3秒経過");
	}

	private IEnumerator Test2Coroutine()
	{
		
		Debug.Log("Test2Coloutine開始");
		while (a > 0)
		{
			Debug.Log("あと" + a + "個生成");
			var obj = Instantiate(Pref);
			yield return Test1Coloutine();
			a--;
		}

	}

	// Start is called before the first frame update
	void Start()
    {
		StartCoroutine(Test2Coroutine());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
