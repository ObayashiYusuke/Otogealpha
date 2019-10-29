using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Record : MonoBehaviour
{
	public int noteNum;
	public float justTime;
	public float hitTime;
	public string judgeGrade;

	public Record(int noteNum, float justTime, float hitTime,string judgeGrade)
	{
		this.noteNum = noteNum;
		this.justTime = justTime;
		this.hitTime = hitTime;
		this.judgeGrade = judgeGrade;
	}
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
