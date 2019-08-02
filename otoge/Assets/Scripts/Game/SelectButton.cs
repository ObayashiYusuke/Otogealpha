using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectButton : MonoBehaviour
{
	public string NoteDataName;
	public int spd;

    public void OnClick()
	{
		NoteMaster.buttonnumber = 4;
		NoteMaster.noteData = NoteDataName;
		
		GameObject master = GameObject.Find("noteMaster");
		NoteMaster noteMaster = master.GetComponent<NoteMaster>();
		noteMaster.speed = spd;
		noteMaster.GoToGame();
	}
}
