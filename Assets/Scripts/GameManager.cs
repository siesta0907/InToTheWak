using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	[Header("Timer Setting")]
	public bool activeTimer;		// 해당 스테이지에서 타이머를 사용할지 여부입니다.
									// 활성화시 스테이지에서 타이머가 작동됩니다
	public float time;				// 타이머에 부여될 시간 (초) 입니다.

	int enemyCnt = 0;				// 스테이지에 남은 개체수입니다. (Enemy 스크립트를 상속받으면 자동으로 개체수가 증가합니다)
	bool gameEnd = false;			// 게임이 종료되었는지 여부입니다.

	Player player;
	GameUIManager um;

	void Awake()
	{
		player = FindObjectOfType<Player>();
		um = FindObjectOfType<GameUIManager>();
	}


	void Start()
	{
		player.OnPlayerDead += GameOver;
		um.FadeIn();
	}


	public void AddEnemyCount(int cnt)
	{
		enemyCnt += cnt;
		if(enemyCnt <= 0)
		{
			GameClear();
		}
	}


	void GameClear()
	{
		// TODO: 이후에 지울 메세지
		Debug.Log("Game Clear!");
		activeTimer = false;
		gameEnd = true;
	}


	void GameOver()
	{
		// TODO: 이후에 지울 메세지
		Debug.Log("Game Over!");
		activeTimer = false;
		gameEnd = true;
	}
}
