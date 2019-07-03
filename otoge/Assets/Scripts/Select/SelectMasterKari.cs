using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class SelectMasterKari : MonoBehaviour
{

	public static string noteName = "Test4simple";
	// Start is called before the first frame update

	private IEnumerator ChoiceKari()
	{
		noteName = "Test4simple";
		Vector3 pos = this.gameObject.transform.position;
		while (Input.GetKeyDown(KeyCode.Return) == false)
		{
			if (Input.GetKeyDown(KeyCode.S))
			{
				noteName = "Test4simple";
				pos.x = -6;
				this.gameObject.transform.position = pos;
			}
			else if (Input.GetKeyDown(KeyCode.F))
			{
				noteName = "Test4A";
				pos.x = -2;
				this.gameObject.transform.position = pos;
			}
			yield return null;
		}
		yield return new WaitForSeconds(3.0f);
		SceneManager.LoadScene("MusicGame");
		yield return null;
	}
	void Start()
    {
		StartCoroutine(ChoiceKari());
    }

    // Update is called once per frame
    void Update()
    {

    }
}
