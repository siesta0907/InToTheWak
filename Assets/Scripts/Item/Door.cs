using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour
{
	public string stageName;	// 로드될 스테이지(Scene)의 이름

	Player player;
	GameUIManager um;

	void Awake()
	{
		player = FindObjectOfType<Player>();
		um = FindObjectOfType<GameUIManager>();
	}

	void Start()
	{
		player.OnTurnEnd += LoadNextStage;
	}

	void LoadNextStage()
	{
		StopAllCoroutines();
		StartCoroutine(LoadNextStageCoroutine());
	}

	IEnumerator LoadNextStageCoroutine()
	{
		yield return new WaitForSeconds(GameData.instance.turnDelay);
		float distance = Vector2.Distance(transform.position, player.targetPos);

		if (distance < 0.1f)
		{
			// 다음씬 로드
			player.ResetDelegate();
			um.FadeOut();
			yield return new WaitForSeconds(0.5f);
			SceneManager.LoadScene(stageName);
		}
	}
}
