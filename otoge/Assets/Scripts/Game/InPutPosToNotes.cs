using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputPosToNotes : MonoBehaviour
{
	public const int KeyNum = 12;

	public NoteMaster noteMaster;
	Vector3 worldPos;
	public float getLength = 5.0f;
	public GameObject judgeLine;
	public GameObject leftLine;
	public GameObject rightLine;
	private float left,width;
	private Vector3 judgeLinePos, judgeLineScale;
	private float nowTime;
    void Start()
    {
		left = leftLine.transform.position.z;
		width = rightLine.transform.position.z - leftLine.transform.position.z;
		judgeLinePos = judgeLine.transform.position;
		judgeLineScale = judgeLine.transform.localScale;
		judgeLinePos.z = left;
		judgeLineScale.z = width;

	}

    // Update is called once per frame
    void Update()
    {
		if (NoteMaster.state == NoteMaster.State.playing)
		{
			nowTime = Time.time - NoteMaster.noteMakeTime - (NoteMaster.realWait - (60 / (NoteMaster.musicData.BPM / 4)));//今の時間を送れた時間分引く
			MouseInputCheck();
			KeyInputCheck();
			TouchInputCheck();

		}
    }
	public void MouseInputCheck()
	{
		if (Input.GetMouseButtonDown(0))
		{
			worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition + Camera.main.transform.forward);
			Debug.Log(worldPos.x + ", " + worldPos.y + ", " + worldPos.z);
			InputJudge(worldPos.z);

		}
	}
	public void KeyInputCheck()
	{
		if (Input.GetKeyDown(KeyCode.Q))
		{
			InputJudge(left + (1 - 0.5f) * width / KeyNum);
		}

		if (Input.GetKeyDown(KeyCode.W))
		{
			InputJudge(left + (2 - 0.5f) * width / KeyNum);
		}
		if (Input.GetKeyDown(KeyCode.E))
		{
			InputJudge(left + (3 - 0.5f) * width / KeyNum);
		}
		if (Input.GetKeyDown(KeyCode.R))
		{
			InputJudge(left + (4 - 0.5f) * width / KeyNum);
		}
		if (Input.GetKeyDown(KeyCode.T))
		{
			InputJudge(left + (5 - 0.5f) * width / KeyNum);
		}
		if (Input.GetKeyDown(KeyCode.Y))
		{
			InputJudge(left + (6 - 0.5f) * width / KeyNum);
		}
		if (Input.GetKeyDown(KeyCode.U))
		{
			InputJudge(left + (7 - 0.5f) * width / KeyNum);
		}
		if (Input.GetKeyDown(KeyCode.I))
		{
			InputJudge(left + (8 - 0.5f) * width / KeyNum);
		}
		if (Input.GetKeyDown(KeyCode.O))
		{
			InputJudge(left + (9 - 0.5f) * width / KeyNum);
		}
		if (Input.GetKeyDown(KeyCode.P))
		{
			InputJudge(left + (10 - 0.5f) * width / KeyNum);
		}
		if (Input.GetKeyDown(KeyCode.BackQuote))
		{
			InputJudge(left + (11 - 0.5f) * width / KeyNum);
		}
		if (Input.GetKeyDown(KeyCode.LeftBracket))
		{
			InputJudge(left + (12 - 0.5f) * width / KeyNum);
		}

	}

	public void TouchInputCheck()
	{
		Touch touchPos2;
		Vector3 touchPos3;
		int count = Input.touchCount;
		if(count > 0)
		{
			for(int i = 0; i < count; i++)
			{
				touchPos2 = Input.GetTouch(i);
				if (touchPos2.phase == TouchPhase.Began)
				{
					touchPos3.x = touchPos2.position.x;
					touchPos3.y = touchPos2.position.y;
					touchPos3.z = 0f;
					worldPos = Camera.main.ScreenToWorldPoint(touchPos3 + Camera.main.transform.forward);
					InputJudge(worldPos.z);
				}
			}
			
		}
	}
	public void InputJudge(float inputPos)
	{
		for (int i = 0; i < NoteMaster.musicData.noteList.Count; i++)
		{
			Vector3 pos = NoteMaster.musicData.noteList[i].gameObject.transform.position;
			Vector3 size = NoteMaster.musicData.noteList[i].gameObject.transform.localScale;
			if (judgeLine.transform.position.x - getLength < pos.x && pos.x < judgeLine.transform.position.x + getLength)//縦幅
			{
				if (pos.z + (size.z / 2) > inputPos && inputPos > pos.z - (size.z / 2))
				{
					if (NoteMaster.musicData.noteList[i].justTime + NoteMaster.greatJudge > nowTime && nowTime > NoteMaster.musicData.noteList[i].justTime - NoteMaster.greatJudge)
					{
						NoteMaster.great++;
						NoteMaster.score += 100;
						noteMaster.JudgeTextRewrite("GREAT");
						Destroy(NoteMaster.musicData.noteList[i].gameObject);
						NoteMaster.musicData.noteList.RemoveAt(i);
					}
					else if (NoteMaster.musicData.noteList[i].justTime + NoteMaster.goodJudge > nowTime && nowTime > NoteMaster.musicData.noteList[i].justTime)
					{
						NoteMaster.late++;
						NoteMaster.score += 50;
						noteMaster.JudgeTextRewrite("LATE");
						Destroy(NoteMaster.musicData.noteList[i].gameObject);
						NoteMaster.musicData.noteList.RemoveAt(i);
					}
					else if (NoteMaster.musicData.noteList[i].justTime > nowTime && nowTime > NoteMaster.musicData.noteList[i].justTime - NoteMaster.goodJudge)
					{
						NoteMaster.fast++;
						NoteMaster.score += 50;
						noteMaster.JudgeTextRewrite("FAST");
						Destroy(NoteMaster.musicData.noteList[i].gameObject);
						NoteMaster.musicData.noteList.RemoveAt(i);
					}

				}
			}
			else if (pos.x < judgeLine.transform.position.x - getLength)
			{
				break;
			}
		}
	}
}
