using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchRecord : MonoBehaviour
{
	public float posx, posy;
	public float pushTime;
    // Start is called before the first frame update
	public TouchRecord(float x,float y,float time)
	{
		posx = x;
		posy = y;
		pushTime = time;
	}
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
