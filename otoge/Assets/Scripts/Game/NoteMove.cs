using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteMove : MonoBehaviour
{

	public float dx;
	//public NoteMaster noteMaster;

	// Start is called before the first frame update
	void Start()
	{
		//dx = noteMaster.speed;

	}

	// Update is called once per frame
	void Update()
	{

		this.transform.position += new Vector3(1 * dx * Time.deltaTime, 0, 0);//等速移動








	}




}