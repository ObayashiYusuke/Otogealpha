using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteFadeOut : MonoBehaviour
{
	public float fadeOutSpeed = 2;
	private bool isFadeOut = false;
	private bool isFadeOutFinished = false;
	private bool isDelete = false;
	private Color color;
	private float beforeTime;
    // Start is called before the first frame update
    void Start()
    {
		color = gameObject.GetComponent<Renderer>().material.color;
		StartCoroutine("FadeOut");
    }

    // Update is called once per frame
    void Update()
    {

		if ((isDelete && this.transform.position.x > 10) || isFadeOutFinished)
		{
			Destroy(this.gameObject);
		}

	}

	public void StartFadeOut()
	{
		isFadeOut = true;
	}
	public void DeleteFlagSet()
	{
		isDelete = true;
	}
	private IEnumerator FadeOut()
	{
		while (true)
		{
			if (isFadeOut)
			{
				beforeTime = Time.time;
				yield return null;
				float deltaTime = Time.time - beforeTime;
				Debug.Log("deltaTime=" + deltaTime);
				color.a -= fadeOutSpeed * deltaTime;
				gameObject.GetComponent<Renderer>().material.color = color;
				//Debug.Log("透明度=" + gameObject.GetComponent<Renderer>().material.color.a);
				if (color.a <= 0f)
				{
					isFadeOutFinished = true;
					yield break;
				}
			}
			else
			{
				yield return null;
			}
			beforeTime = Time.time;
		}
	}
}
