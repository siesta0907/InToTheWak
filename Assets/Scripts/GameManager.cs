using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	[Header("Timer Setting")]
	public bool activeTimer;
	public float time;

	GameUIManager um;

	void Awake()
	{
		um = FindObjectOfType<GameUIManager>();
	}

	void Start()
	{
		um.FadeIn();
	}
}
