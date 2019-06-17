using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteMaster : MonoBehaviour
{
	public int a = 4;
	public GameObject Pref;

	private IEnumerator Test1Coloutine()
	{
		Debug.Log("Test1Coloutine開始");
		yield return new WaitForSeconds(0.1f);

	}

	private IEnumerator Test2Coroutine()
	{
		Vector3 pos;

		Note n;

		Debug.Log("Test2Coloutine開始");
		while (a > 0)
		{
			Debug.Log("あと" + a + "個生成");
			var obj = Instantiate(Pref);
			
			pos.x = -10;
			pos.y = 0;
			pos.z = (a % 4); 
			obj.transform.position = pos;
			n = obj.GetComponent<Note>();

			n.noteType = (a % 4 == 0) ? NoteType.POS_S :
						(a % 4 == 1) ? NoteType.POS_F :
						(a % 4 == 2) ? NoteType.POS_J : NoteType.POS_L;

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
