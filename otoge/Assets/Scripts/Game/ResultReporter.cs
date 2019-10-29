﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class ResultReporter : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	public static bool SaveText(string text,string fileName)
	{
		try
		{
			using (StreamWriter writer = new StreamWriter(Path.Combine(Application.persistentDataPath,fileName), true))
			{
				writer.Write(text);
				writer.Flush();
				writer.Close();
			}
		}catch(Exception e)
		{
			Debug.Log(e.Message);
			return false;
		}
		Debug.Log(Path.Combine(Application.persistentDataPath, fileName));
		return true;
	}
}