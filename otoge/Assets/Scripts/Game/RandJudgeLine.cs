using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandJudgeLine : MonoBehaviour
{
	public RandNoteMaster randNoteMaster;
	public int inputBuffer;
    // Start is called before the first frame update home
    void Start()
    {
        
    }

	private void FixedUpdate()
	{
		inputBuffer = 0;
		/*if (Input.GetKeyDown(KeyCode.S))
		{
			inputBuffer |= (int)NoteType.POS_S; 
		}

		if (Input.GetKeyDown(KeyCode.F))
		{
			inputBuffer |= (int)NoteType.POS_F;
		}
		if (Input.GetKeyDown(KeyCode.J))
		{
			inputBuffer |= (int)NoteType.POS_J;
		}
		if (Input.GetKeyDown(KeyCode.L))
		{
			inputBuffer |= (int)NoteType.POS_L;
		}
		*/

	}

	// Update is called once per frame
	void Update()
    {
        
    }

	private void OnTriggerStay(Collider other)
	{
		Note n = other.gameObject.GetComponent<Note>();
		if((inputBuffer & (int)n.noteType) != 0)
		{
			Destroy(other.gameObject);
			randNoteMaster.score += 100;
		}
	}

	private void OnTriggerExit(Collider other)
	{
		other.gameObject.GetComponent<Renderer>().material.color = new Color(0, 0, 254);
		if (randNoteMaster.life > 0)
		{
			randNoteMaster.life -= 1;
		}
	}

}
