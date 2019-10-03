using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteMove : MonoBehaviour
{

	private float dx;
	private bool move = true;

	// Start is called before the first frame update

	public void SetSpeed(float x)
	{
		dx = x;
	} 

	public void StartMove()
	{
		move = true;
	}
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{

		if (move == true)
		{
			this.transform.position += new Vector3(1 * dx * Time.deltaTime, 0, 0);//等速移動
		}
	}




}