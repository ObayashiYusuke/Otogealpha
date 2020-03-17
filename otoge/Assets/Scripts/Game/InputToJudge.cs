using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/*プレイ中の入力を判定するクラス*/
public class InputToJudge: MonoBehaviour
{
	public const int KeyNum = 12;//キーボードでテストするとき用

	public NoteMaster noteMaster;
	Vector3 worldPos;
	public float getLength = 5.0f;//判定を取るオブジェクトの範囲
	public GameObject judgeLine;//このゲームの判定ラインと左右のラインの位置
	public GameObject leftLine;
	public GameObject rightLine;
	private float left, width;//このゲーム内の操作範囲の左端の位置と幅
	private Vector3 judgeLinePos, judgeLineScale;//判定ラインの位置
	private float nowTime;

	void Start()
	{
		left = leftLine.transform.position.z;//このゲーム内の操作範囲の左端の位置と幅を取得
		width = rightLine.transform.position.z - leftLine.transform.position.z;
		judgeLinePos = judgeLine.transform.position;//判定ラインの位置を取得
		judgeLineScale = judgeLine.transform.localScale;
		judgeLinePos.z = left;
		judgeLineScale.z = width;


	}

	// Update is called once per frame
	void Update()
	{
		if (NoteMaster.state == NoteMaster.State.playing)//ゲームプレイ中のみ動作する
		{
			nowTime = Time.time - NoteMaster.noteMakeTime - (NoteMaster.realWait - (60 / (NoteMaster.musicData.BPM / 4)));//今の時間を送れた時間分引く
			MouseInputCheck();
			KeyInputCheck();
			TouchInputCheck();

		}
	}
	public void MouseInputCheck()//マウスの入力を画面上の座標からオブジェクト空間の座標に変換し判定メソッド(InputJudge)を呼び出す
	{
		if (Input.GetMouseButtonDown(0))
		{
			worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition + Camera.main.transform.forward);
			Debug.Log(worldPos.x + ", " + worldPos.y + ", " + worldPos.z);
			InputJudge(worldPos.z);//3次元情報からオブジェクト空間内の位置情報へ

		}
	}
	public void KeyInputCheck()//キーボード入力をオブジェクト空間の座標に変換し判定メソッド(InputJudge)を呼び出す
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

	public void TouchInputCheck()//画面タッチ入力を画面上の座標からオブジェクト空間の座標に変換し判定メソッド(InputJudge)を呼び出す
	{
		Touch touchPos2;//タッチの入力情報
		Vector3 touchPos3;
		int count = Input.touchCount;//現在のタッチ入力の数
		if (count > 0)
		{
			for (int i = 0; i < count; i++)
			{
				touchPos2 = Input.GetTouch(i);
				if (touchPos2.phase == TouchPhase.Began)//押されたタイミングなら
				{
					NoteMaster.addTouchRecord(touchPos2.position.x, touchPos2.position.y, nowTime);//タッチ情報を記録するリストに1行追加
					touchPos3.x = touchPos2.position.x;//画面のタッチされた位置(2次元)を3次元情報に変換
					touchPos3.y = touchPos2.position.y;
					touchPos3.z = 0f;
					worldPos = Camera.main.ScreenToWorldPoint(touchPos3 + Camera.main.transform.forward);//3次元情報からオブジェクト空間内の位置情報へ
					InputJudge(worldPos.z);

				}
			}

		}
	}
	public void InputJudge(float inputPos)//入力されたオブジェクト空間でのz軸の位置をもとに判定を行う
	{
		
		for (int i = 0; i < NoteMaster.musicData.noteList.Count; i++)
		{
			Note n = NoteMaster.musicData.noteList[i];
			Vector3 pos = n.gameObject.transform.position;
			Vector3 size = n.gameObject.transform.localScale;
			if (judgeLine.transform.position.x - getLength < pos.x && pos.x < judgeLine.transform.position.x + getLength)//縦幅
			{
				if (pos.z + (size.z / 2) > inputPos && inputPos > pos.z - (size.z / 2))//タッチした位置がオブジェクトの座標と重なった場合
				{
					
					if (n.justTime + NoteMaster.greatJudge > nowTime && nowTime > n.justTime - NoteMaster.greatJudge)//押した時間がGreatの時間内の場合
					{
						NoteMaster.great++;//greatの数を1つ増やす
						NoteMaster.score += 100;//得点を100追加
						noteMaster.JudgeTextRewrite("GREAT");//画面の表示されている判定をGREATに更新
						NoteMaster.addRecord(n.noteNum, n.justTime, nowTime, "great");//プレイログを記録するための各ノーツの判定を記録するレコードをリストにひとつ追加
						n.noteMove.StopMove();//オブジェクトの流れを止める
						n.gameObject.GetComponent<NoteFadeOut>().StartFadeOut();//オブジェクトの消失を開始
						NoteMaster.musicData.noteList.RemoveAt(i);//ノーツリストから今回判定されたノートを削除
						break;
					}
					else if (n.justTime + NoteMaster.goodJudge > nowTime && nowTime > n.justTime)//押した時間がLateの時間内の場合
					{
						NoteMaster.late++;
						NoteMaster.score += 50;
						noteMaster.JudgeTextRewrite("LATE");
						NoteMaster.addRecord(n.noteNum, n.justTime, nowTime, "late");
						n.noteMove.StopMove();
						n.gameObject.GetComponent<NoteFadeOut>().StartFadeOut();
						NoteMaster.musicData.noteList.RemoveAt(i);
						break;
					}
					else if (n.justTime > nowTime && nowTime > n.justTime - NoteMaster.goodJudge)//押した時間がFastの時間内の場合
					{
						NoteMaster.fast++;
						NoteMaster.score += 50;
						noteMaster.JudgeTextRewrite("FAST");
						NoteMaster.addRecord(n.noteNum, n.justTime, nowTime, "fast");
						n.noteMove.StopMove();
						n.gameObject.GetComponent<NoteFadeOut>().StartFadeOut();
						NoteMaster.musicData.noteList.RemoveAt(i);
						break;
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

