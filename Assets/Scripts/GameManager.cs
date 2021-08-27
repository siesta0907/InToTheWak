using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
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
