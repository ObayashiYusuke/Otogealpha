using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedChange : MonoBehaviour
{
	public int changeValue;
    // Start is called before the first frame update
    public void OnClick()
	{
		GameObject master = GameObject.Find("noteMaster");
		NoteMaster noteMaster = master.GetComponent<NoteMaster>();
		noteMaster.speed += (float)changeValue;
	}
}
