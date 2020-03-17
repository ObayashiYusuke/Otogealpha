using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*TouchRecordリストから実際に記録する文字列データに変換するクラス*/
public class TouchRecordToStr : MonoBehaviour
{
    // Start is called before the first frame update
	public static string TouchRecordToString(List<TouchRecord> touchRecords)
	{
		string str, fullString;
		fullString = string.Format("{0,5}{1,5}{2,10}\n", "x", "y", "pushtime");
		foreach (TouchRecord r in touchRecords)
		{
			str = string.Format("{0,5}{1,5}{2,10:f3}\n", r.posx, r.posy, r.pushTime);
			fullString += str;
		}
		Debug.Log(fullString);
		return fullString;
	}
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
