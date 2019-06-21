using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class NoteMasterRand : MonoBehaviour
{

	public GameObject Pref;

	public int life = 8;
	public int score = 0;
	public Text scoreText;
	public Text lifeText;

	/* private IEnumerator Test1Coloutine()
	{
		Debug.Log("Test1Coloutine開始");
		yield return new WaitForSeconds(0.1f);

	}*/

	private IEnumerator Test2Coroutine()
	{
		float notekey;
		int speed = 10;//降ってくる密度　少ないほど早い
		int count = 8;

		GameObject obj;

		Vector3 pos;

		Note n;

		Debug.Log("Test2Coloutine開始");
		while (true)
		{
			notekey = Random.value;//変数を一つ抽出

			if(notekey > 0.5f)
			{
				obj = Instantiate(Pref);
				pos.x = -10;
				pos.y = 0;
				pos.z = 0;
				obj.transform.position = pos;
				n = obj.GetComponent<Note>();
				n.noteType = NoteType.POS_S;

				notekey -= 0.5f;
			}
			if(notekey > 0.25f)
			{
				obj = Instantiate(Pref);
				pos.x = -10;
				pos.y = 0;
				pos.z = 1;
				obj.transform.position = pos;
				n = obj.GetComponent<Note>();
				n.noteType = NoteType.POS_F;

				notekey -= 0.25f;
			}
			if(notekey > 0.125f)
			{
				obj = Instantiate(Pref);
				pos.x = -10;
				pos.y = 0;
				pos.z = 2;
				obj.transform.position = pos;
				n = obj.GetComponent<Note>();
				n.noteType = NoteType.POS_J;

				notekey -= 0.125f;
			}
			if(notekey > 0.06125f)
			{
				obj = Instantiate(Pref);
				pos.x = -10;
				pos.y = 0;
				pos.z = 3;
				obj.transform.position = pos;
				n = obj.GetComponent<Note>();
				n.noteType = NoteType.POS_L;

			}

			count--;
			if(count <= 0)
			{
				if(speed > 1)
				{
					speed--;
				}
				if (speed > 7)
				{
					count = 8;
				}
				else if (speed > 4)
				{
					count = 16;
				}
				else if (speed > 2)
				{
					count = 24;
				}
				else
					count = 100;
			}

			for (int i = 1; i <= speed; i++) {
				yield return new WaitForSeconds(0.1f);
			}
			if(life <= 0)
			{
				break;
			}
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
		scoreText.text = "Score : " + score.ToString();
		lifeText.text = "Life : " + life.ToString();
	}
}
