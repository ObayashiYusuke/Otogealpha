﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedChange : MonoBehaviour
{
	public int changeValue;
    // Start is called before the first frame update
    public void OnClick()
	{
		NoteMaster.speed += (float)changeValue;
	}
}
