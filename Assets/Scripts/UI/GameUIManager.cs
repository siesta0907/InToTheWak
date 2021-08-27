using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUIManager : MonoBehaviour
{
	[Header("하위 UI")]
	[SerializeField] private Hud hud;
	[SerializeField] private GameObject fade;

	void Awake()
	{
		DontDestroyOnLoad(this.gameObject);
	}

	public void FadeIn()
	{
		fade.GetComponent<Animator>().SetTrigger("FadeIn");
	}

	public void FadeOut()
	{
		fade.GetComponent<Animator>().SetTrigger("FadeOut");
	}
}
