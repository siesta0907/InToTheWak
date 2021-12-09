using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameData : MonoBehaviour
{
	public static GameData instance;

	// < 게임관련 데이터 > - (EX: 클릭 딜레이, 재화 ...)
	public float turnDelay = 0.35f;     // 턴 딜레이, 턴 종료후 0.25초 뒤에 다시 턴 돌아옴
	public int decreaseSatiety = 1;     // 턴 소모시 포만감이 얼마나 줄어들지

	public int turn;    // 현재까지 흐른 턴


	void Awake()
	{
		#region Singleton
		if (instance == null)
		{
			instance = this;
			DontDestroyOnLoad(this);
		}
		else
		{
			Destroy(this);
		}
		#endregion
	}
}
