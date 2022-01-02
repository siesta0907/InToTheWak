using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * 화면에 바로 보이는 UI들을 관리합니다.
 * 항상 보이는 HUD같은 것들이나, 스테이지 넘어갈때 연출인 Fade등이 예시입니다.
 */
public class GameUIManager : MonoBehaviour
{
	[Header("하위 UI")]
	[SerializeField] private Hud hud;
	[SerializeField] private GameObject fade;

	public static GameUIManager instance;

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
			Destroy(this.gameObject);
		}
		#endregion
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
