﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectButton : MonoBehaviour
{
	public string NoteDataName;

    public void OnClick()
	{
		NoteMaster.buttonnumber = 4;
		NoteMaster.noteData = NoteDataName;
		NoteMaster.state = 1;
		GameObject master = GameObject.Find("noteMaster");
		NoteMaster noteMaster = master.GetComponent<NoteMaster>();
		noteMaster.GoToGame();
	}
}