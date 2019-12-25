using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoToReturn : MonoBehaviour
{
	public void OnClick()
	{

		GameObject master = GameObject.Find("noteMaster");
		NoteMaster noteMaster = master.GetComponent<NoteMaster>();
		noteMaster.GoToTitle();
		Debug.Log("GoToTitle Finished");
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
