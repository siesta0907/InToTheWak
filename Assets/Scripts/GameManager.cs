using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	[Header("Timer Setting")]
	public bool activeTimer;
	public float time;


	int enemyCnt = 0;
	bool gameEnd = false;

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
		if(cnt <= 0)
		{
			GameClear();
		}
	}


	void GameClear()
	{
		Debug.Log("Game Clear!");
		activeTimer = false;
	}


	void GameOver()
	{
		Debug.Log("Game Over!");
		activeTimer = false;
	}
}
