using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData : MonoBehaviour
{
	public static GameData instance;

	// < 게임관련 데이터 > - (EX: 클릭 딜레이, 재화 ...)
	public float moveDelay = 0.35f;     // 이동 딜레이, 이동 후 0.25초 뒤에 다시 이동 가능
	public int increaseHunger = 1;      // 이동시 배고픔이 얼마나 올라갈지

	[SerializeField] public int turn;	// 현재까지 흐른 턴

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
