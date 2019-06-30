using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JudgeLine : MonoBehaviour
{
	public NoteMaster noteMaster;
	public int inputBuffer;
	// Start is called before the first frame update home
	void Start()
	{

	}




	// Update is called once per frame
	void Update()
	{

	}

	private void OnTriggerStay(Collider other)
	{
		Note n = other.gameObject.GetComponent<Note>();
		if ((inputBuffer & (int)n.noteType) != 0)
		{
			Destroy(other.gameObject);
			noteMaster.score += 100;
		}
	}

	private void OnTriggerExit(Collider other)
	{
		other.gameObject.GetComponent<Renderer>().material.color = new Color(0, 0, 254);
		if (noteMaster.life > 0)
		{
			noteMaster.life -= 1;
		}
	}

}
