using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectButton : MonoBehaviour
{
	public string noteDataName;


    public void OnClick()
	{
		NoteMaster.noteDataName = noteDataName;
		
		GameObject master = GameObject.Find("noteMaster");
		NoteMaster noteMaster = master.GetComponent<NoteMaster>();
		noteMaster.GoToGame();
	}
}
