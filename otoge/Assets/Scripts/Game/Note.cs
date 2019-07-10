using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*public enum NoteType
{
		POS_S = 1,
		POS_F = 2,
		POS_J = 4,
		POS_L = 8,
}*/

public class Note : MonoBehaviour
{

	public int noteType;
	public NoteMove noteMove;
	public float time;
	private float goodTime = 10;//消えるまでの時間
	private NoteMaster notemaster;

	private void Update()
	{
		if(Time.time - NoteMaster.starttime - goodTime > time && NoteMaster.starttime != 0 )
		{
			NoteMaster.miss++;
			Destroy(gameObject);
		}
	}

	public void SetGoodTime(float x)
	{
		goodTime = x;
	}
}
