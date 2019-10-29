using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecordsToStr : MonoBehaviour
{

	public static string RecordsToString(List<Record> records)
	{
		string str,fullString;
		fullString = string.Format("{0,8}{1,10}{2,10}{3,8}\n","noteNum","justTime","hitTime","judge");
		foreach(Record r in records)
		{
			str = string.Format("{0,8}{1,10}{2,10:f3}{3,8}\n", r.noteNum, r.justTime, r.hitTime, r.judgeGrade);
			fullString += str;
		}
		Debug.Log(fullString);
		return fullString;

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
