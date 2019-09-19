using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InPutPosToNotes : MonoBehaviour
{
	Vector3 worldPos;
	public float getLength = 5.0f;
	public GameObject judgeLine;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
		if (NoteMaster.state == NoteMaster.State.playing)
		{
			if (Input.GetMouseButtonDown(0))
			{
				float nowTime = Time.time - NoteMaster.noteMakeTime - (NoteMaster.realWait - (60 / (NoteMaster.musicData.BPM / 4)));//今の時間を送れた時間分引く

				worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition + Camera.main.transform.forward);
				Debug.Log(worldPos.x + ", " + worldPos.y + ", " + worldPos.z);
				for (int i = 0; i < NoteMaster.noteList.Count; i++)
				{
					Vector3 pos = NoteMaster.noteList[i].gameObject.transform.position;
					Vector3 size = NoteMaster.noteList[i].gameObject.transform.localScale;
					if (judgeLine.transform.position.x - getLength < pos.x && pos.x < judgeLine.transform.position.x + getLength)
					{
						if (pos.z + (size.z / 2) > worldPos.z && worldPos.z > pos.z - (size.z / 2))
						{
							if (NoteMaster.noteList[i].justTime + NoteMaster.greatJudge > nowTime && nowTime > NoteMaster.noteList[i].justTime - NoteMaster.greatJudge)
							{
								NoteMaster.great++;
								NoteMaster.score += 100;
								Debug.Log("Mouse Great");
								Destroy(NoteMaster.noteList[i].gameObject);
								NoteMaster.noteList.RemoveAt(i);
							}
							else if (NoteMaster.noteList[i].justTime + NoteMaster.goodJudge > nowTime && nowTime > NoteMaster.noteList[i].justTime)
							{
								NoteMaster.late++;
								NoteMaster.score += 50;
								Debug.Log("Mouse late");
								Destroy(NoteMaster.noteList[i].gameObject);
								NoteMaster.noteList.RemoveAt(i);
							}
							else if (NoteMaster.noteList[i].justTime > nowTime && nowTime > NoteMaster.noteList[i].justTime - NoteMaster.goodJudge)
							{
								NoteMaster.fast++;
								NoteMaster.score += 50;
								Debug.Log("Mouse Fast");
								Destroy(NoteMaster.noteList[i].gameObject);
								NoteMaster.noteList.RemoveAt(i);
							}

						}
					}
				}
			}
		}
    }
}
